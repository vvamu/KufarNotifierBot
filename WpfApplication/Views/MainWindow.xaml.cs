
using System.Windows;
using WpfApplication.ViewModels;

namespace WpfApplication.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}