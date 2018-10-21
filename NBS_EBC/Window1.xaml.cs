using NBS_EBC.Waluty;
using System;
using System.Windows;
using System.Windows.Controls;

namespace NBS_EBC
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        private KoronaCzeska kcWaluta;
        private SlowacjaEuro seWaluta;
        private ZlotowkiBankCzeski plbcWaluta;

        public Window1()
        {
            InitializeComponent();

            this.kcWaluta = new KoronaCzeska();
            this.seWaluta = new SlowacjaEuro();
            this.plbcWaluta = new ZlotowkiBankCzeski();

            this.cKalendarz.DisplayDateEnd = DateTime.Now;
        }

        private void cKalendarz_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var calendar = sender as System.Windows.Controls.Calendar;

            if (calendar.SelectedDate.HasValue)
            {
                DateTime date = calendar.SelectedDate.Value;

                if (this.seWaluta.notowania.ContainsKey(date))
                {
                    this.pKursSE.Visibility = Visibility.Visible;

                    this.lKursSE.Text = this.seWaluta.notowania[date].ToString("0.0000") + "  zł ";
                    this.lDzienSE.Text = date.ToShortDateString();
                }
                else this.pKursSE.Visibility = Visibility.Hidden;

                if (this.kcWaluta.notowania.ContainsKey(date))
                {
                    this.pKursKC.Visibility = Visibility.Visible;

                    this.lKursKC.Text = this.kcWaluta.notowania[date].ToString("0.0000") + "  zł ";
                    this.lDzienKC.Text = date.ToShortDateString();
                }
                else this.pKursKC.Visibility = Visibility.Hidden;

                if (this.plbcWaluta.notowania.ContainsKey(date))
                {
                    this.pKursPLBC.Visibility = Visibility.Visible;

                    this.lKursPLBC.Text = this.plbcWaluta.notowania[date].ToString("0.0000") + " czk";
                    this.lDzienPLBC.Text = date.ToShortDateString();
                }
                else this.pKursPLBC.Visibility = Visibility.Hidden;

            }
        }
    }
}