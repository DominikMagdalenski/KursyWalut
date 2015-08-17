/************************************************************************
 * MainWindow.xaml.cs -- główny plik programu łączący logikę, zdarzenia *
 * oraz grafikę całej aplikacji                                         *
 * Autor: Dominik Magdaleński                                           *
 * Data: 25/07/2015                                                     *
 ***********************************************************************/

using System;
using System.Collections.Generic;
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
        private ListBox _wLista;

        public MainWindow()
        {
            InitializeComponent();
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
            if (index > -1)
                listBox.Items.Add(_tabele[wyborTabeli.SelectedIndex].ToString(index));
        }

        private void wyborTabeli_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
    }
}
