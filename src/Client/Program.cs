using Client.Infrastructure;
using Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped<CustomAuthorizationMessageHandler>();
            builder.Services.AddHttpClient<ApiService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["Api:BaseAddress"]);
            })
                .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddAuth0Authentication(options =>
            {
                builder.Configuration.Bind("Auth0", options.ProviderOptions);

                //This is set here to avoid any change via config
                options.ProviderOptions.ResponseType = "code";
            });

            await builder.Build().RunAsync();
        }
    }
}
