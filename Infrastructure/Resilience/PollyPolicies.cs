using Polly;
using Polly.Extensions.Http;

namespace Infrastructure.Resilience
{
    public static class PollyPolicies
    {
        
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError() 
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound) 
                .WaitAndRetryAsync(
                    retryCount: 3,// 3 intentos
                    sleepDurationProvider: intento => TimeSpan.FromSeconds(Math.Pow(2, intento))
                    // + intenta + tarda 
                  
                );
        }

        
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5, // 5 fallos
                    durationOfBreak: TimeSpan.FromSeconds(30) // 30 sec y vuelve a intentar
                );
        }
    }
}