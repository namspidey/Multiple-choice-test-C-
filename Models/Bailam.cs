using System;
using System.Collections.Generic;

namespace Project2.Models
{
    public partial class Bailam
    {
        public Bailam()
        {
            TraloiDetails = new HashSet<TraloiDetail>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? DethiId { get; set; }
        public double? Mark { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int? Status { get; set; }

        public virtual Dethi? Dethi { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<TraloiDetail> TraloiDetails { get; set; }
    }
}
