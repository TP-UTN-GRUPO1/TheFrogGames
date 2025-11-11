using Application.Abstraction;
using Application.Abstraction.ExternalServices;
using Application.Service;
using Azure.Core;
using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Domain.Entities;
using Infrastructure.ExternalServices;
using Infrastructure.Options;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repository;
//using Infrastructure.Seeding;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

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
    options.AddPolicy(name: "FrontendCorsPolicy",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddDbContext<TheFrogGamesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(key))
        {
            throw new InvalidOperationException("Jwt:Key no está configurado. Verifica Key Vault / user-secrets.");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(nameof(TypeRole.SysAdmin), policy => policy.RequireRole(nameof(TypeRole.SysAdmin)));
    options.AddPolicy(nameof(TypeRole.Admin), policy => policy.RequireRole(nameof(TypeRole.Admin)));
    options.AddPolicy(nameof(TypeRole.User), policy => policy.RequireRole(nameof(TypeRole.User)));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

builder.Services.AddHttpClient<IExternalGameService, GamesFromFirebaseService>();
builder.Services.AddScoped<GameRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("FrontendCorsPolicy");

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
