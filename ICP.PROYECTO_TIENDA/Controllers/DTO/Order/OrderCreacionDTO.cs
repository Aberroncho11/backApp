using Icp.TiendaApi.Controllers.DTO.Article;
using Icp.TiendaApi.Controllers.Order.Filtros;
using System.ComponentModel.DataAnnotations;

namespace Icp.TiendaApi.Controllers.DTO.Order
{
    public class OrderCreacionDTO
    {
        [Required(ErrorMessage = "IdUser requerido")]
        public int IdUser { get; set; }
        [Required(ErrorMessage = "PostalCode requerido")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "Town requerido")]
        //[StringLength(20, ErrorMessage = "La longitud máxima de Town debe ser de 20 caracteres")]
        public string Town { get; set; }
        [Required(ErrorMessage = "PhoneNumber requerido")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "PersonalContact requerido")]
        //[StringLength(20, ErrorMessage = "La longitud máxima de PersonalContact debe ser de 20 caracteres")]
        public string PersonalContact { get; set; }
        [Required(ErrorMessage = "Address requerido")]
        //[StringLength(40, ErrorMessage = "La longitud máxima de Address debe ser de 40 caracteres")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Province requerido")]
        //[StringLength(20, ErrorMessage = "La longitud máxima de Province debe ser de 20 caracteres")]
        public string Province { get; set; }
        [Required(ErrorMessage = "Articulos requeridos")]
        public List<ArticleOrderDTO> Articles { get; set; }
    }
}
