using Icp.TiendaApi.Controllers.DTO.Almacen;

namespace Icp.TiendaApi.Controllers.DTO.Articulo
{
    public class ArticuloAlmacenDTO
    {
        public int IdArticulo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Fabricante { get; set; }
        public string Peso { get; set; }
        public string Altura { get; set; }
        public string Ancho { get; set; }
        public string Precio { get; set; }
        public string Foto { get; set; }
        public string EstadoArticulo { get; set; }
        public List<AlmacenDTO> Almacen { get; set; }
    }
}
