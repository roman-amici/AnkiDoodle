﻿using System;
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
        public virtual IEnumerable<Deck>? Decks { get; set; }
        public virtual IEnumerable<Review>? Reviews { get; set; }
    }
}
