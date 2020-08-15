namespace KursyWalutNBP
{
    public class Waluta
    {
        public string Nazwa { get; set; }
        public int Przelicznik { get; set; }
        public string Kod { get; set; }             // np. PLN, USD, itp. itd.
        public double KursSredni { get; set; }
        public double KursKupna { get; set; }
        public double KursSprzedazy { get; set; }
        /// <summary>
        /// Z którego dnia pochodzą dane o walucie.
        /// </summary>
        public string Dzien { get; set; }

        // Właściwość potrzebna z powodu różnicy we wzorcach dokumentów XML
        // do maja 2004 roku dla tabel A i C
        // oraz do kwietnia 2012 roku dla tabeli B
        /// <summary>
        /// Kraj, w którym obowiązuje waluta.
        /// </summary>
        public string Kraj { get; set; }

        public Waluta() { }

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
