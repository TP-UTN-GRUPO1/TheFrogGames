using Application.Abstraction;
using Application.Abstraction.ExternalServices;
using Application.Service;
using Azure.Identity;
using Domain.Entities;
using Infrastructure.ExternalServices;
using Infrastructure.Http;           // <-- NUEVO: Typed Client
using Infrastructure.Options;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repository;
using Infrastructure.Resilience;      
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// esto es de prod
if (builder.Environment.IsProduction())
{
    var vaultEndpoint = Environment.GetEnvironmentVariable("AZURE_KEY_VAULT_ENDPOINT");
    if (!string.IsNullOrEmpty(vaultEndpoint))
    {
        var vaultUri = new Uri(vaultEndpoint);
        builder.Configuration.AddAzureKeyVault(vaultUri, new DefaultAzureCredential());
    }
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("ConnectionStrings:DefaultConnection no está configurada. Verifica user-secrets (dev) o Key Vault (prod).");
}

builder.Services.AddDbContext<TheFrogGamesDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];

        if (string.IsNullOrEmpty(key))
        {
            throw new InvalidOperationException("Jwt:Key no está configurado. Verifica user-secrets (dev) o Key Vault (prod).");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(nameof(TypeRole.SysAdmin), policy => policy.RequireRole(nameof(TypeRole.SysAdmin)));
    options.AddPolicy(nameof(TypeRole.Admin), policy => policy.RequireRole(nameof(TypeRole.Admin)));
    options.AddPolicy(nameof(TypeRole.User), policy => policy.RequireRole(nameof(TypeRole.User)));
});


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddScoped<IPlatformService, PlatformService>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IGenreService, GenreService>();


builder.Services.Configure<GamesApiOptions>(
    builder.Configuration.GetSection("GamesApiOptions"));


builder.Services.AddHttpClient<GamesFirebaseClient>("Firebase", (sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<GamesApiOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(15);
})
// Polly
.AddPolicyHandler(PollyPolicies.GetRetryPolicy())      // Reintenta 3 veces si falla
.AddPolicyHandler(PollyPolicies.GetCircuitBreakerPolicy()); // Apaga si falla 5 veces


builder.Services.AddScoped<IExternalGameService, GamesFromFirebaseService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();