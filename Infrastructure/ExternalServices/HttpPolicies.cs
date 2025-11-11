using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace Infrastructure.ExternalServices
{
    public static class HttpPolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError() 
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound) 
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) 
                );
        }

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5, 
                    durationOfBreak: TimeSpan.FromSeconds(30) 
                );
        }
    }
}
