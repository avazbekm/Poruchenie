using System.Windows.Controls;

namespace Poruchenie.Pages;

/// <summary>
/// Interaction logic for YurJisMuhrsiz.xaml
/// </summary>
public partial class YurJisMuhrsiz : Page
{
    public YurJisMuhrsiz()
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
