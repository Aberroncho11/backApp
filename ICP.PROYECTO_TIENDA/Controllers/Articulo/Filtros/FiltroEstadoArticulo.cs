using System.ComponentModel.DataAnnotations;

namespace Icp.TiendaApi.Controllers.Articulo.Filtros
{
    public class FiltroEstadoArticulo : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var estado = value as string;
            if (estado == "Disponible" || estado == "Pendiente de eliminar" || estado == "Eliminado")
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Estado debe ser 'Disponible' o 'Pendiente de eliminar' o 'Eliminado'");
        }
    }
}
