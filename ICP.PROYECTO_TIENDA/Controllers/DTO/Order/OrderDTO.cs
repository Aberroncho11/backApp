using Icp.TiendaApi.Controllers.DTO.Item;
using Icp.TiendaApi.Controllers.Order.Filtros;
using System.ComponentModel.DataAnnotations;

namespace Icp.TiendaApi.Controllers.DTO.Order
{
    public class OrderDTO
    {
        public int IdOrder { get; set; }
        public int IdUser { get; set; }
        public int PostalCode { get; set; }
        public string Town { get; set; }
        public int PhoneNumber { get; set; }
        public string PersonalContact { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string Status { get; set; }
        public List<ItemDTO> Items { get; set; }

    }
}
