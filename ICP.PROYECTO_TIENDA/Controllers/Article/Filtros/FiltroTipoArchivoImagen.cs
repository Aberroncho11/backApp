using System.ComponentModel.DataAnnotations;

namespace Icp.TiendaApi.Controllers.Articles.Filtros
{
    public class FiltroTipoArchivoImagen : ValidationAttribute
    {
        private readonly string[] tiposValidos;

        public FiltroTipoArchivoImagen(string[] tiposValidos)
        {
            this.tiposValidos = tiposValidos;
        }

        public FiltroTipoArchivoImagen(FiltroGrupoTipoArchivo grupoTipoArchivo)
        {
            if (grupoTipoArchivo == FiltroGrupoTipoArchivo.Imagen)
            {
                tiposValidos = new string[] { "image/jpeg", "image/png" };
            }
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

            if (!tiposValidos.Contains(formFile.ContentType))
            {
                return new ValidationResult($"El tipo del archivo debe ser uno de los siguientes: {string.Join(", ", tiposValidos)}");
            }

            return ValidationResult.Success;
        }
    }
}