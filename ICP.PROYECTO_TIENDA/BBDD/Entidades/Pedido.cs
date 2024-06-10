namespace Icp.TiendaApi.BBDD.Entidades
{
    public partial class Pedido
    {
        public Pedido()
        {
            Producto = new HashSet<Producto>();
        }

        public int IdPedido { get; set; }
        public int UsuarioId { get; set; }
        public string CodigoPostal { get; set; }
        public string Ciudad { get; set; }
        public string Telefono { get; set; }
        public string Contacto { get; set; }
        public string Direccion { get; set; }
        public string Provincia { get; set; }
        public string EstadoPedido { get; set; }

        public Usuario Usuario { get; set; }
        public ICollection<Producto> Producto { get; set; }
    }
}
