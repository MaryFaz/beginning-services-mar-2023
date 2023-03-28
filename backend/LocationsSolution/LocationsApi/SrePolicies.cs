using Polly;
using Polly.Extensions.Http;

namespace LocationsApi;

public static class SrePolicies
{
    // Retry policy - 
    public static IAsyncPolicy<HttpResponseMessage> GetDefaultRetryPolicyAsync()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    //Circuit Breaker - Quit calling for awhile.

    public static IAsyncPolicy<HttpResponseMessage> GetDefaultCircuitBreaker()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30));
    }
}
