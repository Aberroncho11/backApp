using System;
using System.Collections.Generic;

namespace Icp.TiendaApi.BBDD.Modelos
{
    public partial class Stock
    {
        public int IdShelf { get; set; }
        public int IdArticle { get; set; }
        public int Amount { get; set; }

        public Article Article { get; set; }
    }
}
