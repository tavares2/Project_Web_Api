using LGC_CodeChallenge.SDK.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LGC_CodeChallenge.SDK
{
    public static class SdkClientRegistraction
    {
        public static IServiceCollection AddSdkClient(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration.GetValue<string>("SdkClient:BaseUrl");
            services.AddHttpClient<ISdkClient, SdkClient>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            return services;
        }
    }
}
