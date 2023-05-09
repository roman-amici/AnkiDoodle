using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiDoodle.Database.DbModel
{
    public class Card
    {
        public long Id { get; set; }
        public string ContentLocation { get; set; } = null!;
        public List<Deck> Decks { get; set; } = new();
    }
}
