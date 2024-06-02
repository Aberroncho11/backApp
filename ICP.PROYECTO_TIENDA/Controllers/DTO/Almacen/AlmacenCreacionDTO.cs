using System.ComponentModel.DataAnnotations;

namespace Icp.TiendaApi.Controllers.DTO.Almacen
{
    public class AlmacenCreacionDTO
    {
        [Required]
        public int IdArticulo { get; set; }
        [Required]
        public int Cantidad { get; set; }
    }
}
