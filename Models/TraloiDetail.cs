using System;
using System.Collections.Generic;

namespace Project2.Models
{
    public partial class TraloiDetail
    {
        public int Id { get; set; }
        public int? BailamId { get; set; }
        public int? QuesId { get; set; }
        public int? Traloi { get; set; }
        public int? IsCorrect { get; set; }

        public virtual Bailam? Bailam { get; set; }
        public virtual Cauhoi? Ques { get; set; }
    }
}
