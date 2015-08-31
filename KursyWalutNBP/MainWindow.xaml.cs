/************************************************************************
 * MainWindow.xaml.cs -- główny plik programu łączący logikę, zdarzenia *
 * oraz grafikę całej aplikacji                                         *
 * Autor: Dominik Magdaleński                                           *
 * Data: 25/07/2015                                                     *
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KursyWalutNBP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Tworzenie listy tabel kursów walut
        private readonly List<WalutyXML> _tabele = new List<WalutyXML>();
        private readonly List<WalutyXML> _tabeleArch = new List<WalutyXML>();
        //private readonly List<Waluta> _walutyArch = new List<Waluta>(); 
        private List<String> _dirArch = new List<string>();
        private readonly List<String> _listPlikowZMiesiaca = new List<string>(); 

        // lista walut, nad którą aktualnie użytkownik pracuje
        private ListBox _wLista;

        public MainWindow()
        {
            InitializeComponent();
            Inicjalizacja();
        }

        private void Inicjalizacja()
        {
            // przechowywanie w _wLista referencji do listy z którą aktualnie pracujemy
            _wLista = listBox;

            // Nieudane tworzenie nazwy dokumentu
            // Moze się przydać do pobierania danych archiwalnych
            // http://www.nbp.pl/home.aspx?f=/kursy/instrukcja_pobierania_kursow_walut.html
            //DateTime today = DateTime.Today;
            //string year = today.Year.ToString()[2].ToString() + today.Year.ToString()[3].ToString();
            //string filename = "http://www.nbp.pl/kursy/xml/a" + today.DayOfYear + "z" + year + today.Month + today.Day + ".xml";
            //listBox.Items.Add(filename);

            try
            {
                // Dodawanie tabel do listy kursów walut
                // 1. arg - adres URL
                // 2. arg - nazwa Tabeli
                // 3. arg - czy w dokumencie xml znajduje się kurs średni - true (czy kurs kupna/sprzedaży - false)?
                _tabele.Add(new WalutyXML("http://www.nbp.pl/kursy/xml/LastA.xml", "Tabela A", true));
                _tabele.Add(new WalutyXML("http://www.nbp.pl/kursy/xml/LastB.xml", "Tabela B", true));
                _tabele.Add(new WalutyXML("http://www.nbp.pl/kursy/xml/LastC.xml", "Tabela C", false));

                // Dodawanie tabel do comboBox'a 'wyborTabeli' z listy _tabele
                foreach (WalutyXML tabela in _tabele)
                {
                    wyborTabeli.Items.Add(tabela.Nazwa);
                }

                // wypełnienie combobox'a 'wyborRokuArch'
                // najstarsze archiwum sięga 2002 roku
                for (int i = 2002; i <= DateTime.Today.Year; i++)
                {
                    wyborRokuArch.Items.Add(i);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Błąd");
            }
        }

        private void wyborWaluty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // index wybranej waluty
            int index = wyborWaluty.SelectedIndex;
            try
            {
                if (index > -1)
                    listBox.Items.Add(_tabele[wyborTabeli.SelectedIndex].ToString(index));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }
        }

        private void wyborTabeli_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Czyszczenie comboBox'a 'wyborWaluty' przed wypełnieniem nowymi danymi
                wyborWaluty.Items.Clear();

                // Wypełnianie walutami comboBox'a 'wyborWaluty'
                // wyborTabeli.SelectedIndex - index wybranej tabeli
                // _tabele[wyborTabeli.SelectedIndex].Lista - lista walut wybranej tabeli
                foreach (Waluta waluta in _tabele[wyborTabeli.SelectedIndex].Lista)
                {
                    wyborWaluty.Items.Add(waluta.Nazwa);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }
            
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Wyjście z programu za pomocą klawisza ESCAPE
            if (e.Key == Key.Escape)
                Close();
        }

        private void Aktualne_Click(object sender, RoutedEventArgs e)
        {
            _wLista = listBox;
            AktualneGrid.Visibility = Visibility.Visible;
            ArchiwumGrid.Visibility = Visibility.Hidden;
            ZapiszGrid.Visibility = Visibility.Hidden;
        }

        private void Archiwum_Click(object sender, RoutedEventArgs e)
        {
            _wLista = listaWalutArch;
            AktualneGrid.Visibility = Visibility.Hidden;
            ArchiwumGrid.Visibility = Visibility.Visible;
            ZapiszGrid.Visibility = Visibility.Hidden;
        }

        private void Zapisz_Click(object sender, RoutedEventArgs e)
        {
            AktualneGrid.Visibility = Visibility.Hidden;
            ArchiwumGrid.Visibility = Visibility.Hidden;
            ZapiszGrid.Visibility = Visibility.Visible;
        }

        private void Sortuj_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WyswietlA_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WyswietlB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WyswietlC_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Wyczysc_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WykresLista_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WykresA_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WykresB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WykresC_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WykresZmian_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OProgramie_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StronaNBP_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Autorzy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void wyborRokuArch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                wyborMiesiacaArch.Items.Clear();

                // wypełnianie combobox'a 'wyborMiesiacaArch'
                for (int i = 0; i < 12; i++)
                {
                    wyborMiesiacaArch.Items.Add(CultureInfo.GetCultureInfo("pl-PL").DateTimeFormat.GetMonthName(i + 1));
                }
                
                // usuwanie nieodpowiednich miesięcy z combobox'a 'wyborMiesiacaArch'
                // zabieg potrzebny dla ustawienia bieżącego roku
                if (wyborRokuArch.SelectedItem.ToString() == DateTime.Today.Year.ToString())
                    for (int i = 11; i >= DateTime.Today.Month; i--)
                        wyborMiesiacaArch.Items.RemoveAt(i);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }

            // pobieranie pliku dir z nazwami plików .xml zawierającymi kursy walut
            string dirTxt;
            if (wyborRokuArch.SelectedItem.ToString() == DateTime.Today.Year.ToString())
                dirTxt = "dir.txt";
            else
                dirTxt = "dir" + wyborRokuArch.SelectedItem + ".txt";

            try
            {
                wyborDniaArch.Items.Clear();
                System.Net.WebClient wc = new System.Net.WebClient();
                string content = wc.DownloadString("http://www.nbp.pl/kursy/xml/" + dirTxt);
                
                // kopiowanie zawartości pliku dir do listy
                _dirArch = content.Split('\n').ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }
        }

        private void wyborMiesiacaArch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _listPlikowZMiesiaca.Clear();

            try
            {
                wyborDniaArch.Items.Clear();
                int miesiac = wyborMiesiacaArch.SelectedIndex + 1;
                int indexItemFirst = 0, indexItemLast = 0;

                // szukanie pierwszej i ostatniej nazwy pliku z wybranego misiąca
                // z niktorych plikow dir wyciagany jest pusty wiersz i dodawany na koniec listy
                // aby uniknąć błędów, dodano sprawdzenie _dirArch[i] != ""
                for (int i = 0; i < _dirArch.Count && _dirArch[i] != ""; i++)
                {
                    if (Convert.ToInt32(_dirArch[i][7].ToString() + _dirArch[i][8]) < miesiac)
                        indexItemLast = ++indexItemFirst;
                    else if (Convert.ToInt32(_dirArch[i][7].ToString() + _dirArch[i][8]) == miesiac)
                    {
                        //listaWalutArch.Items.Add(_dirArch[i].TrimEnd());
                        ++indexItemLast;
                    }
                }

                // wypełnianie combobox'a 'wyborDniaArch'
                int dzien = Convert.ToInt32(_dirArch[indexItemFirst][9].ToString() + _dirArch[indexItemFirst++][10]);
                wyborDniaArch.Items.Add(dzien);
                while (indexItemLast > indexItemFirst)
                {
                    _listPlikowZMiesiaca.Add(_dirArch[indexItemFirst].TrimEnd());
                    int temp = Convert.ToInt32(_dirArch[indexItemFirst][9].ToString() + _dirArch[indexItemFirst++][10]);
                    if (temp != dzien)
                    {
                        dzien = temp;
                        wyborDniaArch.Items.Add(dzien);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }
        }

        private void wyborDniaArch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _tabeleArch.Clear();
            try
            {
                wyborTabeliArch.Items.Clear();

                // Pobieranie dostępnych tabel z wybranego dnia
                foreach (string t in _listPlikowZMiesiaca)
                {
                    int dzien = Convert.ToInt32(t[9].ToString() + t[10]);
                    if (Convert.ToInt32(wyborDniaArch.SelectedItem) == dzien)
                    {
                        if (t[0] == 'a')
                            _tabeleArch.Add(new WalutyXML("http://www.nbp.pl/kursy/xml/" + t + ".xml", "Tabela A", true));
                        else if (t[0] == 'b')
                            _tabeleArch.Add(new WalutyXML("http://www.nbp.pl/kursy/xml/" + t + ".xml", "Tabela B", true));
                        else if (t[0] == 'c')
                            _tabeleArch.Add(new WalutyXML("http://www.nbp.pl/kursy/xml/" + t + ".xml", "Tabela C", false));
                    }
                    else if (Convert.ToInt32(wyborDniaArch.SelectedItem) < dzien)
                        break;
                }

                _tabeleArch.Sort((x,y) => String.CompareOrdinal(x.Nazwa, y.Nazwa));

                // Wypełnianie combobox'a 'wyborTabeliArch'
                foreach (WalutyXML tabela in _tabeleArch)
                {
                    wyborTabeliArch.Items.Add(tabela.Nazwa);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }
            
        }

        private void wyborTabeliArch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                wyborWalutyArch.Items.Clear();

                // wypełnianie combobox'a 'wyborWalutyArch'
                if(wyborTabeliArch.SelectedIndex > -1)
                    foreach (Waluta waluta in _tabeleArch[wyborTabeliArch.SelectedIndex].Lista)
                    {
                        if (waluta.Nazwa != null && waluta.NazwaKraju != null)
                            wyborWalutyArch.Items.Add(waluta.NazwaKraju + " " + waluta.Nazwa);
                        else if (waluta.Nazwa != null)
                            wyborWalutyArch.Items.Add(waluta.Nazwa);
                        else
                            wyborWalutyArch.Items.Add(waluta.NazwaKraju);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }
        }

        private void wyborWalutyArch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (wyborWalutyArch.SelectedIndex > -1)
                    listaWalutArch.Items.Add(
                        _tabeleArch[wyborTabeliArch.SelectedIndex].ToString(wyborWalutyArch.SelectedIndex));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Błąd");
            }
        }
    }
}
