using System.ComponentModel.DataAnnotations;

namespace Icp.TiendaApi.Controllers.DTO.User
{
    public class UserCredencialesDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
