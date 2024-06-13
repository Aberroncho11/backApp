using Icp.TiendaApi.BBDD.Entidades;

namespace Icp.TiendaApi
{
    public partial class Almacen
    {
        public int IdEstanteria { get; set; }
        public int? ArticuloAlmacen { get; set; }
        public int Cantidad { get; set; }
    }
}
