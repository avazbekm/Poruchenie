using System.Windows.Controls;

namespace Poruchenie.Pages;

/// <summary>
/// Interaction logic for JisJisMuhrli.xaml
/// </summary>
public partial class JisJisMuhrli : Page
{
    public JisJisMuhrli()
    {
        InitializeComponent();
    }
    public void ChopEt()
    {
        PrintDialog printDialog = new PrintDialog();

        if (printDialog.ShowDialog() == true)
        {
            printDialog.PrintVisual(Chiqar, "JisJisMuhrli");
        }
    }

}
