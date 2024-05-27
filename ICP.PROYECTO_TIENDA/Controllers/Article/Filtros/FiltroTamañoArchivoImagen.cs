using System.ComponentModel.DataAnnotations;

namespace Icp.TiendaApi.Controllers.Articles.Filtros
{
    public class FiltroTamañoArchivoImagen : ValidationAttribute
    {
        private readonly int filtroImagen;

        public FiltroTamañoArchivoImagen(int FiltroImagen)
        {
            filtroImagen = FiltroImagen;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if (formFile.Length > filtroImagen * 1024 * 1024)
            {
                return new ValidationResult($"El peso maximo no debe ser mayor a {filtroImagen}mb");
            }

            return ValidationResult.Success;
        }
    }
}
