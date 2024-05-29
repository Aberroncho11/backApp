using Icp.TiendaApi.Controllers.Order.Filtros;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Icp.TiendaApi.BBDD.Modelos
{
    public partial class Order
    {
        public Order()
        {
            Items = new HashSet<Item>();
        }

        public int IdOrder { get; set; }
        public int IdUser { get; set; }
        public string PostalCode { get; set; }
        public string Town { get; set; }
        public string PhoneNumber { get; set; }
        public string PersonalContact { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string Status { get; set; }
        public User User { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
