using System;
using System.Collections.Generic;

namespace Project2.Models
{
    public partial class Cauhoi
    {
        public Cauhoi()
        {
            TraloiDetails = new HashSet<TraloiDetail>();
        }

        public int Id { get; set; }
        public string? NoiDung { get; set; }
        public string? A { get; set; }
        public string? B { get; set; }
        public string? C { get; set; }
        public string? D { get; set; }
        public int? DapAn { get; set; }
        public int? DethiId { get; set; }

        public virtual Dethi? Dethi { get; set; }
        public virtual ICollection<TraloiDetail> TraloiDetails { get; set; }
    }
}
