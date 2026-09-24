using ConsoleApplication.Commands;
using ConsoleApplication.Commands.Base;
using ConsoleApplication.Commands.Kufar;
using Kufar;
using Kufar.Handlers;
using Kufar.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Telegram;
using Telegram.Options;

public static class Program
{
    [STAThread]
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                ConfigureServices(context,services);

            })
            .ConfigureAppConfiguration(conf =>
            {
                var path = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName, "ConsoleApplication", "appsettings.json");
                conf.AddJsonFile(path);
            })
            .Build();
        
        using var scope = host.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<CommandRunner>();

        var exitCode = await runner.RunAsync(args);
        Environment.Exit(exitCode);
    }

    private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        var config = context.Configuration;
        services.AddOptions<KufarOptions>().Bind(config.GetSection("Kufar")).ValidateOnStart();
        services.AddOptions<TelegramOption>().Bind(config.GetSection("Telegram")).ValidateOnStart();

        services.AddKufar();
        services.AddSingleton<ITelegramHandler, TelegramHandler>();
       

        services.AddSingleton<ICommand, SyncCommand>();
        services.AddSingleton<ICommand, HelpCommand>();
        services.AddSingleton<ICommand, RunTelegramBotCommand>();
        services.AddSingleton<CommandRunner>();

    }


}