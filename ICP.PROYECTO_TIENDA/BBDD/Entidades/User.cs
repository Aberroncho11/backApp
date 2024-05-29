using System;
using System.Collections.Generic;

namespace Icp.TiendaApi.BBDD.Modelos
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public int IdUser { get; set; }
        public int IdProfile { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }

        public Profile Profile { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
