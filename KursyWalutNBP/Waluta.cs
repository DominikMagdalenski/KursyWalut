/************************************************************************
 * Waluta.cs -- klasa reprezentująca walutę                             *
 * Autor: Dominik Magdaleński                                           *
 * Data: 25/07/2015                                                     *
 ***********************************************************************/

namespace KursyWalutNBP
{
    /// <summary>
    /// Klasa reprezentująca walutę.
    /// </summary>
    public class Waluta
    {
        public string Nazwa { get; set; }           // nazwa waluty
        public int Przelicznik { get; set; }     // przelicznik
        public string Kod { get; set; }             // kod, np. PLN, USD, itp. itd.
        public double KursSredni { get; set; }      // kurs średni
        public double KursKupna { get; set; }       // kurs kupna
        public double KursSprzedazy { get; set; }   // kurs sprzedaży
        /// <summary>
        /// Z którego dnia pochodzą dane o tej walucie.
        /// </summary>
        public string Dzien { get; set; }           // z ktorego dnia kurs waluty?

        // właściwość potrzebna z powodu różnicy we wzorcach dokumentów XML
        // do maja 2004 roku dla tabel A i C
        // oraz do kwietnia 2012 roku dla tabeli B
        /// <summary>
        /// Kraj, w którym obowiązuje waluta.
        /// </summary>
        public string Kraj { get; set; }

        // konstruktor domyślny
        public Waluta() { }

        // konstruktor z czterema argumentami
        // 1. arg - nazwa - nazwa waluty
        // 2. arg - przelicznik - przelicznik waluty
        // 3. arg - kod - kod waluty, np. PLN, EUR, itp. itd.
        // 4. arg - kurs - kurs średni waluty
        /// <summary>
        /// Konstruktor ustawiający 4 właściwości.
        /// </summary>
        /// <param name="nazwa">Nazwa waluty, np. "euro".</param>
        /// <param name="przelicznik">Przelicznik waluty, np. 1.</param>
        /// <param name="kod">Kod waluty, np. PLN, EUR, USD.</param>
        /// <param name="kurs">Kurs średni waluty, np. 4.2080.</param>
        /// <example>Waluta euro = new Waluta("euro", 1, "EUR", 4.2080);</example>
        public Waluta(string nazwa, int przelicznik, string kod, double kurs)
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
        /// <summary>
        /// Konstruktor ustawiający 5 właściwości.
        /// </summary>
        /// <param name="nazwa">Nazwa waluty, np. "funt szterling".</param>
        /// <param name="przelicznik">Przelicznik waluty, np. 1.</param>
        /// <param name="kod">Kod waluty, np. "GBP"</param>
        /// <param name="kursKupna">Kurs kupna waluty, np. 5.7369.</param>
        /// <param name="kursSprzedazy">Kurs sprzedaży waluty, np. 5.8527.</param>
        /// <example>Waluta funtSzterling = new Waluta("funt szterling", 1, "GBP", 5.7369, 5.8527);</example>
        public Waluta(string nazwa, int przelicznik, string kod, double kursKupna, double kursSprzedazy)
        {
            Nazwa = nazwa;
            Przelicznik = przelicznik;
            Kod = kod;
            KursKupna = kursKupna;
            KursSprzedazy = kursSprzedazy;
        }
    }
}
