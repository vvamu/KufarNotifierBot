using Kufar.Handlers;
using Kufar.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Kufar;

public static class KufarServiceCollectionExtensions
{
    public static IServiceCollection AddKufar(
        this IServiceCollection services,
        Action<KufarOptions>? configureOptions = null)
    {
        if(configureOptions!=null)
            services.Configure(configureOptions);

        services
            .AddOptions<KufarOptions>()
            .ValidateOnStart();

        services.AddHttpClient("kufar", (sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<KufarOptions>>().Value;

            client.BaseAddress = new Uri(options.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);

            if (!string.IsNullOrEmpty(options.UserToken))
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(options.TokenProvider, options.UserToken);
        });

        services.AddSingleton<IKufarHandler>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            var client = factory.CreateClient("kufar");
            //var logger = sp.GetRequiredService<ILogger<KufarHandler>>();
            return new KufarHandler(client);
        });

        return services;
    }
}
