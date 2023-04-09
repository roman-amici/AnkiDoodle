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

        private long NextCardIndex()
        {
            if (DeckEdit == null || DeckEdit.Count == 0)
            {
                return 0;
            }

            return DeckEdit.Max(x => x.CardIndex) + 1;
        }

        public event EventHandler? OnCardAdded;

        [RelayCommand]
        private void AddNewCard()
        {
            if (DeckEdit is null)
            {
                return;
            }

            var card = new CardDesignBasic(NextCardIndex()) { TextFront = "<New Card>" };
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
            CurrentCard = null;
            DeckEdit.Insert(newIndex, currentCard);
            DeckEdit.RemoveAt(index + 1);
            CurrentCard = currentCard;
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

            if (index + 1 == DeckEdit.Count)
            {
                return;
            }

            var newIndex = index + 2;
            CurrentCard = null;

            if (newIndex == DeckEdit.Count)
            {
                DeckEdit.Add(currentCard);
            }
            else
            {
                DeckEdit.Insert(newIndex, currentCard);
            }
            DeckEdit.RemoveAt(index);
            CurrentCard = currentCard;
        }
    }
}
