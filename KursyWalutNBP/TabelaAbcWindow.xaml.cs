using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace KursyWalutNBP
{
    /// <summary>
    /// Interaction logic for TabelaAB.xaml
    /// </summary>
    public partial class TabelaAbcWindow : Window
    {
        public TabelaAbcWindow()
        {
            InitializeComponent();
        }

        public TabelaAbcWindow(WalutyXml tabela)
        {
            InitializeComponent();
            if (tabela.Nazwa == "Tabela C")
            {
                TabelaGridView.Columns.Remove(SredniCol);
                WalutaCol.Width = 205;
                TabelaGridView.Columns.Add(new GridViewColumn{Header = "Kupno", Width = 55, DisplayMemberBinding = new Binding("KursKupna")});
                TabelaGridView.Columns.Add(new GridViewColumn{Header = "Sprzedaż", Width = 55, DisplayMemberBinding = new Binding("KursSprzedazy")});
            }
            TabelaListView.ItemsSource = tabela.Lista;
        }
    }
}
