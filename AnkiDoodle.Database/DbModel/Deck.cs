using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiDoodle.Database.DbModel
{
    public class Deck
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Card> Cards { get; set; } = new();
        public List<CardOrder> OrderedCards { get; set; } = new();

    }
}
