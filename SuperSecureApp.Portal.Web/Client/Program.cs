using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SuperSecureApp.Portal.Web;
using SuperSecureApp.Portal.Web.Services;

namespace SuperSecureApp.Portal.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");


            // todosgw: added to allow easy access to tokens throughout the app using DI
            builder.Services.AddScoped<AccessTokenService>(); 
            
            // todosgw: added to allow easy access to httpclient with the additional bearer token header
            builder.Services.AddScoped(sp =>
            {
                var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
                var accessTokenService = sp.GetRequiredService<AccessTokenService>();
                var accessToken = accessTokenService.GetAccessTokenAsync().Result;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken); ;
                }
                return httpClient;
            });

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
            });

            await builder.Build().RunAsync();
        }
    }
}