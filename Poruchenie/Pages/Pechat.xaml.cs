using System.Windows.Controls;

namespace Poruchenie.Pages;

/// <summary>
/// Interaction logic for Pechat.xaml
/// </summary>
public partial class Pechat : Page
{
    public Pechat()
    {
        InitializeComponent();
    }

    public void ChopEt()
    {
        PrintDialog printDialog = new PrintDialog();

        if (printDialog.ShowDialog() == true)
        {
            printDialog.PrintVisual(Chiqar, "YurdikMuhrsiz");
        }

    }
}
