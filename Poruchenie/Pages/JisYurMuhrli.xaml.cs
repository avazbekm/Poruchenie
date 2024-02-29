using System.Windows.Controls;

namespace Poruchenie.Pages;

/// <summary>
/// Interaction logic for JisYurMuhrli.xaml
/// </summary>
public partial class JisYurMuhrli : Page
{
    public JisYurMuhrli()
    {
        InitializeComponent();
    }
    public void ChopEt()
    {
        PrintDialog printDialog = new PrintDialog();

        if (printDialog.ShowDialog() == true)
        {
            printDialog.PrintVisual(Chiqar, "JismoniyYurdikMuhrli");
        }
    }
}
