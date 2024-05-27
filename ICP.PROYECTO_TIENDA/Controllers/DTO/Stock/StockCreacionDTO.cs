using System.ComponentModel.DataAnnotations;

namespace Icp.TiendaApi.Controllers.DTO.Stock
{
    public class StockCreacionDTO
    {
        [Required]
        public int IdArticle { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}
