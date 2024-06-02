namespace Icp.TiendaApi.BBDD.Entidades
{
    public partial class Producto
    {
        public int PedidoId { get; set; }
        public int ArticuloId { get; set; }
        public int Cantidad { get; set; }

        public Articulo Articulo { get; set; }
        public Pedido Pedido { get; set; }
    }
}
