using System.Windows;
using Poruchenie.Service.Services;
using Poruchenie.Service.Interfaces;

namespace Poruchenie;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IBlockDateService blockDateService;

    public App()
    {
        InitializeComponent();
        blockDateService = new BlockDateService();

        var result = blockDateService.GetAsync().Result.Data;
        if (result is null)
        {
            var endDate = blockDateService.CreateAsync().Result.StatusCode;
            if (!endDate.Equals(200))
                MessageBox.Show("Xatolik sodir bo'ldi.");
        }
    }
}
