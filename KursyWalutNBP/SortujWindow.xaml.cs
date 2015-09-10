using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace KursyWalutNBP
{
    /// <summary>
    /// Interaction logic for SortujWindow.xaml
    /// </summary>
    public partial class SortujWindow : Window
    {
        // referencja do głównego okna aplikacji
        private readonly MainWindow _mainWindow;
        private readonly bool _arch;

        public SortujWindow()
        {
            InitializeComponent();
        }

        // konstruktor z argumentem typu MainWindow
        public SortujWindow(MainWindow mainWindowArg)
        {
            InitializeComponent();
            _mainWindow = mainWindowArg;
            _arch = Equals(_mainWindow.WLista, _mainWindow.listaWalutArch);
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
            /*if (sortujCB.SelectedIndex > -1)
            {
                if (!_arch)
                {
                    CollectionView view =
                        (CollectionView) CollectionViewSource.GetDefaultView(_mainWindow.listaWalutAkt);
                    view.SortDescriptions.Add(new SortDescription(sortujCB.SelectedItem.ToString(), ListSortDirection.Ascending));
                }
                else
                {
                    CollectionView view =
                        (CollectionView)CollectionViewSource.GetDefaultView(_mainWindow.listaWalutArch);
                    view.SortDescriptions.Add(new SortDescription(sortujCB.SelectedItem.ToString(), ListSortDirection.Ascending));
                }
                Close();
            }*/
        }
    }
}
