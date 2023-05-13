using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiDoodle.Database.DbModel
{
    public class User
    {
        long Id { get; set; }
        string Name { get; set; } = null!;
        public virtual IEnumerable<Review>? Reviews { get; set; }
    }
}
