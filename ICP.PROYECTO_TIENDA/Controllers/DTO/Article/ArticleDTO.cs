using Icp.TiendaApi.BBDD.Modelos;
using Icp.TiendaApi.Controllers.DTO.Stock;

namespace Icp.TiendaApi.Controllers.DTO.Article
{
    public class ArticleDTO
    {
        public string Description { get; set; }
        public string Maker { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Price { get; set; }
        public string Foto {  get; set; }
        public string Status { get; set; }

    }
}
