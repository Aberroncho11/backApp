using Icp.TiendaApi.Controllers.Usuario.Filtros;
using System.ComponentModel.DataAnnotations;

namespace Icp.TiendaApi.Controllers.DTO.Usuario
{
    public class UsuarioCredencialesDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [FiltroPassword]
        public string Password { get; set; }
    }
}
