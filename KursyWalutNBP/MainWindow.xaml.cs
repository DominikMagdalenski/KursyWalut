﻿/************************************************************************
 * MainWindow.xaml.cs -- główny plik programu łączący logikę, zdarzenia *
 * oraz grafikę całej aplikacji                                         *
 * Autor: Dominik Magdaleński                                           *
 * Data: 25/07/2015                                                     *
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using ListBox = System.Windows.Controls.ListBox;
using MessageBox = System.Windows.MessageBox;

namespace KursyWalutNBP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Tworzenie listy tabel kursów walut
        private readonly List<WalutyXml> _tabele = new List<WalutyXml>();
        private readonly List<WalutyXml> _tabeleArch = new List<WalutyXml>();
        private List<String> _dirArch = new List<string>();
        private readonly List<String> _listPlikowZMiesiaca = new List<string>();

        private BackgroundWorker _bkgWorker;
        private PobieranieWindow _dwnlWindow;

        // lista walut, nad którą aktualnie użytkownik pracuje
        private ListBox _wLista;
        public ListBox WLista { get { return _wLista; } }

        /// <summary>
        /// Konstruktor domyślny klasy MainWindow.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Inicjalizacja();
        }

        /// <summary>
        /// Metoda, którą powinna być wywoływana w konstruktorze MainWindow().
        /// Inicjalizuje ona pole _wLista, które przechowuje referencję
        /// do listy wybranych walut, z którą aktualnie pracuje użytkownik programu.
        /// </summary>
        private void Inicjalizacja()
        {
            // przechowywanie w _wLista referencji do listy z którą aktualnie pracujemy
            _wLista = listaWalutAkt;

            try
            {
                // wypełnienie combobox'a 'wyborRokuArch'
                // najstarsze archiwum sięga 2002 roku
                for (int i = 2002; i <= DateTime.Today.Year; i++)
                {
                    wyborRokuArch.Items.Add(i);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Błąd");
            }
        }

        /// <summary>
        /// Inicjuje pobieranie aktualnych kursów walut z oficjalnej strony NBP.
        /// Pobieranie jest uruchamiane asynchronicznie za pomocą klasy BackgroundWorker.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pobierzAktualneKursy_Click(object sender, RoutedEventArgs e)
        {
            // Rozpoczęcie pobierania aktualnych kursów w tle
            // Pomaga w tym klasa BackgroundWorker
            if (_bkgWorker == null)
            {
                _bkgWorker = new BackgroundWorker();
                // tworzenie nowego okienka, które uwidacznia progres pobierania
                _dwnlWindow = new PobieranieWindow();
                _bkgWorker.DoWork += _bkgWorker_DoWork;
                _bkgWorker.RunWorkerCompleted += _bkgWorker_RunWorkerCompleted;
                _bkgWorker.ProgressChanged += _bkgWorker_ProgressChanged;
                _bkgWorker.WorkerReportsProgress = true;
            }
            // deaktywowanie przycisku pobierania
            pobierzAktualneKursy.IsEnabled = false;
            pobierzAktualneKursy.Content = "Pobieranie...";
            // wyświetlenie okienka z progresem pobierania
            _dwnlWindow.Show();
            // rozpoczęcie pobierania za pomocą asynchronicznej metody RunWorkerAsync,
            // którą udostępnia klasa BackgroundWorker
            _bkgWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Pobiera aktualne kursy walut z oficjalnej strony NBP.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _bkgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Dodawanie tabel do listy kursów walut
                // 1. arg - adres URL
                // 2. arg - nazwa Tabeli
                // 3. arg - czy w dokumencie xml znajduje się kurs średni - true (czy kurs kupna/sprzedaży - false)?
                _tabele.Add(new WalutyXml("http://www.nbp.pl/kursy/xml/LastA.xml", "Tabela A", true));
                // raportowanie progresu (wywołuje zdarzenie ProgressChanged)
                _bkgWorker.ReportProgress(33);
                _tabele.Add(new WalutyXml("http://www.nbp.pl/kursy/xml/LastB.xml", "Tabela B", true));
                _bkgWorker.ReportProgress(66);
                _tabele.Add(new WalutyXml("http://www.nbp.pl/kursy/xml/LastC.xml", "Tabela C", false));
                _bkgWorker.ReportProgress(100);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }
        }

        /// <summary>
        /// Zdarzenie wywoływane, kiedy _bkgWorker zakończy prace,
        /// czyli wtedy kiedy zostaną pobrane dokumenty XML
        /// z aktualnymi kursami walut.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _bkgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // zamyka okienko z progresem pobierania
            _dwnlWindow.Close();
            pobierzAktualneKursy.Content = "Pobieranie zakończone";
            try
            {
                // Dodawanie tabel do comboBox'a 'wyborTabeli' z listy _tabele
                foreach (WalutyXml tabela in _tabele)
                {
                    wyborTabeli.Items.Add(tabela.Nazwa);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }
        }

        /// <summary>
        /// Zdarzenie raportuje o postępach pobierania aktualnych kursów walut.
        /// Zmienia wartość _dwnlWindow.progressBar w zależności od postępów pobierania.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _bkgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // aktualizuje progressBar w oknie _dwnlWindow
            _dwnlWindow.progresBarPobieranie.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// Zdarzenie wywoływane, kiedy użytkownik wybierze inną walutę w ComboBox'ie.
        /// Dodaje wybraną walutę do listy aktualnych, wybranych walut.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wyborWaluty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // index wybranej waluty
            int index = wyborWaluty.SelectedIndex;
            try
            {
                if (index > -1)
                    listaWalutAkt.Items.Add(_tabele[wyborTabeli.SelectedIndex].Lista[index]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }
        }

        /// <summary>
        /// Zdarzenie wywoływane, kiedy użytkownik wybierze inną tabelę w ComboBox'ie.
        /// Czyści ComboBox z wyborem waluty, a następnie go wypełnia odpowiednimi walutami.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Zdarzenie wywoływane, gdy zostanie naciśniety jakiś przycisk.
        /// Obsłużony jest tylko przycisk ESCAPE, który kończy działanie programu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Wyjście z programu za pomocą klawisza ESCAPE
            if (e.Key == Key.Escape)
                Close();
        }

        /// <summary>
        /// Zdarzenie wywoływane po wybraniu opcji "Tryb->Kursy aktualne".
        /// Przełącza na tryb "Kursy aktualne".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Aktualne_Click(object sender, RoutedEventArgs e)
        {
            _wLista = listaWalutAkt;
            AktualneGrid.Visibility = Visibility.Visible;
            ArchiwumGrid.Visibility = Visibility.Hidden;
            ZapiszGrid.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Zdarzenie wywoływane po wybraniu opcji "Tryb->Archiwum".
        /// Przełącza na tryb "Archiwum".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Archiwum_Click(object sender, RoutedEventArgs e)
        {
            _wLista = listaWalutArch;
            AktualneGrid.Visibility = Visibility.Hidden;
            ArchiwumGrid.Visibility = Visibility.Visible;
            ZapiszGrid.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Zdarzenie wywoływane po wybraniu opcji "Tryb->Zapisz...".
        /// Przełącza na tryb "Zapisz...".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Zapisz_Click(object sender, RoutedEventArgs e)
        {
            AktualneGrid.Visibility = Visibility.Hidden;
            ArchiwumGrid.Visibility = Visibility.Hidden;
            ZapiszGrid.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Zdarzenie wywoływane po wybraniu opcji "Edycja->Sortuj...".
        /// Uruchamia okienko, za pomocą którego można posortować
        /// listę wybranych walut.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sortuj_Click(object sender, RoutedEventArgs e)
        {
            SortujWindow sortujWindow = new SortujWindow(this);
            if (Equals(_wLista, listaWalutAkt))
                sortujWindow.Title += " - aktualne";
            else
                sortujWindow.Title += " - archiwum";
            sortujWindow.Show();
        }

        /// <summary>
        /// Zdarzenie wywoływane po wybraniu opcji "Edycja->Wyświetl tabelę->Tabela A".
        /// Otwiera okienko z aktualnymi kursami walut dot. Tabeli A.
        /// Jeśli tabela nie została wcześniej pobrana, to zdarzenie
        /// automatycznie pobierze tabelę ze strony NBP.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WyswietlA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // indeksy: 0 - tabelaA, 1 - tabelaB, 2 - tabelaC
                // jeśli nie pobrano wcześniej tabeli, to pobierz ją teraz
                WalutyXml tab = _tabele.Count != 3 ? new WalutyXml("http://www.nbp.pl/kursy/xml/LastA.xml", "Tabela A", true) : _tabele[0];
                string dzien = DateTime.Today.Day + "." + DateTime.Today.Month + "." + DateTime.Today.Year;
                // tworzenie nowego okna dla tabeli
                TabelaAbcWindow tabelaA = new TabelaAbcWindow(tab) {Title = "Tabela A " + dzien};
                // wyświetlenie okna
                tabelaA.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Błąd");
            }
        }

        /// <summary>
        /// Zdarzenie wywoływane po wybraniu opcji "Edycja->Wyświetl tabelę->Tabela B".
        /// Otwiera okienko z aktualnymi kursami walut dot. Tabeli B.
        /// Jeśli tabela nie została wcześniej pobrana, to zdarzenie
        /// automatycznie pobierze tabelę ze strony NBP.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WyswietlB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // indeksy: 0 - tabelaA, 1 - tabelaB, 2 - tabelaC
                // jeśli nie pobrano wcześniej tabeli, to pobierz ją teraz
                WalutyXml tab = _tabele.Count != 3 ? new WalutyXml("http://www.nbp.pl/kursy/xml/LastB.xml", "Tabela B", true) : _tabele[1];
                string dzien = DateTime.Today.Day + "." + DateTime.Today.Month + "." + DateTime.Today.Year;
                // tworzenie nowego okna dla tabeli
                TabelaAbcWindow tabelaB = new TabelaAbcWindow(tab) { Title = "Tabela B " + dzien };
                // wyświetlenie okna
                tabelaB.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Błąd");
            }
        }

        /// <summary>
        /// Zdarzenie wywoływane po wybraniu opcji "Edycja->Wyświetl tabelę->Tabela C".
        /// Otwiera okienko z aktualnymi kursami walut dot. Tabeli C.
        /// Jeśli tabela nie została wcześniej pobrana, to zdarzenie
        /// automatycznie pobierze tabelę ze strony NBP.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WyswietlC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // indeksy: 0 - tabelaA, 1 - tabelaB, 2 - tabelaC
                // jeśli nie pobrano wcześniej tabeli, to pobierz ją teraz
                WalutyXml tab = _tabele.Count != 3 ? new WalutyXml("http://www.nbp.pl/kursy/xml/LastB.xml", "Tabela C", true) : _tabele[2];
                string dzien = DateTime.Today.Day + "." + DateTime.Today.Month + "." + DateTime.Today.Year;
                // tworzenie nowego okna dla tabeli
                TabelaAbcWindow tabelaC = new TabelaAbcWindow(tab) { Title = "Tabela C " + dzien };
                // wyświetlenie okna
                tabelaC.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Błąd");
            }
        }

        /// <summary>
        /// Zdarzenie wywoływane po wybraniu opcji "Edycja->Wyczyść listę".
        /// Czyści listę wybranych walut. W zależności od tego jaki tryb
        /// został ostatnio wybrany, taka lista zostanie wyczyszczona.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Wyczysc_Click(object sender, RoutedEventArgs e)
        {
            // Jeśli użytkownik pracuje w trybie aktualnych kursów,
            // i wybierze opcje "Wyczyść listę", to zdarzenie wyczyści
            // listę wybranych, aktualnych kursów walut.
            // A jeżeli użytkownik pracuje w trybie archiwalnych kursów,
            // to po wybraniu opcji "Wyczyść listę", zdarzenie wyczyści
            // listę wybranych, archiwalnych kursów walut.
            if (Equals(_wLista, listaWalutAkt))
                listaWalutAkt.Items.Clear();
            else if(Equals(_wLista, listaWalutArch))
                listaWalutArch.Items.Clear();
        }

        /*private void WykresLista_Click(object sender, RoutedEventArgs e)
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

        }*/

        /*private void Pomoc_Click(object sender, RoutedEventArgs e)
        {

        }*/

        /// <summary>
        /// Zdarzenie wywoływane po wybraniu opcji "Pomoc->O programie".
        /// Wyświetla małe okno z krótkim opisem programu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OProgramie_Click(object sender, RoutedEventArgs e)
        {
            OProgramieWindow oProgramieWindow = new OProgramieWindow();
            oProgramieWindow.Show();
        }

        /// <summary>
        /// Zdarzenie wywoływane po wybraniu opcji "Pomoc->Strona NBP".
        /// Otwiera w domyślnej przeglądarce oficjalną stronę NBP.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StronaNBP_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.nbp.pl/");
        }

        /// <summary>
        /// Zdarzenie jest wywoływane po wybraniu opcji "Pomoc->Autorzy".
        /// Wyświetla małe okno z informacją o autorach projektu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Autorzy_Click(object sender, RoutedEventArgs e)
        {
            AutorzyWindow autorzyWindow = new AutorzyWindow();
            autorzyWindow.Show();
        }

        /// <summary>
        /// Zdarzenie wywoływane po wybraniu roku w ComboBox, w trybie "Archiwum".
        /// Pobiera odpowiedni plik "dir[rok].txt" ze strony NBP
        /// oraz wypełnia ComboBox z wyborem miesiąca.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            // ustalanie nazwy pliku dir w zależności od wybranego roku
            string dirTxt;
            if (wyborRokuArch.SelectedItem.ToString() == DateTime.Today.Year.ToString())
                dirTxt = "dir.txt";
            else
                dirTxt = "dir" + wyborRokuArch.SelectedItem + ".txt";

            try
            {
                wyborDniaArch.Items.Clear();
                // pobieranie pliku dir z nazwami plików .xml zawierającymi kursy walut
                WebClient wc = new WebClient();
                string content = wc.DownloadString("http://www.nbp.pl/kursy/xml/" + dirTxt);
                
                // kopiowanie zawartości pliku dir do listy
                _dirArch = content.Split('\n').ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }
        }

        /// <summary>
        /// Zdarzenie wywoływane po wybraniu miesiąca w ComboBox, w trybie "Archiwum"
        /// Wypełnia ComboBox do wyboru dnia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wyborMiesiacaArch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _listPlikowZMiesiaca.Clear();

            try
            {
                wyborDniaArch.Items.Clear();
                int miesiac = wyborMiesiacaArch.SelectedIndex + 1;
                int indexItemFirst = 0, indexItemLast = 0;

                // szukanie pierwszej i ostatniej nazwy pliku z wybranego misiąca
                // z niektorych plikow dir wyciagany jest pusty wiersz i dodawany na koniec listy
                // aby uniknąć błędów, dodano sprawdzenie _dirArch[i] != ""
                for (int i = 0; i < _dirArch.Count && _dirArch[i] != ""; i++)
                {
                    if (Convert.ToInt32(_dirArch[i][7].ToString() + _dirArch[i][8]) < miesiac)
                        indexItemLast = ++indexItemFirst;
                    else if (Convert.ToInt32(_dirArch[i][7].ToString() + _dirArch[i][8]) == miesiac)
                        ++indexItemLast;
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

        /// <summary>
        /// Zdarzenie wywoływane po wybraniu dnia w ComboBox, w trybie "Archiwum"
        /// Pobiera tabele z kursami walut, zależnie od wybranego dnia.
        /// Wypełnia ComboBox z wyborem tabeli.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        _tabeleArch.Add(new WalutyXml("http://www.nbp.pl/kursy/xml/" + t + ".xml", "Tabela A", true));
                        else if (t[0] == 'b')
                        _tabeleArch.Add(new WalutyXml("http://www.nbp.pl/kursy/xml/" + t + ".xml", "Tabela B", true));
                        else if (t[0] == 'c')
                        _tabeleArch.Add(new WalutyXml("http://www.nbp.pl/kursy/xml/" + t + ".xml", "Tabela C", false));
                        }
                    else if (Convert.ToInt32(wyborDniaArch.SelectedItem) < dzien)
                        break;
                }

                _tabeleArch.Sort((x,y) => String.CompareOrdinal(x.Nazwa, y.Nazwa));

                // Wypełnianie combobox'a 'wyborTabeliArch'
                foreach (WalutyXml tabela in _tabeleArch)
                {
                    wyborTabeliArch.Items.Add(tabela.Nazwa);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }
            
        }

        /// <summary>
        /// Zdarzenie wywoływane po wybraniu tabeli w ComboBox, w trybie "Archiwum".
        /// Wypełnia ComboBox do wyboru waluty, zależnie od wybranej tabeli.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wyborTabeliArch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                wyborWalutyArch.Items.Clear();

                // wypełnianie combobox'a 'wyborWalutyArch'
                if(wyborTabeliArch.SelectedIndex > -1)
                    foreach (Waluta waluta in _tabeleArch[wyborTabeliArch.SelectedIndex].Lista)
                    {
                        if (waluta.Nazwa != null && waluta.Kraj != null)
                            wyborWalutyArch.Items.Add(waluta.Kraj + " " + waluta.Nazwa);
                        else if (waluta.Nazwa != null)
                            wyborWalutyArch.Items.Add(waluta.Nazwa);
                        else
                            wyborWalutyArch.Items.Add(waluta.Kraj);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }
        }

        /// <summary>
        /// Zdarzenie wywoływane po wybraniu waluty w ComboBox, w trybie "Archiwum".
        /// Dodaje do listy wybranych walut archiwalnych wybraną walutę.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wyborWalutyArch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ustawianie stringu 'miesiac' w zależności od wybranego miesiąca
            // 01, 02,..., 09, 10, 11, 12
            string miesiac = (wyborMiesiacaArch.SelectedIndex + 1) > 9
                ? (wyborMiesiacaArch.SelectedIndex+1).ToString()
                : "0" + (wyborMiesiacaArch.SelectedIndex+1);

            int i = wyborWalutyArch.SelectedIndex;
            
            try
            {
                // ustalanie stringu dzień
                // 01, 02,..., 09, 10, 11,..., 31
                string dzien = Convert.ToInt32(wyborDniaArch.SelectedItem) > 9
                    ? wyborDniaArch.SelectedItem.ToString()
                    : "0" + wyborDniaArch.SelectedItem;

                // dodawanie wybranej waluty do listy wybranych walut (archiwalnych)
                if (i > -1)
                {
                    _tabeleArch[wyborTabeliArch.SelectedIndex].Lista[i].Dzien =
                        wyborRokuArch.SelectedItem + "." + miesiac +
                        "." + dzien;
                    listaWalutArch.Items.Add(
                        _tabeleArch[wyborTabeliArch.SelectedIndex].Lista[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }
        }

        /// <summary>
        /// Zdarzenie wywoływane po naciśnięciu przycisku z napisem "Wybierz folder..." w trybie "Zapisz".
        /// Otwiera okno, które pozwala wybrać folder, w którym to użytkownik
        /// może zapisać swoje listy wybranych walut.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zapiszWybierzFolder_Click(object sender, RoutedEventArgs e)
        {
            // tworzenie okienka, w którym użytkownik może wybrać folder,
            // w celu ustalenia ścieżki do zapisu list wybranych walut
            FolderBrowserDialog folderDialog = new FolderBrowserDialog { SelectedPath = "C:\\KursyWalut\\" };
            DialogResult result = folderDialog.ShowDialog();
            if (result.ToString() == "OK")
                zapiszSciezka.Text = folderDialog.SelectedPath;
        }

        /// <summary>
        /// Zdarzenie wywoływane po naciśnięciu przycisku z napisem "ZAPISZ",
        /// w trybie "Zapisz...".
        /// Zapisuje wybrane listy do plików w wybranym folderze,
        /// w wybranym przez użytkownika formacie oraz nazwie pliku.
        /// Jeśli jest to lista - archiwum, to do nazwy pliku
        /// dopisywany jest ciąg "Arch".
        /// Dostępne formaty zapisu, to .csv oraz .xml.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zapiszButton_Click(object sender, RoutedEventArgs e)
        {
            // jeśli nie wybrano żadnej z list do zapisu,
            // to wyświetl MessageBox z informacją o błędzie,
            // oraz zakończ zdarzenie
            if (zapiszListaAktualneCB.IsChecked == false && zapiszListaArchCB.IsChecked == false)
            {
                MessageBox.Show("Wybierz jakąś listę do zapisania.", "Błąd");
                return;
            }
            // jeśli nie wybrano żadnego formatu zapisu,
            // to wyświetl MessageBox z informacją
            // i zakończ zdarzenie
            if (zapiszFormatCsv.IsChecked == false && zapiszFormatXml.IsChecked == false)
            {
                MessageBox.Show("Wybierz jakiś format pliku.", "Błąd");
                return;
            }
            try
            {
                // modyfikacja ścieżki zapisu polegająca na podwojeniu
                // występujących w niej backslashy '\'
                // jest to potrzebne, ponieważ '\' jest znakiem specjalnym
                // (escape characters)
                string sciezka = zapiszSciezka.Text.Replace(@"\", @"\\");
                // utwórz folder jeśli nie istnieje
                if (!Directory.Exists(sciezka))
                    Directory.CreateDirectory(sciezka);
                // jeśli wybrano liste aktulnych kursów do zapisu, to:
                if (zapiszListaAktualneCB.IsChecked == true)
                {
                    // jeśli wybrany format to .csv, to:
                    if (zapiszFormatCsv.IsChecked == true)
                    {
                        // zapisz ścieżkę do pliku w obiekcie file
                        string file = sciezka + "\\" + zapiszNazwaPliku.Text + ".csv";
                        // otwórz plik, a jeśli nie istnieje, to go utwórz
                        // z prawami do zapisu
                        FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write);
                        // otwórz strumień do zapisu, do pliku
                        StreamWriter sw = new StreamWriter(fs);
                        // poniższe dwie instrukcje czyszczą zawartość pliku
                        fs.SetLength(0);
                        fs.Flush();
                        sw.WriteLine("Waluta,Przelicznik,Kod,Kurs średni,Kurs kupna, Kurs sprzedaży");
                        // poniższa pętla zapisuje dane z listy wybranych,
                        // aktualnych kursów walut do pliku .csv
                        foreach (Waluta waluta in listaWalutAkt.Items)
                        {
                            sw.WriteLine(waluta.Nazwa + ","
                                        + waluta.Przelicznik + "," + waluta.Kod
                                        + "," + waluta.KursSredni + ","
                                        + waluta.KursKupna + "," + waluta.KursSprzedazy);
                        }
                        // zamykanie strumienia oraz pliku
                        sw.Close();
                        fs.Close();
                    }
                    // jeśli wybrany format to Dokument XML .xml, to:
                    if (zapiszFormatXml.IsChecked == true)
                    {
                        // zapisz ścieżkę do pliku w obiekcie file
                        string file = sciezka + "\\" + zapiszNazwaPliku.Text + ".xml";
                        // tworzenie pliku .xml
                        using (XmlWriter writer = XmlWriter.Create(file))
                        {
                            // przechowanie ustawień dot. formatu danych,
                            // jakie istnieją w Polsce, w obiekcie polska
                            CultureInfo polska = new CultureInfo("pl-PL");
                            // poniższe instrukcje zapisują dane do pliku .xml
                            writer.WriteStartDocument();
                            writer.WriteStartElement("waluty_lista_aktualna");
                            writer.WriteElementString("data_zapisu", DateTime.Now.ToString(polska));
                            foreach (Waluta waluta in listaWalutAkt.Items)
                            {
                                writer.WriteStartElement("pozycja");
                                writer.WriteElementString("nazwa_waluty", waluta.Nazwa);
                                writer.WriteElementString("przelicznik", waluta.Przelicznik.ToString());
                                writer.WriteElementString("kod_waluty", waluta.Kod);
                                writer.WriteElementString("kurs_sredni", waluta.KursSredni.ToString(polska));
                                writer.WriteElementString("kurs_kupna", waluta.KursKupna.ToString(polska));
                                writer.WriteElementString("kurs_sprzedazy", waluta.KursSprzedazy.ToString(polska));
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                            writer.WriteEndDocument();
                        }
                    }
                }
                // jeśli wybrano liste archiwalnych kursów do zapisu, to:
                if (zapiszListaArchCB.IsChecked == true)
                {
                    // jeśli wybrany format to .csv, to:
                    if (zapiszFormatCsv.IsChecked == true)
                    {
                        // zapisz ścieżkę do pliku w obiekcie file
                        string file = sciezka + "\\" + zapiszNazwaPliku.Text + "Arch.csv";
                        // otwórz plik, a jeśli nie istnieje, to go utwórz
                        // z prawami do zapisu
                        FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write);
                        // otwórz strumień do zapisu do pliku
                        StreamWriter sw = new StreamWriter(fs);
                        // poniższe dwie instrukcje czyszczą plik
                        fs.SetLength(0);
                        fs.Flush();
                        sw.WriteLine("Dzień,Waluta,Kraj,Przelicznik,Kod,Kurs średni,Kurs kupna,Kurs sprzedaży");
                        // poniższa pętla zapisuje dane z listy wybranych,
                        // archiwalnych kursów walut do pliku .csv
                        foreach (Waluta waluta in listaWalutArch.Items)
                        {
                            sw.WriteLine(waluta.Dzien + ","
                                        + waluta.Nazwa + "," + waluta.Kraj + ","
                                        + waluta.Przelicznik + "," + waluta.Kod
                                        + "," + waluta.KursSredni + ","
                                        + waluta.KursKupna + "," + waluta.KursSprzedazy);
                        }
                        // zamykanie strumienia oraz pliku
                        sw.Close();
                        fs.Close();
                    }
                    // jeśli wybrany format to Dokument XML .xml, to:
                    if (zapiszFormatXml.IsChecked == true)
                    {
                        // zapisz ścieżkę do pliku w obiekcie file
                        string file = sciezka + "\\" + zapiszNazwaPliku.Text + "Arch.xml";
                        // tworzenie pliku .xml
                        using (XmlWriter writer = XmlWriter.Create(file))
                        {
                            // przechowanie ustawień dot. formatu danych,
                            // jakie istnieją w Polsce, w obiekcie polska
                            CultureInfo polska = new CultureInfo("pl-PL");
                            // poniższe instrukcje zapisują dane do pliku .xml
                            writer.WriteStartDocument();
                            writer.WriteStartElement("waluty_lista_archiwalna");
                            writer.WriteElementString("data_zapisu", DateTime.Now.ToString(polska));
                            foreach (Waluta waluta in listaWalutArch.Items)
                            {
                                writer.WriteStartElement("pozycja");
                                writer.WriteElementString("dzien", waluta.Dzien);
                                writer.WriteElementString("nazwa_waluty", waluta.Nazwa);
                                writer.WriteElementString("kraj", waluta.Kraj);
                                writer.WriteElementString("przelicznik", waluta.Przelicznik.ToString());
                                writer.WriteElementString("kod_waluty", waluta.Kod);
                                writer.WriteElementString("kurs_sredni", waluta.KursSredni.ToString(polska));
                                writer.WriteElementString("kurs_kupna", waluta.KursKupna.ToString(polska));
                                writer.WriteElementString("kurs_sprzedazy", waluta.KursSprzedazy.ToString(polska));
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                            writer.WriteEndDocument();
                        }
                    }
                }
                MessageBox.Show("Operacja zapisu przebiegła pomyślnie.", "Zapisz");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }
        }
    }
}
