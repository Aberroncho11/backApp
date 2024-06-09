using Icp.TiendaApi.Controllers.Articulo.Filtros;
using System.ComponentModel.DataAnnotations;

namespace Icp.TiendaApi.Controllers.DTO.Articulo
{
    public class ArticuloPutDTO
    {
        [Required(ErrorMessage = "Descripción requerida")]
        [StringLength(50, ErrorMessage = "La longitud de la descripción debe ser máxima de 50 caracteres")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Fabricante requerido")]
        [StringLength(20, ErrorMessage = "La longitud del fabricante debe ser máxima de 20 caracteres")]
        public string Fabricante { get; set; }
        [Required(ErrorMessage = "Peso requerido")]
        public string Peso { get; set; }
        [Required(ErrorMessage = "Altura requerido")]
        public string Altura { get; set; }
        [Required(ErrorMessage = "Ancho requerido")]
        public string Ancho { get; set; }
        [Required(ErrorMessage = "Precio requerido")]
        public string Precio { get; set; }
        [Required(ErrorMessage = "Estado requerido")]
        [FiltroEstadoArticulo]
        public string EstadoArticulo { get; set; }
        [FiltroTipoArchivoImagen(grupoTipoArchivo: FiltroGrupoTipoArchivo.Imagen)]
        public IFormFile Foto { get; set; }
    }
}
