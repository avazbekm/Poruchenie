using System.Windows;
using Poruchenie.Service.Services;
using Poruchenie.Service.Interfaces;

namespace Poruchenie;

/// <summary>
/// Interaction logic for BlockWindow.xaml
/// </summary>
public partial class BlockWindow : Window
{
    private readonly IBlockDateService blockService;

    public BlockWindow()
    {
        InitializeComponent();
        blockService = new BlockDateService();
    }

    private async void btnTasdiqlash_Click(object sender, RoutedEventArgs e)
    {
        string password = tbUsername.Text + DateTimeOffset.Now.Date.AddDays(10).ToString().Substring(0, 10);

        //var term = tbUsername.Text.Substring(tbUsername.Text.Length - 10);

        // necha oy ishlanini aniqlab olamiz
       // int months = int.Parse(term);

        // Hash the password
        //string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        try
        {
            // Verify the password
            bool isVerified = BCrypt.Net.BCrypt.Verify(password, tbParol.Text);
            if (isVerified)
            {
                var result = await blockService.UpdateAsync();
                var term =result.Data.BlockDate.ToString();
                term=term.Substring(term.Length - 10);

                if (result.StatusCode.Equals(200))
                {
                    MessageBox.Show($"{term} gacha to'liq ishlaydi.");
                    this.Close();
                }
            }
        }
        catch
        {
            MessageBox.Show("Iltimos yuqoridagi manzillarga murojaat qiling. O'zgalar mehnatini qadrlang. ");
        }
    }
}

