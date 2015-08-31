/************************************************************************
 * Waluta.cs -- klasa reprezentująca walutę                             *
 * Autor: Dominik Magdaleński                                           *
 * Data: 25/07/2015                                                     *
 ***********************************************************************/

namespace KursyWalutNBP
{
    class Waluta
    {
        public string Nazwa { get; set; }           // nazwa waluty
        public double Przelicznik { get; set; }     // przelicznik
        public string Kod { get; set; }             // kod, np. PLN, USD, itp. itd.
        public double KursSredni { get; set; }      // kurs średni
        public double KursKupna { get; set; }       // kurs kupna
        public double KursSprzedazy { get; set; }   // kurs sprzedaży

        // właściwości potrzebne z powodu różnicy we wzorcach dokumentów XML
        // do maja 2004 roku dla tabel A i C
        // oraz tylko NazwaKraju do kwietnia 2012 roku dla tabeli B
        public string NazwaKraju { get; set; }      // nazwa kraju
        //public int SymbolWaluty { get; set; }       // symbol waluty

        // konstruktor domyślny
        public Waluta() { }

        // konstruktor z czterema argumentami
        // 1. arg - nazwa - nazwa waluty
        // 2. arg - przelicznik - przelicznik waluty
        // 3. arg - kod - kod waluty, np. PLN, EUR, itp. itd.
        // 4. arg - kurs - kurs średni waluty
        public Waluta(string nazwa, double przelicznik, string kod, double kurs)
        {
            Nazwa = nazwa;
            Przelicznik = przelicznik;
            Kod = kod;
            KursSredni = kurs;
        }

        // konstruktor z pięcioma argumentami
        // 1. arg - nazwa - nazwa waluty
        // 2. arg - przelicznik - przelicznik waluty
        // 3. arg - kod - kod waluty, np. PLN, EUR, itp. itd.
        // 4. arg - kursKupna - kurs kupna waluty
        // 5. arg - kursSprzedazy - kurs sprzedaży waluty
        public Waluta(string nazwa, double przelicznik, string kod, double kursKupna, double kursSprzedazy)
        {
            Nazwa = nazwa;
            Przelicznik = przelicznik;
            Kod = kod;
            KursKupna = kursKupna;
            KursSprzedazy = kursSprzedazy;
        }
    }
}
