using System.ComponentModel.DataAnnotations;

namespace Icp.TiendaApi.Controllers.DTO.User
{
    public class UserCreacionDTO
    {
        [Required]
        public int IdProfile { get; set; }
        [Required]
        //[StringLength(20, MinimumLength = 8 , ErrorMessage = "La longitud de Password debe ser de minimo de 8 y maximo de 20 caracteres")]
        //[RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$", ErrorMessage = "La contraseña debe tener al menos una mayúscula, una minúscula y un número")]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        //[StringLength(30, ErrorMessage = "La longitud de Email debe ser de maximo de 30 caracteres")]
        public string Email { get; set; }
        [Required]
        //[StringLength(20, ErrorMessage = "La longitud de Nickname debe ser de maximo de 20 caracteres")]
        public string Nickname { get; set; }
    }
}
