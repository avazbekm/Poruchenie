using System.Windows;
using Poruchenie.Pages;
using Poruchenie.Helpers;
using System.Windows.Controls;
using Poruchenie.Domain.Etities;
using Poruchenie.Service.Services;
using Poruchenie.Service.Interfaces;
using System.Windows.Media;

namespace Poruchenie;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly IYurdikService yurdikService;
    private readonly Yurdik yurdik = new Yurdik();

    private readonly IJismoniyService jismoniyService;
    private readonly Jismoniy jismoniy = new Jismoniy();

    Pechat pechat = new Pechat();
    YurdikMuhrli yurdikMuhrli = new YurdikMuhrli();
    JisYur jisYur = new JisYur();
    JisYurMuhrli jisYurMuhrli = new JisYurMuhrli();

    public MainWindow()
    {
        InitializeComponent();
        yurdikService = new YurdikService();
        jismoniyService = new JismoniyService();
    }

    private void btnExit(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private async void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        string text = textBox.Text;
        Pechat pch = new Pechat();

        string selectedValue = ((ComboBoxItem)cbYurdik.SelectedItem).Content.ToString();

        // Faqatgina raqamlar kiritilishi mumkin
        foreach (char character in text)
        {
            if (!char.IsDigit(character))
            {
                // Agar raqam, chiziqcha yoki chiziq kiritilmagan bo'lsa, uning kiritilishini to'xtatamiz
                textBox.Text = text.Replace(character.ToString(), "");
                textBox.CaretIndex = text.Length;
                return;
            }
        }

        // 20 ta raqam to'liq kiritilganligini tekshiramiz
        if (text.Length == 20 && selectedValue.Equals("Yurdik shaxs"))
        {
            var existCount = await yurdikService.GetByCountNumberAsync(text.ToString());
            if (existCount.Data != null)
            {
                tbTulovchiBank.Text = existCount.Data.Bank;
                tbTulovchiInn.Text = existCount.Data.INN.ToString();

                tbTulovchiNomi.Text = existCount.Data.Name;
                pch.tbTulovchi.Text = tbTulovchiNomi.Text;

                tbTulovchiMfo.Text = existCount.Data.MFO;

                if (tbRahbar.Text != null)
                    tbRahbar.Text = existCount.Data.Rahbar;
                else tbRahbar.Text = string.Empty;

                if (tbBoshXisobchi.Text != null)
                    tbBoshXisobchi.Text = existCount.Data.BoshXisobchi;
                else tbBoshXisobchi.Text = string.Empty;

            }

            // Agar 20 ta raqam to'liq kiritilgan bo'lsa, texboxga kiritishni to'xtatamiz
            textBox.TextChanged -= TextBox_TextChanged;
        }
        // 20 ta raqam to'liq kiritilganligini tekshiramiz
        if (text.Length == 20 && selectedValue.Equals("Jismoniy shaxs"))
        {
            var existCountJismoniy = await jismoniyService.GetByCountNumberAsync(text.ToString());
            if (existCountJismoniy.Data != null)
            {
                tbTulovchiBank2.Text = existCountJismoniy.Data.Bank;
                tbTulovchiJshshir.Text = existCountJismoniy.Data.PINFL.ToString();

                tbTulovchiNomi2.Text = existCountJismoniy.Data.Name;
                pch.tbTulovchi.Text = tbTulovchiNomi.Text;

                tbTulovchiMfo2.Text = existCountJismoniy.Data.MFO;
                tbRahbar.Text = existCountJismoniy.Data.Rahbar;
                tbBoshXisobchi.Text = existCountJismoniy.Data.Rahbar;

            }

            // Agar 20 ta raqam to'liq kiritilgan bo'lsa, texboxga kiritishni to'xtatamiz
            textBox.TextChanged -= TextBox_TextChanged;
        }

        // Raqamni boshqasiga almashtirsa, texboxda qolgan 20 raqam kiritishni ta'minlaymiz
        textBox.TextChanged += TextBox_TextChanged;

    }

    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
        string selectedValue = ((ComboBoxItem)cbYurdik.SelectedItem).Content.ToString();

        if (selectedValue.Equals("Yurdik shaxs"))
        {
            yurdik.Name = tbTulovchiNomi.Text;
            yurdik.Bank = tbTulovchiBank.Text;
            yurdik.CreatedAt = DateTime.UtcNow;
            yurdik.CountNumber = tbTulovchiXr.Text;
            yurdik.INN = Convert.ToInt32(tbTulovchiInn.Text);
            yurdik.MFO = tbTulovchiMfo.Text;
            yurdik.Rahbar = tbRahbar.Text;
            yurdik.BoshXisobchi = tbBoshXisobchi.Text;

            await yurdikService.CreateAsync(yurdik);
        }
        else if (selectedValue.Equals("Jismoniy shaxs"))
        {
            jismoniy.Name = tbTulovchiNomi2.Text;
            jismoniy.Bank = tbTulovchiBank2.Text;
            jismoniy.CreatedAt = DateTime.UtcNow;
            jismoniy.CountNumber = tbTulovchiXr2.Text;
            jismoniy.PINFL = tbTulovchiJshshir.Text;
            jismoniy.MFO = tbTulovchiMfo2.Text;
            jismoniy.Rahbar = tbRahbar.Text;

            await jismoniyService.CreateAsync(jismoniy);
        }
    }

    private void btnPechat(object sender, RoutedEventArgs e)
    {
        string selectedValue = ((ComboBoxItem)Muhr.SelectedItem).Content.ToString();

        // yurdikdan yurdikka muhrli bo'lganda
        if (selectedValue.Equals("Muhrli") &&
            Yurdik.Visibility == Visibility.Visible &&
            Yurdik1.Visibility == Visibility.Visible)
        {
            #region Porucheniyani yuqori qismi

            yurdikMuhrli.tbNumber.Text = $"TO'LOV TOPSHIRIQNOMA   № {tbMainNuber.Text}";

            // DatePickerdan sana olish
            DateTime? selectedDate = datePicker.SelectedDate;
            DateTime? selectedDate4 = datePicker4.SelectedDate;


            // Agar sana tanlangan bo'lsa, uni textBlockga o'rnatish
            if (selectedDate.HasValue && selectedDate4.HasValue)
            {
                yurdikMuhrli.tbSana.Text = $"   Xujjat sanasi    {selectedDate.Value.ToShortDateString()}" +
                    $"                                                     Valyutalashtirish sanasi    {selectedDate4.Value.ToShortDateString()}";
            }
             
            yurdikMuhrli.tbTulovchi.Text = $"{tbTulovchiNomi.Text,30}";
            yurdikMuhrli.tbInn.Text = tbTulovchiInn.Text;
            yurdikMuhrli.tbTulovchiXr.Text = tbTulovchiXr.Text;
            yurdikMuhrli.tbTolovchiMfo.Text = tbTulovchiMfo.Text;
            yurdikMuhrli.tbTulovchiBank.Text = tbTulovchiBank.Text;

            yurdikMuhrli.tbSumma.Text = tbSumma.Text;

            yurdikMuhrli.tbOluvchi.Text = tbOluvchiNomi.Text;
            yurdikMuhrli.tbOluvchiInn.Text = tbOluvchiInn.Text;
            yurdikMuhrli.tbOluvchiXr.Text = tbOluvchiXr.Text;
            yurdikMuhrli.tbOluvchiBank.Text = tbOluvchiBank.Text;
            yurdikMuhrli.tbOluvchiMfo.Text = tbOluvchiMfo.Text;

            yurdikMuhrli.tbSummaSoz.Text = tbSummaSoz.Text;
            yurdikMuhrli.tbTulovMaqsadi.Text = $"{tbTulovMaqsad.Text}";
            yurdikMuhrli.tbRahbar.Text = tbRahbar.Text;
            yurdikMuhrli.tbBoshXisobchi.Text = tbBoshXisobchi.Text;
            #endregion

            #region Porucheniyani paskqi qismi
            yurdikMuhrli.tbNumber1.Text = $"TO'LOV TOPSHIRIQNOMA   № {tbMainNuber.Text}";

            // Agar sana tanlangan bo'lsa, uni textBlockga o'rnatish
            if (selectedDate.HasValue && selectedDate4.HasValue)
            {
                yurdikMuhrli.tbSana1.Text = $"      Xujjat sanasi    {selectedDate.Value.ToShortDateString()}" +
                    $"                                                     Valyutalashtirish sanasi    {selectedDate4.Value.ToShortDateString()}";
            }

            yurdikMuhrli.tbTulovchi1.Text = $"{tbTulovchiNomi.Text,30}";
            yurdikMuhrli.tbInn1.Text = tbTulovchiInn.Text;
            yurdikMuhrli.tbTulovchiXr1.Text = tbTulovchiXr.Text;
            yurdikMuhrli.tbTolovchiMfo1.Text = tbTulovchiMfo.Text;
            yurdikMuhrli.tbTulovchiBank1.Text = tbTulovchiBank.Text;

            yurdikMuhrli.tbSumma1.Text = tbSumma.Text;

            yurdikMuhrli.tbOluvchi1.Text = tbOluvchiNomi.Text;
            yurdikMuhrli.tbOluvchiInn1.Text = tbOluvchiInn.Text;
            yurdikMuhrli.tbOluvchiXr1.Text = tbOluvchiXr.Text;
            yurdikMuhrli.tbOluvchiBank1.Text = tbOluvchiBank.Text;
            yurdikMuhrli.tbOluvchiMfo1.Text = tbOluvchiMfo.Text;

            yurdikMuhrli.tbSummaSoz1.Text = tbSummaSoz.Text;
            yurdikMuhrli.tbTulovMaqsadi1.Text = $"{tbTulovMaqsad.Text}";
            yurdikMuhrli.tbRahbar1.Text = tbRahbar.Text;
            yurdikMuhrli.tbBoshXisobchi1.Text = tbBoshXisobchi.Text;
            #endregion


            yurdikMuhrli.ChopEt();
        }

        // yurdikdan yurdikka muhrsiz bo'lganda
        else if (selectedValue.Equals("Muhrsiz") &&
            Yurdik.Visibility == Visibility.Visible &&
            Yurdik1.Visibility == Visibility.Visible)
        {
            #region Porucheniyani yuqori qismi

            pechat.tbNumber.Text = $"TO'LOV TOPSHIRIQNOMA   № {tbMainNuber.Text}";

            // DatePickerdan sana olish
            DateTime? selectedDate = datePicker.SelectedDate;
            DateTime? selectedDate4 = datePicker4.SelectedDate;

            // Agar sana tanlangan bo'lsa, uni textBlockga o'rnatish
            if (selectedDate.HasValue && selectedDate4.HasValue)
            {
                pechat.tbSana.Text = $"     Xujjat sanasi    {selectedDate.Value.ToShortDateString()}" +
                    $"                                                     Valyutalashtirish sanasi    {selectedDate4.Value.ToShortDateString()}";
            }

            pechat.tbTulovchi.Text = $"{tbTulovchiNomi.Text,30}";
            pechat.tbInn.Text = tbTulovchiInn.Text;
            pechat.tbTulovchiXr.Text = tbTulovchiXr.Text;
            pechat.tbTolovchiMfo.Text = tbTulovchiMfo.Text;
            pechat.tbTulovchiBank.Text = tbTulovchiBank.Text;

            pechat.tbSumma.Text = tbSumma.Text;

            pechat.tbOluvchi.Text = tbOluvchiNomi.Text;
            pechat.tbOluvchiInn.Text = tbOluvchiInn.Text;
            pechat.tbOluvchiXr.Text = tbOluvchiXr.Text;
            pechat.tbOluvchiBank.Text = tbOluvchiBank.Text;
            pechat.tbOluvchiMfo.Text = tbOluvchiMfo.Text;

            pechat.tbSummaSoz.Text = tbSummaSoz.Text;
            pechat.tbTulovMaqsadi.Text = $"{tbTulovMaqsad.Text}";
            pechat.tbRahbar.Text = tbRahbar.Text;
            pechat.tbBoshXisobchi.Text = tbBoshXisobchi.Text;
            #endregion

            #region Porucheniyani paskqi qismi
            pechat.tbNumber1.Text = $"TO'LOV TOPSHIRIQNOMA   № {tbMainNuber.Text}";

            // Agar sana tanlangan bo'lsa, uni textBlockga o'rnatish
            if (selectedDate.HasValue && selectedDate4.HasValue)
            {
                pechat.tbSana1.Text = $"      Xujjat sanasi    {selectedDate.Value.ToShortDateString()}" +
                    $"                                                     Valyutalashtirish sanasi    {selectedDate4.Value.ToShortDateString()}";
            }

            pechat.tbTulovchi1.Text = $"{tbTulovchiNomi.Text,30}";
            pechat.tbInn1.Text = tbTulovchiInn.Text;
            pechat.tbTulovchiXr1.Text = tbTulovchiXr.Text;
            pechat.tbTolovchiMfo1.Text = tbTulovchiMfo.Text;
            pechat.tbTulovchiBank1.Text = tbTulovchiBank.Text;

            pechat.tbSumma1.Text = tbSumma.Text;

            pechat.tbOluvchi1.Text = tbOluvchiNomi.Text;
            pechat.tbOluvchiInn1.Text = tbOluvchiInn.Text;
            pechat.tbOluvchiXr1.Text = tbOluvchiXr.Text;
            pechat.tbOluvchiBank1.Text = tbOluvchiBank.Text;
            pechat.tbOluvchiMfo1.Text = tbOluvchiMfo.Text;

            pechat.tbSummaSoz1.Text = tbSummaSoz.Text;
            pechat.tbTulovMaqsadi1.Text = $"{tbTulovMaqsad.Text}";
            pechat.tbRahbar1.Text = tbRahbar.Text;
            pechat.tbBoshXisobchi1.Text = tbBoshXisobchi.Text;
            #endregion

            pechat.ChopEt();
        }

        // YaTT dan yudikka muhrli bo'lganda
        else if (selectedValue.Equals("Muhrli") &&
            Jismoniy.Visibility == Visibility.Visible &&
            Yurdik1.Visibility == Visibility.Visible)
        {
            #region Porucheniyani yuqori qismi

            jisYurMuhrli.tbNumber.Text = $"TO'LOV TOPSHIRIQNOMA   № {tbMainNuber2.Text}";

            // DatePickerdan sana olish
            DateTime? selectedDate = datePicker2.SelectedDate;
            DateTime? selectedDate3 = datePicker3.SelectedDate;

            // Agar sana tanlangan bo'lsa, uni textBlockga o'rnatish
            if (selectedDate.HasValue && selectedDate3.HasValue)
            {
                jisYurMuhrli.tbSana.Text = $"      Xujjat sanasi    {selectedDate.Value.ToShortDateString()}" +
                    $"                                                     Valyutalashtirish sanasi    {selectedDate3.Value.ToShortDateString()}";
            }

            jisYurMuhrli.tbTulovchi.Text = $"{tbTulovchiNomi2.Text,30}";
            jisYurMuhrli.tbInn.Text = tbTulovchiJshshir.Text;
            jisYurMuhrli.tbTulovchiXr.Text = tbTulovchiXr2.Text;
            jisYurMuhrli.tbTolovchiMfo.Text = tbTulovchiMfo2.Text;
            jisYurMuhrli.tbTulovchiBank.Text = tbTulovchiBank2.Text;

            jisYurMuhrli.tbSumma.Text = tbSumma2.Text;

            jisYurMuhrli.tbOluvchi.Text = tbOluvchiNomi.Text;
            jisYurMuhrli.tbOluvchiInn.Text = tbOluvchiInn.Text;
            jisYurMuhrli.tbOluvchiXr.Text = tbOluvchiXr.Text;
            jisYurMuhrli.tbOluvchiBank.Text = tbOluvchiBank.Text;
            jisYurMuhrli.tbOluvchiMfo.Text = tbOluvchiMfo.Text;

            jisYurMuhrli.tbSummaSoz.Text = tbSummaSoz.Text;
            jisYurMuhrli.tbTulovMaqsadi.Text = $"{tbTulovMaqsad.Text}";
            jisYurMuhrli.tbRahbar.Text = tbRahbar.Text;
            jisYurMuhrli.tbBoshXisobchi.Text = tbBoshXisobchi.Text;
            #endregion

            #region Porucheniyani paskqi qismi
            jisYurMuhrli.tbNumber1.Text = $"TO'LOV TOPSHIRIQNOMA   № {tbMainNuber2.Text}";

            // Agar sana tanlangan bo'lsa, uni textBlockga o'rnatish
            if (selectedDate.HasValue && selectedDate3.HasValue)
            {
                jisYurMuhrli.tbSana1.Text = $"      Xujjat sanasi    {selectedDate.Value.ToShortDateString()}" +
                    $"                                                     Valyutalashtirish sanasi    {selectedDate3.Value.ToShortDateString()}";
            }

            jisYurMuhrli.tbTulovchi1.Text = $"{tbTulovchiNomi2.Text,30}";
            jisYurMuhrli.tbInn1.Text = tbTulovchiJshshir.Text;
            jisYurMuhrli.tbTulovchiXr1.Text = tbTulovchiXr2.Text;
            jisYurMuhrli.tbTolovchiMfo1.Text = tbTulovchiMfo2.Text;
            jisYurMuhrli.tbTulovchiBank1.Text = tbTulovchiBank2.Text;

            jisYurMuhrli.tbSumma1.Text = tbSumma2.Text;

            jisYurMuhrli.tbOluvchi1.Text = tbOluvchiNomi.Text;
            jisYurMuhrli.tbOluvchiInn1.Text = tbOluvchiInn.Text;
            jisYurMuhrli.tbOluvchiXr1.Text = tbOluvchiXr.Text;
            jisYurMuhrli.tbOluvchiBank1.Text = tbOluvchiBank.Text;
            jisYurMuhrli.tbOluvchiMfo1.Text = tbOluvchiMfo.Text;

            jisYurMuhrli.tbSummaSoz1.Text = tbSummaSoz.Text;
            jisYurMuhrli.tbTulovMaqsadi1.Text = $"{tbTulovMaqsad.Text}";
            jisYurMuhrli.tbRahbar1.Text = tbRahbar.Text;
            jisYurMuhrli.tbBoshXisobchi1.Text = tbBoshXisobchi.Text;
            #endregion

            jisYurMuhrli.ChopEt();
        }

        // YaTT dan yudikka muhrsiz bo'lganda
        else if (selectedValue.Equals("Muhrsiz") &&
            Jismoniy.Visibility == Visibility.Visible &&
            Yurdik1.Visibility == Visibility.Visible)
        {
            #region Porucheniyani yuqori qismi

            jisYur.tbNumber.Text = $"TO'LOV TOPSHIRIQNOMA   № {tbMainNuber2.Text}";

            // DatePickerdan sana olish
            DateTime? selectedDate = datePicker2.SelectedDate;
            DateTime? selectedDate3 = datePicker3.SelectedDate;

            // Agar sana tanlangan bo'lsa, uni textBlockga o'rnatish
            if (selectedDate.HasValue && selectedDate3.HasValue)
            {
                jisYur.tbSana.Text = $"      Xujjat sanasi    {selectedDate.Value.ToShortDateString()}" +
                    $"                                                 Valyutalashtirish sanasi    {selectedDate3.Value.ToShortDateString()}";
            }

            jisYur.tbTulovchi.Text = $"{tbTulovchiNomi2.Text,30}";
            jisYur.tbInn.Text = tbTulovchiJshshir.Text;
            jisYur.tbTulovchiXr.Text = tbTulovchiXr2.Text;
            jisYur.tbTolovchiMfo.Text = tbTulovchiMfo2.Text;
            jisYur.tbTulovchiBank.Text = tbTulovchiBank2.Text;

            jisYur.tbSumma.Text = tbSumma2.Text;

            jisYur.tbOluvchi.Text = tbOluvchiNomi.Text;
            jisYur.tbOluvchiInn.Text = tbOluvchiInn.Text;
            jisYur.tbOluvchiXr.Text = tbOluvchiXr.Text;
            jisYur.tbOluvchiBank.Text = tbOluvchiBank.Text;
            jisYur.tbOluvchiMfo.Text = tbOluvchiMfo.Text;

            jisYur.tbSummaSoz.Text = tbSummaSoz.Text;
            jisYur.tbTulovMaqsadi.Text = $"{tbTulovMaqsad.Text}";
            jisYur.tbRahbar.Text = tbRahbar.Text;
            jisYur.tbBoshXisobchi.Text = tbBoshXisobchi.Text;
            #endregion

            #region Porucheniyani paskqi qismi
            jisYur.tbNumber1.Text = $"TO'LOV TOPSHIRIQNOMA   № {tbMainNuber2.Text}";

            // Agar sana tanlangan bo'lsa, uni textBlockga o'rnatish
            if (selectedDate.HasValue && selectedDate3.HasValue)
            {
                jisYur.tbSana1.Text = $"      Xujjat sanasi    {selectedDate.Value.ToShortDateString()}" +
                    $"                                                  Valyutalashtirish sanasi    {selectedDate3.Value.ToShortDateString()}";
            }

            jisYur.tbTulovchi1.Text = $"{tbTulovchiNomi2.Text,30}";
            jisYur.tbInn1.Text = tbTulovchiJshshir.Text;
            jisYur.tbTulovchiXr1.Text = tbTulovchiXr2.Text;
            jisYur.tbTolovchiMfo1.Text = tbTulovchiMfo2.Text;
            jisYur.tbTulovchiBank1.Text = tbTulovchiBank2.Text;

            jisYur.tbSumma1.Text = tbSumma2.Text;

            jisYur.tbOluvchi1.Text = tbOluvchiNomi.Text;
            jisYur.tbOluvchiInn1.Text = tbOluvchiInn.Text;
            jisYur.tbOluvchiXr1.Text = tbOluvchiXr.Text;
            jisYur.tbOluvchiBank1.Text = tbOluvchiBank.Text;
            jisYur.tbOluvchiMfo1.Text = tbOluvchiMfo.Text;

            jisYur.tbSummaSoz1.Text = tbSummaSoz.Text;
            jisYur.tbTulovMaqsadi1.Text = $"{tbTulovMaqsad.Text}";
            jisYur.tbRahbar1.Text = tbRahbar.Text;
            jisYur.tbBoshXisobchi1.Text = tbBoshXisobchi.Text;
            #endregion

            jisYur.ChopEt();
        }

        // YaTT dan YaTTga muhrsiz bo'lganda
        else if (selectedValue.Equals("Muhrsiz") &&
            Jismoniy.Visibility == Visibility.Visible &&
            Jismoniy1.Visibility == Visibility.Visible)
        {
            JisJisMuhrsiz jisJisMuhrsiz = new JisJisMuhrsiz();

            #region Porucheniyani yuqori qismi

            jisJisMuhrsiz.tbNumber.Text = $"TO'LOV TOPSHIRIQNOMA   № {tbMainNuber2.Text}";

            // DatePickerdan sana olish
            DateTime? selectedDate = datePicker2.SelectedDate;
            DateTime? selectedDate3 = datePicker3.SelectedDate;

            // Agar sana tanlangan bo'lsa, uni textBlockga o'rnatish
            if (selectedDate.HasValue && selectedDate3.HasValue)
            {
                jisJisMuhrsiz.tbSana.Text = $"      Xujjat sanasi    {selectedDate.Value.ToShortDateString()}" +
                    $"                                                     Valyutalashtirish sanasi    {selectedDate3.Value.ToShortDateString()}";
            }

            jisJisMuhrsiz.tbTulovchi.Text = $"{tbTulovchiNomi2.Text,30}";
            jisJisMuhrsiz.tbInn.Text = tbTulovchiJshshir.Text;
            jisJisMuhrsiz.tbTulovchiXr.Text = tbTulovchiXr2.Text;
            jisJisMuhrsiz.tbTolovchiMfo.Text = tbTulovchiMfo2.Text;
            jisJisMuhrsiz.tbTulovchiBank.Text = tbTulovchiBank2.Text;

            jisJisMuhrsiz.tbSumma.Text = tbSumma2.Text;

            jisJisMuhrsiz.tbOluvchi.Text = tbOluvchiNomi1.Text;
            jisJisMuhrsiz.tbOluvchiInn.Text = tbOluvchiJshshir.Text;
            jisJisMuhrsiz.tbOluvchiXr.Text = tbOluvchiXr1.Text;
            jisJisMuhrsiz.tbOluvchiBank.Text = tbOluvchiBank1.Text;
            jisJisMuhrsiz.tbOluvchiMfo.Text = tbOluvchiMfo1.Text;

            jisJisMuhrsiz.tbSummaSoz.Text = tbSummaSoz.Text;
            jisJisMuhrsiz.tbTulovMaqsadi.Text = $"{tbTulovMaqsad.Text}";
            jisJisMuhrsiz.tbRahbar.Text = tbRahbar.Text;
            jisJisMuhrsiz.tbBoshXisobchi.Text = tbBoshXisobchi.Text;
            #endregion

            #region Porucheniyani paskqi qismi
            jisJisMuhrsiz.tbNumber1.Text = $"TO'LOV TOPSHIRIQNOMA   № {tbMainNuber2.Text}";

            // Agar sana tanlangan bo'lsa, uni textBlockga o'rnatish
            if (selectedDate.HasValue && selectedDate3.HasValue)
            {
                jisJisMuhrsiz.tbSana1.Text = $"      Xujjat sanasi    {selectedDate.Value.ToShortDateString()}" +
                    $"                                                    Valyutalashtirish sanasi    {selectedDate3.Value.ToShortDateString()}";
            }

            jisJisMuhrsiz.tbTulovchi1.Text = $"{tbTulovchiNomi2.Text,30}";
            jisJisMuhrsiz.tbInn1.Text = tbTulovchiJshshir.Text;
            jisJisMuhrsiz.tbTulovchiXr1.Text = tbTulovchiXr2.Text;
            jisJisMuhrsiz.tbTolovchiMfo1.Text = tbTulovchiMfo2.Text;
            jisJisMuhrsiz.tbTulovchiBank1.Text = tbTulovchiBank2.Text;

            jisJisMuhrsiz.tbSumma1.Text = tbSumma2.Text;

            jisJisMuhrsiz.tbOluvchi1.Text = tbOluvchiNomi1.Text;
            jisJisMuhrsiz.tbOluvchiInn1.Text = tbOluvchiJshshir.Text;
            jisJisMuhrsiz.tbOluvchiXr1.Text = tbOluvchiXr1.Text;
            jisJisMuhrsiz.tbOluvchiBank1.Text = tbOluvchiBank1.Text;
            jisJisMuhrsiz.tbOluvchiMfo1.Text = tbOluvchiMfo1.Text;

            jisJisMuhrsiz.tbSummaSoz1.Text = tbSummaSoz.Text;
            jisJisMuhrsiz.tbTulovMaqsadi1.Text = $"{tbTulovMaqsad.Text}";
            jisJisMuhrsiz.tbRahbar1.Text = tbRahbar.Text;
            jisJisMuhrsiz.tbBoshXisobchi1.Text = tbBoshXisobchi.Text;
            #endregion

            jisJisMuhrsiz.ChopEt();
        }

        // YaTT dan YaTTga muhrli bo'lganda
        else if (selectedValue.Equals("Muhrli") &&
            Jismoniy.Visibility == Visibility.Visible &&
            Jismoniy1.Visibility == Visibility.Visible)
        {
            JisJisMuhrli jisJisMuhrli = new JisJisMuhrli();

            #region Porucheniyani yuqori qismi

            jisJisMuhrli.tbNumber.Text = $"TO'LOV TOPSHIRIQNOMA   № {tbMainNuber2.Text}";

            // DatePickerdan sana olish
            DateTime? selectedDate = datePicker2.SelectedDate;
            DateTime? selectedDate3 = datePicker3.SelectedDate;

            // Agar sana tanlangan bo'lsa, uni textBlockga o'rnatish
            if (selectedDate.HasValue && selectedDate3.HasValue)
            {
                jisJisMuhrli.tbSana.Text = $"      Xujjat sanasi    {selectedDate.Value.ToShortDateString()}" +
                    $"                                                     Valyutalashtirish sanasi    {selectedDate3.Value.ToShortDateString()}";
            }

            jisJisMuhrli.tbTulovchi.Text = $"{tbTulovchiNomi2.Text,30}";
            jisJisMuhrli.tbInn.Text = tbTulovchiJshshir.Text;
            jisJisMuhrli.tbTulovchiXr.Text = tbTulovchiXr2.Text;
            jisJisMuhrli.tbTolovchiMfo.Text = tbTulovchiMfo2.Text;
            jisJisMuhrli.tbTulovchiBank.Text = tbTulovchiBank2.Text;

            jisJisMuhrli.tbSumma.Text = tbSumma2.Text;

            jisJisMuhrli.tbOluvchi.Text = tbOluvchiNomi1.Text;
            jisJisMuhrli.tbOluvchiInn.Text = tbOluvchiJshshir.Text;
            jisJisMuhrli.tbOluvchiXr.Text = tbOluvchiXr1.Text;
            jisJisMuhrli.tbOluvchiBank.Text = tbOluvchiBank1.Text;
            jisJisMuhrli.tbOluvchiMfo.Text = tbOluvchiMfo1.Text;

            jisJisMuhrli.tbSummaSoz.Text = tbSummaSoz.Text;
            jisJisMuhrli.tbTulovMaqsadi.Text = $"{tbTulovMaqsad.Text}";
            jisJisMuhrli.tbRahbar.Text = tbRahbar.Text;
            jisJisMuhrli.tbBoshXisobchi.Text = tbBoshXisobchi.Text;
            #endregion

            #region Porucheniyani paskqi qismi
            jisJisMuhrli.tbNumber1.Text = $"TO'LOV TOPSHIRIQNOMA   № {tbMainNuber2.Text}";

            // Agar sana tanlangan bo'lsa, uni textBlockga o'rnatish
            if (selectedDate.HasValue && selectedDate3.HasValue)
            {
                jisJisMuhrli.tbSana1.Text = $"      Xujjat sanasi    {selectedDate.Value.ToShortDateString()}" +
                    $"                                                     Valyutalashtirish sanasi    {selectedDate3.Value.ToShortDateString()}";
            }

            jisJisMuhrli.tbTulovchi1.Text = $"{tbTulovchiNomi2.Text,30}";
            jisJisMuhrli.tbInn1.Text = tbTulovchiJshshir.Text;
            jisJisMuhrli.tbTulovchiXr1.Text = tbTulovchiXr2.Text;
            jisJisMuhrli.tbTolovchiMfo1.Text = tbTulovchiMfo2.Text;
            jisJisMuhrli.tbTulovchiBank1.Text = tbTulovchiBank2.Text;

            jisJisMuhrli.tbSumma1.Text = tbSumma2.Text;

            jisJisMuhrli.tbOluvchi1.Text = tbOluvchiNomi1.Text;
            jisJisMuhrli.tbOluvchiInn1.Text = tbOluvchiJshshir.Text;
            jisJisMuhrli.tbOluvchiXr1.Text = tbOluvchiXr1.Text;
            jisJisMuhrli.tbOluvchiBank1.Text = tbOluvchiBank1.Text;
            jisJisMuhrli.tbOluvchiMfo1.Text = tbOluvchiMfo1.Text;

            jisJisMuhrli.tbSummaSoz1.Text = tbSummaSoz.Text;
            jisJisMuhrli.tbTulovMaqsadi1.Text = $"{tbTulovMaqsad.Text}";
            jisJisMuhrli.tbRahbar1.Text = tbRahbar.Text;
            jisJisMuhrli.tbBoshXisobchi1.Text = tbBoshXisobchi.Text;
            #endregion

            jisJisMuhrli.ChopEt();
        }

        // Yurdikdan YaTTga muhrli bo'lganda
        else if (selectedValue.Equals("Muhrli") &&
            Yurdik.Visibility == Visibility.Visible &&
            Jismoniy1.Visibility == Visibility.Visible)
        {
            YurJisMuhrli yurJisMuhrli = new YurJisMuhrli();

            #region Porucheniyani yuqori qismi

            yurJisMuhrli.tbNumber.Text = $"TO'LOV TOPSHIRIQNOMA   № {tbMainNuber.Text}";

            // DatePickerdan sana olish
            DateTime? selectedDate = datePicker2.SelectedDate;
            DateTime? selectedDate4 = datePicker4.SelectedDate;

            // Agar sana tanlangan bo'lsa, uni textBlockga o'rnatish
            if (selectedDate.HasValue && selectedDate4.HasValue)
            {
                yurJisMuhrli.tbSana.Text = $"      Xujjat sanasi    {selectedDate.Value.ToShortDateString()}" +
                    $"                                                     Valyutalashtirish sanasi    {selectedDate4.Value.ToShortDateString()}";
            }

            yurJisMuhrli.tbTulovchi.Text = $"{tbTulovchiNomi.Text,30}";
            yurJisMuhrli.tbInn.Text = tbTulovchiInn.Text;
            yurJisMuhrli.tbTulovchiXr.Text = tbTulovchiXr.Text;
            yurJisMuhrli.tbTolovchiMfo.Text = tbTulovchiMfo.Text;
            yurJisMuhrli.tbTulovchiBank.Text = tbTulovchiBank.Text;

            yurJisMuhrli.tbSumma.Text = tbSumma.Text;

            yurJisMuhrli.tbOluvchi.Text = tbOluvchiNomi1.Text;
            yurJisMuhrli.tbOluvchiInn.Text = tbOluvchiJshshir.Text;
            yurJisMuhrli.tbOluvchiXr.Text = tbOluvchiXr1.Text;
            yurJisMuhrli.tbOluvchiBank.Text = tbOluvchiBank1.Text;
            yurJisMuhrli.tbOluvchiMfo.Text = tbOluvchiMfo1.Text;

            yurJisMuhrli.tbSummaSoz.Text = tbSummaSoz.Text;
            yurJisMuhrli.tbTulovMaqsadi.Text = $"{tbTulovMaqsad.Text}";
            yurJisMuhrli.tbRahbar.Text = tbRahbar.Text;
            yurJisMuhrli.tbBoshXisobchi.Text = tbBoshXisobchi.Text;
            #endregion

            #region Porucheniyani paskqi qismi
            yurJisMuhrli.tbNumber1.Text = $"TO'LOV TOPSHIRIQNOMA   № {tbMainNuber.Text}";

            // Agar sana tanlangan bo'lsa, uni textBlockga o'rnatish
            if (selectedDate.HasValue && selectedDate4.HasValue)
            {
                yurJisMuhrli.tbSana1.Text = $"      Xujjat sanasi    {selectedDate.Value.ToShortDateString()}" +
                    $"                                                     Valyutalashtirish sanasi    {selectedDate4.Value.ToShortDateString()}";
            }

            yurJisMuhrli.tbTulovchi1.Text = $"{tbTulovchiNomi.Text,30}";
            yurJisMuhrli.tbInn1.Text = tbTulovchiInn.Text;
            yurJisMuhrli.tbTulovchiXr1.Text = tbTulovchiXr.Text;
            yurJisMuhrli.tbTolovchiMfo1.Text = tbTulovchiMfo.Text;
            yurJisMuhrli.tbTulovchiBank1.Text = tbTulovchiBank.Text;

            yurJisMuhrli.tbSumma1.Text = tbSumma.Text;

            yurJisMuhrli.tbOluvchi1.Text = tbOluvchiNomi1.Text;
            yurJisMuhrli.tbOluvchiInn1.Text = tbOluvchiJshshir.Text;
            yurJisMuhrli.tbOluvchiXr1.Text = tbOluvchiXr1.Text;
            yurJisMuhrli.tbOluvchiBank1.Text = tbOluvchiBank1.Text;
            yurJisMuhrli.tbOluvchiMfo1.Text = tbOluvchiMfo1.Text;

            yurJisMuhrli.tbSummaSoz1.Text = tbSummaSoz.Text;
            yurJisMuhrli.tbTulovMaqsadi1.Text = $"{tbTulovMaqsad.Text}";
            yurJisMuhrli.tbRahbar1.Text = tbRahbar.Text;
            yurJisMuhrli.tbBoshXisobchi1.Text = tbBoshXisobchi.Text;
            #endregion

            yurJisMuhrli.ChopEt();
        }

        // Yurdikdan YaTTga muhrsiz bo'lganda
        else if (selectedValue.Equals("Muhrsiz") &&
            Yurdik.Visibility == Visibility.Visible &&
            Jismoniy1.Visibility == Visibility.Visible)
        {
            YurJisMuhrsiz yurJisMuhrsiz = new YurJisMuhrsiz();

            #region Porucheniyani yuqori qismi

            yurJisMuhrsiz.tbNumber.Text = $"TO'LOV TOPSHIRIQNOMA   № {tbMainNuber.Text}";

            // DatePickerdan sana olish
            DateTime? selectedDate = datePicker2.SelectedDate;
            DateTime? selectedDate4 = datePicker4.SelectedDate;

            // Agar sana tanlangan bo'lsa, uni textBlockga o'rnatish
            if (selectedDate.HasValue)
            {
                yurJisMuhrsiz.tbSana.Text = $"      Xujjat sanasi    {selectedDate.Value.ToShortDateString()}" +
                    $"                                                     Valyutalashtirish sanasi    {selectedDate4.Value.ToShortDateString()}";
            }

            yurJisMuhrsiz.tbTulovchi.Text = $"{tbTulovchiNomi.Text,30}";
            yurJisMuhrsiz.tbInn.Text = tbTulovchiInn.Text;
            yurJisMuhrsiz.tbTulovchiXr.Text = tbTulovchiXr.Text;
            yurJisMuhrsiz.tbTolovchiMfo.Text = tbTulovchiMfo.Text;
            yurJisMuhrsiz.tbTulovchiBank.Text = tbTulovchiBank.Text;

            yurJisMuhrsiz.tbSumma.Text = tbSumma.Text;

            yurJisMuhrsiz.tbOluvchi.Text = tbOluvchiNomi1.Text;
            yurJisMuhrsiz.tbOluvchiInn.Text = tbOluvchiJshshir.Text;
            yurJisMuhrsiz.tbOluvchiXr.Text = tbOluvchiXr1.Text;
            yurJisMuhrsiz.tbOluvchiBank.Text = tbOluvchiBank1.Text;
            yurJisMuhrsiz.tbOluvchiMfo.Text = tbOluvchiMfo1.Text;

            yurJisMuhrsiz.tbSummaSoz.Text = tbSummaSoz.Text;
            yurJisMuhrsiz.tbTulovMaqsadi.Text = $"{tbTulovMaqsad.Text}";
            yurJisMuhrsiz.tbRahbar.Text = tbRahbar.Text;
            yurJisMuhrsiz.tbBoshXisobchi.Text = tbBoshXisobchi.Text;
            #endregion

            #region Porucheniyani paskqi qismi
            yurJisMuhrsiz.tbNumber1.Text = $"TO'LOV TOPSHIRIQNOMA   № {tbMainNuber.Text}";

            // Agar sana tanlangan bo'lsa, uni textBlockga o'rnatish
            if (selectedDate.HasValue && selectedDate4.HasValue)
            {
                yurJisMuhrsiz.tbSana1.Text = $"      Xujjat sanasi    {selectedDate.Value.ToShortDateString()}" +
                    $"                                                     Valyutalashtirish sanasi    {selectedDate4.Value.ToShortDateString()}";
            }

            yurJisMuhrsiz.tbTulovchi1.Text = $"{tbTulovchiNomi.Text,30}";
            yurJisMuhrsiz.tbInn1.Text = tbTulovchiInn.Text;
            yurJisMuhrsiz.tbTulovchiXr1.Text = tbTulovchiXr.Text;
            yurJisMuhrsiz.tbTolovchiMfo1.Text = tbTulovchiMfo.Text;
            yurJisMuhrsiz.tbTulovchiBank1.Text = tbTulovchiBank.Text;

            yurJisMuhrsiz.tbSumma1.Text = tbSumma.Text;

            yurJisMuhrsiz.tbOluvchi1.Text = tbOluvchiNomi1.Text;
            yurJisMuhrsiz.tbOluvchiInn1.Text = tbOluvchiJshshir.Text;
            yurJisMuhrsiz.tbOluvchiXr1.Text = tbOluvchiXr1.Text;
            yurJisMuhrsiz.tbOluvchiBank1.Text = tbOluvchiBank1.Text;
            yurJisMuhrsiz.tbOluvchiMfo1.Text = tbOluvchiMfo1.Text;

            yurJisMuhrsiz.tbSummaSoz1.Text = tbSummaSoz.Text;
            yurJisMuhrsiz.tbTulovMaqsadi1.Text = $"{tbTulovMaqsad.Text}";
            yurJisMuhrsiz.tbRahbar1.Text = tbRahbar.Text;
            yurJisMuhrsiz.tbBoshXisobchi1.Text = tbBoshXisobchi.Text;
            #endregion

            yurJisMuhrsiz.ChopEt();
        }

    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
    }

    private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        string text = textBox.Text;

        // Faqatgina raqamlar kiritilishi mumkin
        foreach (char character in text)
        {
            if (!char.IsDigit(character))
            {
                // Agar raqamdan boshqa belgi kiritilsa o'chirlad
                textBox.Text = text.Replace(character.ToString(), "");
                textBox.CaretIndex = text.Length;
                return;
            }
        }

        // 20 ta raqam to'liq kiritilganligini tekshiramiz
        if (text.Length == 9)
        {
            // Agar 9 ta raqam to'liq kiritilgan bo'lsa, texboxga kiritishni to'xtatamiz
            textBox.TextChanged -= TextBox_TextChanged;
            return;
        }

        // Raqamni boshqasiga almashtirsa, texboxda qolgan 19 raqam kiritishni ta'minlaymiz
        textBox.TextChanged += TextBox_TextChanged;

    }

    private void TextBox_TextChanged_3(object sender, TextChangedEventArgs e)
    {

    }

    private async void TextBox_TextChanged_4(object sender, TextChangedEventArgs e)
    {

        TextBox textBox = (TextBox)sender;
        string text = textBox.Text;

        string selectedValue = ((ComboBoxItem)cbJismoniy.SelectedItem).Content.ToString();

        // Faqatgina raqamlar kiritilishi mumkin
        foreach (char character in text)
        {
            if (!char.IsDigit(character))
            {
                // Agar raqam, chiziqcha yoki chiziq kiritilmagan bo'lsa, uning kiritilishini to'xtatamiz
                textBox.Text = text.Replace(character.ToString(), "");
                textBox.CaretIndex = text.Length;
                return;
            }
        }

        // 20 ta raqam to'liq kiritilganligini tekshiramiz
        if (text.Length == 20 && selectedValue.Equals("Yurdik shaxs"))
        {
            var existCount = await yurdikService.GetByCountNumberAsync(text.ToString());
            if (existCount.Data != null)
            {
                tbOluvchiBank.Text = existCount.Data.Bank;
                tbOluvchiInn.Text = existCount.Data.INN.ToString();
                tbOluvchiNomi.Text = existCount.Data.Name;
                tbOluvchiMfo.Text = existCount.Data.MFO;
            }

        // Agar 20 ta raqam to'liq kiritilgan bo'lsa, texboxga kiritishni to'xtatamiz
            textBox.TextChanged -= TextBox_TextChanged;
            return;
        }

        if (text.Length == 20 && selectedValue.Equals("Jismoniy shaxs"))
        {
            var existCount = await jismoniyService.GetByCountNumberAsync(text.ToString());
            if (existCount.Data != null)
            {
                tbOluvchiBank1.Text = existCount.Data.Bank;
                tbOluvchiJshshir.Text = existCount.Data.PINFL;
                tbOluvchiNomi1.Text = existCount.Data.Name;
                tbOluvchiMfo1.Text = existCount.Data.MFO;
            }

            // Agar 20 ta raqam to'liq kiritilgan bo'lsa, texboxga kiritishni to'xtatamiz
            textBox.TextChanged -= TextBox_TextChanged;
            return;
        }

        // Raqamni boshqasiga almashtirsa, texboxda qolgan 19 raqam kiritishni ta'minlaymiz
        textBox.TextChanged += TextBox_TextChanged;
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        string selectedValue = ((ComboBoxItem)cbJismoniy.SelectedItem).Content.ToString();

        if (selectedValue.Equals("Yurdik shaxs"))
        {
            yurdik.Name = tbOluvchiNomi.Text;
            yurdik.Bank = tbOluvchiBank.Text;
            yurdik.CreatedAt = DateTime.UtcNow;
            yurdik.CountNumber = tbOluvchiXr.Text;
            yurdik.INN = Convert.ToInt32(tbOluvchiInn.Text);
            yurdik.MFO = tbOluvchiMfo.Text;

            await yurdikService.CreateAsync(yurdik);
        }
        else
        {
            jismoniy.Name = tbOluvchiNomi1.Text;
            jismoniy.Bank = tbOluvchiBank1.Text;
            jismoniy.CreatedAt = DateTime.UtcNow;
            jismoniy.CountNumber = tbOluvchiXr1.Text;
            jismoniy.PINFL = tbOluvchiJshshir.Text;
            jismoniy.MFO = tbOluvchiMfo1.Text;

            await jismoniyService.CreateAsync(jismoniy);
        }

    }

    private void tbOluvchiInn_TextChanged(object sender, TextChangedEventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        string text = textBox.Text;

        // Faqatgina raqamlar kiritilishi mumkin
        foreach (char character in text)
        {
            if (!char.IsDigit(character))
            {
                // Raqamlardan boshqa belgi kiritilmaydi
                textBox.Text = text.Replace(character.ToString(), "");
                textBox.CaretIndex = text.Length;
                return;
            }
        }

        // 9 ta raqam to'liq kiritilganligini tekshiramiz
        if (text.Length == 9)
        {
            // Agar 9 ta raqam to'liq kiritilgan bo'lsa, texboxga kiritishni to'xtatamiz
            textBox.TextChanged -= TextBox_TextChanged;
            return;
        }

        // Raqamni boshqasiga almashtirsa, texboxda qolgan 19 raqam kiritishni ta'minlaymiz
        textBox.TextChanged += TextBox_TextChanged;
    }

    private async void tbTulovchiMfo_TextChanged(object sender, TextChangedEventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        string text = textBox.Text;

        // Faqatgina raqamlar kiritilishi mumkin
        foreach (char character in text)
        {
            if (!char.IsDigit(character))
            {
                // Agar raqam, chiziqcha yoki chiziq kiritilmagan bo'lsa, uning kiritilishini to'xtatamiz
                textBox.Text = text.Replace(character.ToString(), "");
                textBox.CaretIndex = text.Length;
                return;
            }
        }

        // 5 ta raqam to'liq kiritilganligini tekshiramiz
        if (text.Length == 5)
        {
            if ((tbTulovchiBank.Text.Equals(string.Empty) ||
                tbTulovchiXr.Text.Equals("") ||
                !(await yurdikService.GetByCountNumberAsync(tbTulovchiXr.Text)).Message.Equals("Ok")) &&
                Yurdik.Visibility == Visibility.Visible)
                tbTulovchiBank.Text = GetByFilialiMfo.GetBankNameByMfo(text);

            else if ((tbTulovchiBank2.Text.Equals("") ||
                tbTulovchiXr2.Text.Equals("") ||
                !(await jismoniyService.GetByCountNumberAsync(tbTulovchiXr.Text)).Message.Equals("Ok")) &&
                Jismoniy.Visibility == Visibility.Visible)
                tbTulovchiBank2.Text = GetByFilialiMfo.GetBankNameByMfo(text);

            // Agar 5 ta raqam to'liq kiritilgan bo'lsa, texboxga kiritishni to'xtatamiz
            textBox.TextChanged -= TextBox_TextChanged;
            return;
        }

        // Raqamni boshqasiga almashtirsa, texboxda qolgan 5 raqam kiritishni ta'minlaymiz
        textBox.TextChanged += TextBox_TextChanged;

    }

    private async void tbOluvchiMfo_TextChanged(object sender, TextChangedEventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        string text = textBox.Text;

        // Faqatgina raqamlar kiritilishi mumkin
        foreach (char character in text)
        {
            if (!char.IsDigit(character))
            {
                // Agar raqam, chiziqcha yoki chiziq kiritilmagan bo'lsa, uning kiritilishini to'xtatamiz
                textBox.Text = text.Replace(character.ToString(), "");
                textBox.CaretIndex = text.Length;
                return;
            }
        }

        // 5 ta raqam to'liq kiritilganligini tekshiramiz
        if (text.Length == 5)
        {
            if ((tbOluvchiBank1.Text.Equals(string.Empty) ||
                tbOluvchiXr1.Text.Equals("") ||
                !(await jismoniyService.GetByCountNumberAsync(tbOluvchiXr1.Text)).Message.Equals("Ok")) &&
                Jismoniy1.Visibility == Visibility.Visible)
                tbOluvchiBank1.Text = GetByFilialiMfo.GetBankNameByMfo(text);
            else if ((tbOluvchiBank.Text.Equals("") ||
                tbOluvchiXr.Text.Equals("") ||
                !(await yurdikService.GetByCountNumberAsync(tbOluvchiXr.Text)).Message.Equals("Ok")) &&
                Yurdik1.Visibility == Visibility.Visible)
                tbOluvchiBank.Text = GetByFilialiMfo.GetBankNameByMfo(text);
            // Agar 5 ta raqam to'liq kiritilgan bo'lsa, texboxga kiritishni to'xtatamiz
            textBox.TextChanged -= TextBox_TextChanged;
            return;
        }

        // Raqamni boshqasiga almashtirsa, texboxda qolgan 19 raqam kiritishni ta'minlaymiz
        textBox.TextChanged += TextBox_TextChanged;

    }

    private void TextBox_TextChanged_5(object sender, TextChangedEventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        string text = textBox.Text;

        // Faqatgina haqiqiy sonlar kiritilishi mumkin
        string newText = "";
        foreach (char character in text)
        {
            if (char.IsDigit(character) || character == '.')
            {
                newText += character;
            }
        }

        // 001245  sonning oldiddagi nolni tozalash uchun
        if (newText.StartsWith("0"))
        {
            newText = newText.TrimStart('0');
        }

        // Barcha nuqtalarni tekshirish va faqat bir marta joylash uchun
        if (newText.IndexOf('.') != newText.LastIndexOf('.'))
        {
            newText = newText.Remove(newText.LastIndexOf('.'));
        }

        // Nuqtalar bo'yicha ikki bo'lsa, yoki sonning boshida nuqta bo'lsa, uning ikkinchi nuqtasini o'chiramiz
        if (newText.Contains('.'))
        {
            int firstDotIndex = newText.IndexOf('.');
            int lastDotIndex = newText.LastIndexOf('.');

            if (firstDotIndex != lastDotIndex || firstDotIndex == 0)
            {
                newText = newText.Remove(lastDotIndex);
            }
        }

        textBox.Text = newText;
        textBox.CaretIndex = newText.Length;


        if (Jismoniy.Visibility == Visibility.Visible)
            tbSumma.Text = tbSumma2.Text;

        // sonni so'zga aylatirishga funksiya
        if (long.TryParse(tbSumma.Text, out long number))
        {
            tbSummaSoz.Text = $"{RaqamdanSozga(number)} so'm";

            //bunda Jismoniydan Jismoniyga pul o'tkanda 
            if (Jismoniy1.Visibility == Visibility.Visible)
                tbSummaSoz1.Text = tbSummaSoz.Text;
        }
        else
        {
            if (tbSumma.Text.Length > 0)
            {
                // Nuqta indeksini topamiz
                int dotIndex = tbSumma.Text.IndexOf('.');

                // Nuqtaga qarab butun hamda qoldiqni ajratamiz
                string beforeDot = tbSumma.Text.Substring(0, dotIndex);
                string afterDot = tbSumma.Text.Substring(dotIndex + 1);

                if (long.TryParse(beforeDot, out long son))
                    tbSummaSoz.Text = $"{RaqamdanSozga(son)} so'm {afterDot} tiyin";
                //bunda Jismoniydan Jismoniyga pul o'tkanda 
                if (Jismoniy1.Visibility == Visibility.Visible)
                    tbSummaSoz1.Text = tbSummaSoz.Text;

            }
        }
    }


    public static string RaqamdanSozga(long raqam)
    {
        string words = "";

        if ((raqam / 1000000000000) > 0)
        {
            words += RaqamdanSozga(raqam / 1000000000000) + " trillion ";
            raqam %= 1000000000000;
        }

        if ((raqam / 1000000000) > 0)
        {
            words += RaqamdanSozga(raqam / 1000000000) + " milliard ";
            raqam %= 1000000000;
        }

        if ((raqam / 1000000) > 0)
        {
            words += RaqamdanSozga(raqam / 1000000) + " million ";
            raqam %= 1000000;
        }

        if ((raqam / 1000) > 0)
        {
            words += RaqamdanSozga(raqam / 1000) + " ming ";
            raqam %= 1000;
        }

        if ((raqam / 100) > 0)
        {
            words += RaqamdanSozga(raqam / 100) + " yuz ";
            raqam %= 100;
        }

        if (raqam > 0)
        {
            if (words != "")
                words += "";

            string[] birlar = { "", "bir", "ikki", "uch", "to'rt", "besh", "olti", "yetti", "sakkiz", "to'qqiz" };
            string[] onlar = { "", "o'n", "yigirma", "o'ttiz", "qirq", "ellik", "oltmish", "yetmish", "sakson", "to'qson" };
            string[] yuzlar = { "", "yuz", "ikki yuz", "uch yuz", "to'rt yuz", "besh yuz", "olti yuz", "yetti yuz", "sakkiz Yuz", "To'qqiz Yuz" };

            if (raqam < 10)
                words += birlar[raqam];
            else if (raqam < 20)
                words += "o'n " + birlar[raqam - 10];
            else
            {
                words += onlar[raqam / 10];
                if ((raqam % 10) > 0)
                    words += " " + birlar[raqam % 10];
            }
        }

        return words;
    }

    private void TextBox_TextChanged_6(object sender, TextChangedEventArgs e)
    {

    }

    private void tbMainNuber_TextChanged(object sender, TextChangedEventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        string text = textBox.Text;

        // Faqatgina raqamlar kiritilishi mumkin
        foreach (char character in text)
        {
            if (!char.IsDigit(character))
            {
                // Agar raqamdan boshqa belgi kiritilsa o'chirlad
                textBox.Text = text.Replace(character.ToString(), "");
                textBox.CaretIndex = text.Length;
                return;
            }
        }

        // 14 ta raqam to'liq kiritilganligini tekshiramiz
        if (text.Length == 5)
        {
            // Agar 5 ta raqam to'liq kiritilgan bo'lsa, texboxga kiritishni to'xtatamiz
            textBox.TextChanged -= TextBox_TextChanged;
            return;
        }

        // Raqamni boshqasiga almashtirsa, texboxda qolgan 5 raqam kiritishni ta'minlaymiz
        textBox.TextChanged += TextBox_TextChanged;

    }

    private void Jshshir(object sender, TextChangedEventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        string text = textBox.Text;

        // Faqatgina raqamlar kiritilishi mumkin
        foreach (char character in text)
        {
            if (!char.IsDigit(character))
            {
                // Agar raqamdan boshqa belgi kiritilsa o'chirlad
                textBox.Text = text.Replace(character.ToString(), "");
                textBox.CaretIndex = text.Length;
                return;
            }
        }

        // 14 ta raqam to'liq kiritilganligini tekshiramiz
        if (text.Length == 14)
        {
            // Agar 14 ta raqam to'liq kiritilgan bo'lsa, texboxga kiritishni to'xtatamiz
            textBox.TextChanged -= TextBox_TextChanged;
            return;
        }

        // Raqamni boshqasiga almashtirsa, texboxda qolgan 19 raqam kiritishni ta'minlaymiz
        textBox.TextChanged += TextBox_TextChanged;

    }

    private void cbYurdik_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string selectedValue = ((ComboBoxItem)cbYurdik.SelectedItem).Content.ToString();
        if (selectedValue.Equals("Yurdik shaxs"))
        {
            cbYudik1.SelectedIndex = 0;
            Yurdik.Visibility = Visibility.Visible;
            Jismoniy.Visibility = Visibility.Collapsed;
        }
        else
        {
            cbYudik1.SelectedIndex = 1;
            Yurdik.Visibility = Visibility.Collapsed;
            Jismoniy.Visibility = Visibility.Visible;
        }
    }

    private void cbYudik1_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string selectedValue = ((ComboBoxItem)cbYudik1.SelectedItem).Content.ToString();
        if (selectedValue.Equals("Jismoniy shaxs"))
        {
            cbYudik1.SelectedIndex = 1;
            Jismoniy.Visibility = Visibility.Visible;
            Yurdik.Visibility = Visibility.Collapsed;
        }
    }

    private void tbJshshir(object sender, TextChangedEventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        string text = textBox.Text;

        // Faqatgina raqamlar kiritilishi mumkin
        foreach (char character in text)
        {
            if (!char.IsDigit(character))
            {
                // Agar raqamdan boshqa belgi kiritilsa o'chirlad
                textBox.Text = text.Replace(character.ToString(), "");
                textBox.CaretIndex = text.Length;
                return;
            }
        }

        // 14 ta raqam to'liq kiritilganligini tekshiramiz
        if (text.Length == 14)
        {
            // Agar 14 ta raqam to'liq kiritilgan bo'lsa, texboxga kiritishni to'xtatamiz
            textBox.TextChanged -= TextBox_TextChanged;
            return;
        }

        // Raqamni boshqasiga almashtirsa, texboxda qolgan 19 raqam kiritishni ta'minlaymiz
        textBox.TextChanged += TextBox_TextChanged;
    }

    private void cbJismoniy_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string selectValue = ((ComboBoxItem)cbJismoniy.SelectedItem).Content.ToString();

        if (selectValue.Equals("Yurdik shaxs"))
        {
            cbJismoniy.SelectedIndex = 0;
            Yurdik1.Visibility = Visibility.Visible;
            Jismoniy1.Visibility = Visibility.Collapsed;
        }
    }

    private void cbYurdikk_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string selectValue = ((ComboBoxItem)cbYurdikk.SelectedItem).Content.ToString();

        if (selectValue.Equals("Jismoniy shaxs"))
        {
            cbYurdikk.SelectedIndex = 1;
            Jismoniy1.Visibility = Visibility.Visible;
            Yurdik1.Visibility = Visibility.Collapsed;
        }
    }

    private void Button_GiveFeedback(object sender, GiveFeedbackEventArgs e)
    {

    }

    private void Muhr_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (Muhr.SelectedItem is ComboBoxItem selectedItem)
        {
            if (selectedItem.Content.ToString() == "Muhrli")
            {
                Muhr.Background = new SolidColorBrush(Colors.Green);
            }
            else
            {
                // Reset to default background color or set to another color if needed
                Muhr.Background = new SolidColorBrush(Colors.Red);
            }
        }
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        MessageBox.Show($"\tAssalomu alaykum hurmatli foydalanuvchi!\n" +
            $" Bu dastur xisob raqamdan xisob raqamga pul ko'chirishda foydalaniladi." +
            $" Aziz foydalanuvchi sizning vaqtingizni tejashga hamda ishlashda qulaylik imkonini yaratgan" +
            $" bo'lsam mamnunman. Dastur mutlaqo tekin, duoda onamni eslab qo'ysangiz hursand bo'lardim." +
            $" Dasturdan foydalanganingiz uchun tashakkur!\n" +
            $" hurmat bilan Avazbek.");
    }
}
