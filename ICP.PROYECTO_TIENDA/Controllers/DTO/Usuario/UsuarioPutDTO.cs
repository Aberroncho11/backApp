using Icp.TiendaApi.Controllers.Usuario.Filtros;
using System.ComponentModel.DataAnnotations;

namespace Icp.TiendaApi.Controllers.DTO.Usuario
{
    public class UsuarioPutDTO
    {
        [Required]
        [FiltroPerfil]
        public int Perfil { get; set; }
        [Required]
        [FiltroPassword]
        public string Password { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "La longitud de Email debe ser de maximo de 30 caracteres")]
        public string Email { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "La longitud de Nickname debe ser de maximo de 20 caracteres")]
        public string Nickname { get; set; }
        [Required]
        public string EstadoUsuario { get; set; }
    }
}
