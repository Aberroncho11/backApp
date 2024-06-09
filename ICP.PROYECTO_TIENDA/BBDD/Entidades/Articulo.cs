namespace Icp.TiendaApi.BBDD.Entidades
{
    public partial class Articulo
    {
        public Articulo()
        {
            Almacen = new HashSet<Almacen>();
            Productos = new HashSet<Producto>();
        }

        public int IdArticulo { get; set; }
        public string Descripcion { get; set; }
        public string Fabricante { get; set; }
        public string Peso { get; set; }
        public string Altura { get; set; }
        public string Ancho { get; set; }
        public string Precio { get; set; }
        public string Foto { get; set; }
        public string EstadoArticulo { get; set; }
        public virtual ICollection<Almacen> Almacen { get; set; }
        public virtual ICollection<Producto> Productos { get; set; }
    }
}
