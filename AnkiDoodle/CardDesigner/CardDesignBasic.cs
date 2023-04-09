using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiDoodle.CardDesigner
{
    internal partial class CardDesignBasic : ObservableObject
    {
        public CardDesignBasic(long cardIndex)
        {
            CardIndex = cardIndex;
        }

        public readonly long CardIndex;

        [ObservableProperty]
        private string textFront = string.Empty;

        [ObservableProperty]
        private string textBack = string.Empty;

    }
}
