using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiDoodle.Database.DbModel
{
    public class CardOrder
    {
        public long Order { get; set; }
        public long CardId { get; set; }
        public long DeckId { get; set; }
    }
}
