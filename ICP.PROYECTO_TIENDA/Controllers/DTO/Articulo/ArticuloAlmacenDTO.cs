using Icp.TiendaApi.Controllers.DTO.Almacen;

namespace Icp.TiendaApi.Controllers.DTO.Articulo
{
    public class ArticuloAlmacenDTO
    {
        public int IdArticulo { get; set; }
        public string Descripcion { get; set; }
        public string Fabricante { get; set; }
        public double Peso { get; set; }
        public double Altura { get; set; }
        public double Ancho { get; set; }
        public double Precio { get; set; }
        public string Foto { get; set; }
        public string EstadoArticulo { get; set; }
        public List<AlmacenDTO> Almacen { get; set; }
    }
}
