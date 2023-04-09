using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiDoodle.DataModel
{
    internal class BasicCard : Card
    {
        public string TextFront { get; set; } = string.Empty;
        public string TextBack { get; set; } = string.Empty;
    }
}
