using System.ComponentModel.DataAnnotations;

namespace Icp.TiendaApi.Controllers.DTO.Almacen
{
    public class AlmacenAddDTO
    {
        [Required]
        public int ArticuloAlmacen { get; set; }
        [Required]
        public int Cantidad { get; set; }
    }
}
