using System;
using System.Collections.Generic;

namespace Icp.TiendaApi.BBDD.Modelos
{
    public partial class Profile
    {
        public Profile()
        {
            Users = new HashSet<User>();
        }

        public int IdProfile { get; set; }
        public string Type { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
