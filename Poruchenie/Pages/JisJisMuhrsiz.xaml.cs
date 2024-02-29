using System.Windows.Controls;

namespace Poruchenie.Pages;

/// <summary>
/// Interaction logic for JisJisMuhrsiz.xaml
/// </summary>
public partial class JisJisMuhrsiz : Page
{
    public JisJisMuhrsiz()
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
