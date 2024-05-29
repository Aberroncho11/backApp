using Icp.TiendaApi.Controllers.Articles.Filtros;
using System.ComponentModel.DataAnnotations;

namespace Icp.TiendaApi.Controllers.DTO.Article
{
    public class ArticlePutDTO
    {
        [Required(ErrorMessage = "Descripción requerida")]
        [StringLength(150, MinimumLength = 20, ErrorMessage = "La longitud de la descripción debe estar entre 20 y 150 caracteres")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Maker requerido")]
        [StringLength(20, ErrorMessage = "La longitud del Maker debe ser máxima de 20 caracteres")]
        public string Maker { get; set; }
        [Required(ErrorMessage = "Weight requerido")]
        public double Weight { get; set; }
        [Required(ErrorMessage = "Height requerido")]
        public double Height { get; set; }
        [Required(ErrorMessage = "Width requerido")]
        public double Width { get; set; }
        [Required(ErrorMessage = "Price requerido")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Status requerido")]
        public string Status { get; set; }
        [FiltroTamañoArchivoImagen(4)]
        [FiltroTipoArchivoImagen(grupoTipoArchivo: FiltroGrupoTipoArchivo.Imagen)]
        public IFormFile Foto { get; set; }
    }
}
