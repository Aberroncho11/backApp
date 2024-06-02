using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Icp.TiendaApi.Controllers.Pedido.Filtros
{
    public class FiltroTelefono : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var telefono = value as string;
            if (telefono != null && Regex.IsMatch(telefono, @"^[0-9]{9}$"))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Número de teléfono no válido. Debe ser un número de teléfono español en el formato +34 seguido de 9 dígitos.");
        }
    }
}
