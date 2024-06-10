using Icp.TiendaApi.Controllers.DTO.Articulo;
using Icp.TiendaApi.Controllers.Pedido.Filtros;
using System.ComponentModel.DataAnnotations;

namespace Icp.TiendaApi.Controllers.DTO.Pedido
{
    public class PedidoPostDTO
    {
        [Required(ErrorMessage = "UsuarioId requerido")]
        public int UsuarioId { get; set; }
        [Required(ErrorMessage = "CodigoPostal requerido")]
        [FiltroCodigoPostal]
        public string CodigoPostal { get; set; }
        [Required(ErrorMessage = "Ciudad requerida")]
        [StringLength(20, ErrorMessage = "La longitud máxima de Ciudad debe ser de 20 caracteres")]
        public string Ciudad { get; set; }
        [Required(ErrorMessage = "Telefono requerido")]
        [FiltroTelefono]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "Contacto requerido")]
        [StringLength(20, ErrorMessage = "La longitud máxima de Contacto debe ser de 20 caracteres")]
        public string Contacto { get; set; }
        [Required(ErrorMessage = "Direccion requerida")]
        [StringLength(40, ErrorMessage = "La longitud máxima de Direccion debe ser de 40 caracteres")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "Provincia requerida")]
        public string Provincia { get; set; }
        [Required(ErrorMessage = "Articulos requeridos")]
        public List<ArticuloPedidoDTO> Articulos { get; set; }
    }
}
