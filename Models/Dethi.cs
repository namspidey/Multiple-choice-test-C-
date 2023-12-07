using System;
using System.Collections.Generic;

namespace Project2.Models
{
    public partial class Dethi
    {
        public Dethi()
        {
            Bailams = new HashSet<Bailam>();
            Cauhois = new HashSet<Cauhoi>();
        }

        public int Id { get; set; }
        public int? MonhocId { get; set; }
        public int? Status { get; set; }

        public virtual Monhoc? Monhoc { get; set; }
        public virtual ICollection<Bailam> Bailams { get; set; }
        public virtual ICollection<Cauhoi> Cauhois { get; set; }
    }
}
