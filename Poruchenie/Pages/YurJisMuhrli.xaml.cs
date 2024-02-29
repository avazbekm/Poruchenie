using System.Windows.Controls;

namespace Poruchenie.Pages;

/// <summary>
/// Interaction logic for YurJisMuhrli.xaml
/// </summary>
public partial class YurJisMuhrli : Page
{
    public YurJisMuhrli()
    {
        InitializeComponent();
    }

    public void ChopEt()
    {
        PrintDialog printDialog = new PrintDialog();

        if (printDialog.ShowDialog() == true)
        {
            printDialog.PrintVisual(Chiqar, "YurJisMurhli");
        }
    }
}
