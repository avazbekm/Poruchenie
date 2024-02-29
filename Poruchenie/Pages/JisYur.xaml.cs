using System.Windows.Controls;

namespace Poruchenie.Pages;

/// <summary>
/// Interaction logic for JisYur.xaml
/// </summary>
public partial class JisYur : Page
{
    public JisYur()
    {
        InitializeComponent();
    }

    public void ChopEt()
    {
        PrintDialog printDialog = new PrintDialog();

        if (printDialog.ShowDialog() == true)
        {
            printDialog.PrintVisual(Chiqar, "JismoniyYurdikMuhrsiz");
        }
    }
}
