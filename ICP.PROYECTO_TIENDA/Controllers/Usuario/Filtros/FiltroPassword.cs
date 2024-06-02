namespace Icp.TiendaApi.Controllers.Usuario.Filtros
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public class FiltroPassword : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;
            if (password != null && Regex.IsMatch(password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$"))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("La contraseña debe tener al menos una letra mayúscula, una letra minúscula, un número y ser de al menos 8 caracteres.");
        }
    }

}
