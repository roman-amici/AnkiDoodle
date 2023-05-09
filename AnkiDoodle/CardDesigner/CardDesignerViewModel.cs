using AnkiDoodle.DataModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiDoodle.CardDesigner
{
    internal partial class CardDesignerViewModel : ObservableObject
    {
        [ObservableProperty]
        private CardDesignBasic? currentCard;


        [ObservableProperty]
        private ObservableCollection<CardDesignBasic>? deckEdit;

        [ObservableProperty]
        private string deckName = string.Empty;

        private Deck? DeckInfo { get; set; }

        public event EventHandler? OnCardAdded;

        [RelayCommand]
        private void AddNewCard()
        {
            if (DeckEdit is null)
            {
                return;
            }

            var card = new CardDesignBasic() { TextFront = "<New Card>" };
            DeckEdit.Add(card);
            CurrentCard = card;

            OnCardAdded?.Invoke(this, new EventArgs());

        }

        [RelayCommand]
        private void DeleteCurrentCard()
        {
            if (CurrentCard is null || DeckEdit is null)
            {
                return;
            }

            var index = DeckEdit.IndexOf(CurrentCard);

            DeckEdit.Remove(CurrentCard);
            CurrentCard = null;

            if (index - 1 >= 0)
            {
                CurrentCard = DeckEdit[index - 1];
            }
            else
            {
                CurrentCard = DeckEdit.FirstOrDefault();
            }

        }

        [RelayCommand]
        private void MoveCardUp()
        {
            if (CurrentCard is null || DeckEdit is null)
            {
                return;
            }

            var currentCard = CurrentCard;
            var index = DeckEdit.IndexOf(currentCard);
            var newIndex = index - 1;
            if (newIndex < 0)
            {
                return;
            }

            DeckEdit.Move(index, newIndex);
        }

        [RelayCommand]
        private void MoveCardDown()
        {
            if (CurrentCard is null || DeckEdit is null)
            {
                return;
            }

            var currentCard = CurrentCard;
            var index = DeckEdit.IndexOf(currentCard);
            var newIndex = index + 1;

            if (newIndex == DeckEdit.Count)
            {
                return;
            }

            DeckEdit.Move(index, newIndex);
        }
    }
}
