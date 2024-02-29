using System.Windows.Controls;

namespace Poruchenie.Pages;

/// <summary>
/// Interaction logic for YurdikMuhrli.xaml
/// </summary>
public partial class YurdikMuhrli : Page
{
    public YurdikMuhrli()
    {
        InitializeComponent();
    }

    public void ChopEt()
    {
        PrintDialog printDialog = new PrintDialog();

        if (printDialog.ShowDialog() == true)
        {
            printDialog.PrintVisual(Chiqar, "YurdikMuhrli");
        }
    }
}
