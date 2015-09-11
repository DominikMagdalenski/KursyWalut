using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace KursyWalutNBP
{
    /// <summary>
    /// Interaction logic for SortujWindow.xaml
    /// </summary>
    public partial class SortujWindow : Window
    {
        private readonly bool _arch;
        private readonly System.Windows.Controls.ListView _listView;

        public SortujWindow()
        {
            InitializeComponent();
        }

        // konstruktor z argumentem typu MainWindow
        public SortujWindow(MainWindow mainWindowArg)
        {
            InitializeComponent();
            // referencja do głównego okna aplikacji
            MainWindow mainWindow = mainWindowArg;
            _arch = Equals(mainWindow.WLista, mainWindow.listaWalutArch);
            _listView = _arch ? mainWindow.listaWalutArch : mainWindow.listaWalutAkt;
            wypelnijSortCB();
        }

        private void wypelnijSortCB()
        {
            if(_arch)
                sortujCB.Items.Add("Dzień");
            sortujCB.Items.Add("Waluta");
            if (_arch)
                sortujCB.Items.Add("Kraj");
            sortujCB.Items.Add("Przelicznik");
            sortujCB.Items.Add("Kod");
            sortujCB.Items.Add("średni");
            sortujCB.Items.Add("kupno");
            sortujCB.Items.Add("sprzedaż");
        }

        private void sortujButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Waluta> lista = _listView.Items.Cast<Waluta>().ToList();
                switch (sortujCB.SelectedItem.ToString())
                {
                    case "Dzień":
                        lista.Sort((x, y) => String.CompareOrdinal(x.Dzien, y.Dzien));
                        break;
                    case "Waluta":
                        lista.Sort((x, y) => String.CompareOrdinal(x.Nazwa, y.Nazwa));
                        break;
                    case "Kraj":
                        lista.Sort((x, y) => String.CompareOrdinal(x.Kraj, y.Kraj));
                        break;
                    case "Przelicznik":
                        lista.Sort((x, y) => String.CompareOrdinal(x.Przelicznik.ToString(), y.Przelicznik.ToString()));
                        break;
                    case "Kod":
                        lista.Sort((x, y) => String.CompareOrdinal(x.Kod, y.Kod));
                        break;
                    case "średni":
                        lista.Sort((x, y) => String.CompareOrdinal(x.KursSredni.ToString(), y.KursSredni.ToString()));
                        break;
                    case "kupno":
                        lista.Sort((x, y) => String.CompareOrdinal(x.KursKupna.ToString(), y.KursKupna.ToString()));
                        break;
                    case "sprzedaż":
                        lista.Sort((x, y) => String.CompareOrdinal(x.KursSprzedazy.ToString(), y.KursSprzedazy.ToString()));
                        break;
                    default:
                        lista.Sort((x, y) => String.CompareOrdinal(x.Nazwa, y.Nazwa));
                        break;
                }
                _listView.Items.Clear();
                foreach (Waluta waluta in lista)
                {
                    _listView.Items.Add(waluta);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Błąd");
            }
        }
    }
}
