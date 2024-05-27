using System.ComponentModel.DataAnnotations;

namespace Icp.TiendaApi.Controllers.Order.Filtros
{
    public class EstadoOrderFiltro : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var estado = value.ToString();

            if(estado != "LISTO PARA ENVIAR" && estado != "PENDIENTE DE STOCK" && estado != "ENVIADO")
            {
                return new ValidationResult("El estado debe ser uno de los tres permitidos");
            }

            return ValidationResult.Success;
        }
    }
}
