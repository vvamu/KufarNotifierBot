using Kufar;
using Kufar.Handlers;
using Kufar.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Telegram;
using Telegram.Options;
using WpfApplication.ViewModels;
using WpfApplication.Views;

namespace WpfApplication;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                ConfigureServices(context, services);
            })
            .ConfigureAppConfiguration(conf =>
            {
                var path = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName, "ConsoleApplication", "appsettings.json");
                conf.AddJsonFile(path);
            })
            .Build();

        host.Start();

        var app = new App(host);
        app.InitializeComponent();
        app.Run();

        host.StopAsync().GetAwaiter().GetResult();
        host.Dispose();
    }

    private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        var config = context.Configuration;
        services.AddOptions<KufarOptions>().Bind(config.GetSection("Kufar")).ValidateOnStart();
        services.AddOptions<TelegramOption>().Bind(config.GetSection("Telegram")).ValidateOnStart();

        services.AddKufar();
        services.AddSingleton<ITelegramHandler, TelegramHandler>();

        // ViewModels
        services.AddSingleton<MainWindowViewModel>();

        // Views
        services.AddSingleton<MainWindow>();
    }
}