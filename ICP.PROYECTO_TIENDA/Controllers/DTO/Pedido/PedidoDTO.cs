using Icp.TiendaApi.Controllers.DTO.Producto;

namespace Icp.TiendaApi.Controllers.DTO.Pedido
{
    public class PedidoDTO
    {
        public int IdPedido { get; set; }
        public int UsuarioId { get; set; }
        public string CodigoPostal { get; set; }
        public string Ciudad { get; set; }
        public string Telefono { get; set; }
        public string Contacto { get; set; }
        public string Direccion { get; set; }
        public string Provincia { get; set; }
        public string EstadoPedido { get; set; }
        public List<ProductoDTO> Producto { get; set; }

    }
}
