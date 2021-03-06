using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace KursyWalutNBP
{
    /// <summary>
    /// Klasa przedstawiająca zbiór walut.
    /// Za jej pomocą można wyciągnąć z pliku XML dane o kursach walut.
    /// </summary>
    public class WalutyXml
    {
        public string Nazwa { get; set; }           // nazwa tabeli
        public Boolean KursSredni { get; set; }     // czy uwzgledniany jest tylko kurs średni?
        private readonly int _count;                // liczba walut w tabeli
        public int Count { get { return _count;} }  // liczba walut w tabeli

        private readonly List<Waluta> _lista;                   // lista walut
        public List<Waluta> Lista { get { return _lista; } }    // lista walut

        /// <summary>
        /// Konstruktor z trzema parametrami, odpowiedzialny za pobranie
        /// i przetworzenie dokumentu XML z kursami walut.
        /// </summary>
        /// <param name="filename">Nazwa pliku (URL).</param>
        /// <param name="nazwa">Nazwa zbioru walut lub tabeli, np. "Tabela A".</param>
        /// <param name="kursSredni">Czy w tym zbiorze/tabeli mają być uwzględnione kursy średni?</param>
        public WalutyXml(string filename, string nazwa, Boolean kursSredni)
        {
            Nazwa = nazwa;
            KursSredni = kursSredni;

            var doc = new XmlDocument();

            _lista = new List<Waluta>();

            // pobieranie dokumentu z adresu URL - filename
            doc.Load(filename);

            // obliczanie liczby walut
            _count = doc.GetElementsByTagName("pozycja").Count;

            // Petla dodaje waluty do listy zapisując w odpowiednich właściwościach
            // nazwę waluty, przelicznik, kod
            // oraz kurs średni/kupna/sprzedaży (zależy od arg. kursSredni)
            for (int i = 0; i < _count; i++)
            {
                _lista.Add(new Waluta());
                XmlNode xmlNode = doc.GetElementsByTagName("nazwa_waluty").Item(i);
                if (xmlNode != null)
                    _lista[i].Nazwa = xmlNode.InnerText;
                xmlNode = doc.GetElementsByTagName("przelicznik").Item(i);
                if (xmlNode != null)
                    _lista[i].Przelicznik = Convert.ToInt32(xmlNode.InnerText);
                xmlNode = doc.GetElementsByTagName("kod_waluty").Item(i);
                if (xmlNode != null)
                    _lista[i].Kod = xmlNode.InnerText;
                xmlNode = doc.GetElementsByTagName("nazwa_kraju").Item(i);
                if (xmlNode != null)
                    _lista[i].Kraj = xmlNode.InnerText;
                
                if (kursSredni)
                {
                    xmlNode = doc.GetElementsByTagName("kurs_sredni").Item(i);
                    if (xmlNode != null)
                        _lista[i].KursSredni =
                            double.Parse(xmlNode.InnerText, CultureInfo.GetCultureInfo("pl-PL"));
                    //Double.Parse(xmlNode.InnerText.Replace(',', '.'));
                    // Convert.ToDouble
                }
                else
                {
                    xmlNode = doc.GetElementsByTagName("kurs_kupna").Item(i);
                    if (xmlNode != null)
                        _lista[i].KursKupna =
                            double.Parse(xmlNode.InnerText, CultureInfo.GetCultureInfo("pl-PL"));
                    xmlNode = doc.GetElementsByTagName("kurs_sprzedazy").Item(i);
                    if (xmlNode != null)
                        _lista[i].KursSprzedazy =
                            double.Parse(xmlNode.InnerText, CultureInfo.GetCultureInfo("pl-PL"));
                }
            }
        }

        public string ToString(int index)  // index - indeks wybranej waluty w comboBox'ie
        {
            string kurs;

            if(Lista[index].Nazwa != null && Lista[index].Kraj != null)
                kurs = Lista[index].Kraj + " " + Lista[index].Nazwa + " " + Lista[index].Przelicznik + " " + Lista[index].Kod;
            else if (Lista[index].Nazwa != null)
                kurs = Lista[index].Nazwa + " " + Lista[index].Przelicznik + " " + Lista[index].Kod;
            else
                kurs = Lista[index].Kraj + " " + Lista[index].Przelicznik + " " + Lista[index].Kod;
            
            // czy w doc XML jest tylko kurs sredni - true? czy kupna/sprzedaży - false?
            if (KursSredni)
                // Wyswietlanie informacji w listBox'ie na temat wybranej waluty
                // gdy uwzględniany jest tylko kurs średni - Tabela A oraz Tabela B
                kurs += " " + Lista[index].KursSredni;
            else
                // Wyswietlanie informacji w listBox'ie na temat wybranej waluty
                // gdy uwzględniany jest kurs kupna/sprzedaży - Tabela C
                kurs += " kupno: " + Lista[index].KursKupna + " sprzedaż: " + Lista[index].KursSprzedazy;

            return kurs;
        }
    }
}
