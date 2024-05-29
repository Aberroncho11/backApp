using System;
using System.Collections.Generic;

namespace Icp.TiendaApi.BBDD.Modelos
{
    public partial class Item
    {
        public int OrderId { get; set; }
        public int ArticleId { get; set; }
        public int Quantity { get; set; }

        public Article Article { get; set; }
        public Order Order { get; set; }
    }
}
