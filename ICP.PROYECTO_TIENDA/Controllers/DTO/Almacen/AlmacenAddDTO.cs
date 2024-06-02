using System.ComponentModel.DataAnnotations;

namespace Icp.TiendaApi.Controllers.DTO.Almacen
{
    public class AlmacenAddDTO
    {
        [Required]
        public int IdEstanteria { get; set; }
        [Required]
        public int Cantidad { get; set; }
    }
}
