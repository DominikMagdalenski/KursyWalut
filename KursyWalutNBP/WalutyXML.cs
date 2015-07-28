/************************************************************************
 * WalutyXML.cs -- klasa zapisująca dane z dokumentów XML               *
 * do zdefiniowanej listy walut                                         *
 * Autor: Dominik Magdaleński                                           *
 * Data: 25/07/2015                                                     *
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace KursyWalutNBP
{
    class WalutyXML
    {
        public string Nazwa { get; set; }           // nazwa tabeli
        public Boolean KursSredni { get; set; }     // czy uwzgledniany jest tylko kurs średni?
        private readonly int _count;                // ilość walut w tabeli
        public int Count { get { return _count;} }  // ilość walut w tabeli

        private readonly List<Waluta> _lista;                   // lista walut
        public List<Waluta> Lista { get { return _lista; } }    // lista walut

        // konstruktor
        // 1. arg - filename - adres URL dokumentu XML
        // 2. arg - nazwa - nazwa tabeli
        // 3. arg - kursSredni - czy w tabeli jest uwzgledniany tylko kurs sredni - true?
        // czy kurs kupna/sprzedaży - false?
        public WalutyXML(string filename, string nazwa, Boolean kursSredni)
        {
            Nazwa = nazwa;
            KursSredni = kursSredni;

            // Tworzy nowy obiekt klasy CultureInfo o nazwie culture.
            // Jest on znaczący przy parsowaniu string na double,
            // ponieważ w Polsce separatorem dziesiętnym
            // w liczbach zmiennoprzecinkowych jest przecinek
            // podczas gdy standardowo w językach programowania
            // takim separatorem jest kropka.
            // Nowy obiekt klasy CultureInfo zostanie przekazany
            // metodzie double.Parse i wtedy separatorem dziesiętnym
            // w tej metodzie będzie, w tym przypadku, przecinek.
            // Jest to zależne od podanego 'specific culture'
            // w konstruktorze CultureInfo(String)
            CultureInfo culture = new CultureInfo("pl-PL");

            // tworzenie nowego dokumentu XML
            var doc = new XmlDocument();

            // tworzenie nowej listy walut
            _lista = new List<Waluta>();

            // pobieranie dokumentu z adresu URL - filename
            doc.Load(filename);
            
            // obliczanie ilości walut
            _count = doc.GetElementsByTagName("pozycja").Count;

            // Petla dodaje waluty do listy zapamiętując w odpowiednich właściwościach
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
                    _lista[i].Przelicznik = Convert.ToDouble(xmlNode.InnerText);
                xmlNode = doc.GetElementsByTagName("kod_waluty").Item(i);
                if (xmlNode != null)
                    _lista[i].Kod = xmlNode.InnerText;

                if (kursSredni)
                {
                    xmlNode = doc.GetElementsByTagName("kurs_sredni").Item(i);
                    if (xmlNode != null)
                        _lista[i].KursSredni =
                            double.Parse(xmlNode.InnerText, culture);
                }
                else
                {
                    xmlNode = doc.GetElementsByTagName("kurs_kupna").Item(i);
                    if (xmlNode != null)
                        _lista[i].KursKupna =
                            double.Parse(xmlNode.InnerText, culture);
                    xmlNode = doc.GetElementsByTagName("kurs_sprzedazy").Item(i);
                    if (xmlNode != null)
                        _lista[i].KursSprzedazy =
                            double.Parse(xmlNode.InnerText, culture);
                }
            }
        }

        // Przeciążenie metody ToString z argumentem typu int
        // 1. arg - index - indeks wybranej waluty w comboBox'ie
        public string ToString(int index)
        {
            string kurs = Lista[index].Nazwa + " " + Lista[index].Przelicznik + " " + Lista[index].Kod;
            
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
