using System.IO.Packaging;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Xps.Packaging;
using System.Windows.Xps;

namespace Poruchenie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ComboBox_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                // FlowDocument yaratish
                FlowDocument flowDocument = new FlowDocument(new Paragraph(new Run("Salom, bu qog'ozga chiqarilgan matn!")));

                // FlowDocumentni XPS formatiga aylantirish
                using (MemoryStream stream = new MemoryStream())
                {
                    Package package = Package.Open(stream, FileMode.Create);
                    PackageStore.AddPackage(new Uri("pack://temp.xps"), package);

                    XpsDocument xpsDocument = new XpsDocument(package, CompressionOption.SuperFast, "pack://temp.xps");
                    XpsDocumentWriter xpsWriter = XpsDocument.CreateXpsDocumentWriter(xpsDocument);

                    xpsWriter.Write(((IDocumentPaginatorSource)flowDocument).DocumentPaginator);
                    xpsDocument.Close();

                    // Qog'ozga chiqarish
                    printDialog.PrintDocument(xpsDocument.GetFixedDocumentSequence(), "Printing WPF Document");
                }
            }
        }
    }
}