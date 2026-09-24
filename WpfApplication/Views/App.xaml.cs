using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;
using Telegram;
using WpfApplication.ViewModels;

namespace WpfApplication.Views;

public partial class App(IHost _host) : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        _host.StartAsync();

        var mainWindow = _host.Services.GetRequiredService<WpfApplication.Views.MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }
}