using System;
using System.Collections.Generic;

namespace Project2.Models
{
    public partial class User
    {
        public User()
        {
            Bailams = new HashSet<Bailam>();
        }

        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int? Role { get; set; }

        public virtual ICollection<Bailam> Bailams { get; set; }
    }
}
