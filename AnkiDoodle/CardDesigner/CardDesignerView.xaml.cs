using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnkiDoodle.CardDesigner
{
    /// <summary>
    /// Interaction logic for CardDesignerView.xaml
    /// </summary>
    public partial class CardDesignerView : UserControl
    {
        public CardDesignerView()
        {
            InitializeComponent();

            if (DataContext is CardDesignerViewModel viewModel)
            {
                viewModel.OnCardAdded += (s, e) =>
                {
                    CardFrontTextBox.Focus();
                    CardFrontTextBox.SelectAll();
                };
            }
        }

        public void Control_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is CardDesignerViewModel viewModel)
            {
                viewModel.DeckEdit = new ObservableCollection<CardDesignBasic>()
                {
                    new CardDesignBasic(0) { TextFront = "Card1 Front", TextBack = "Card1 Back"},
                    new CardDesignBasic(1) { TextFront = "Card2 Front", TextBack = "Card2 Back"},
                    new CardDesignBasic(2) { TextFront = "Card3 Front", TextBack = "Card3 Back"}
                };

                viewModel.DeckName = "Test Deck";

                viewModel.CurrentCard = viewModel.DeckEdit[0];
            }
        }

        public void Deck_KeyEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && DataContext is CardDesignerViewModel viewModel)
            {
                viewModel.DeleteCurrentCardCommand.Execute(this);
            }
        }

        public void Card_KeyEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && DataContext is CardDesignerViewModel viewModel)
            {
                viewModel.AddNewCardCommand.Execute(this);
            }
        }
    }
}
