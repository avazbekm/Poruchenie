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
        
        var oldCode = (await blockService.GetAsync()).Data.OldCode;

        if (oldCode.Equals(tbParol.Text)) 
        {
            MessageBox.Show("Bu paroldan foydalangansiz. Yangi parol kiriting");
            return;
        }
        string password = tbUsername.Text + DateTimeOffset.UtcNow.AddMinutes(7).ToString().Substring(0,16);

        try
        {
            // Verify the password
            bool isVerified = BCrypt.Net.BCrypt.Verify(password, tbParol.Text);
            if (isVerified)
            {
                var result = await blockService.UpdateAsync();
                var term = result.Data.BlockDate.ToString();
                term = term.Substring(term.Length - 10);

                if (result.StatusCode.Equals(200))
                {
                    MessageBox.Show($"{term} gacha to'liq ishlaydi.");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Muddati tugagan paroldan foydalanmoqchisiz. Yangi parol kiriting. ");
            }
        }
        catch
        {
            MessageBox.Show("Iltimos yuqoridagi manzillarga murojaat qiling. O'zgalar mehnatini qadrlang. ");
        }
    }
}

