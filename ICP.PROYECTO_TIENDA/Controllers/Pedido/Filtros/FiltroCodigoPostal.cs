using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Icp.TiendaApi.Controllers.Pedido.Filtros
{
    public class FiltroCodigoPostal : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var codigoPostal = value as string;
            if (codigoPostal != null && Regex.IsMatch(codigoPostal, "^[0-5][0-9]{4}$"))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Código postal no válido. Debe ser un código postal español de cinco dígitos.");
        }
    }
}
