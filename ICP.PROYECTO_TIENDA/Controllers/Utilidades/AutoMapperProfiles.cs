using AutoMapper;
using Icp.TiendaApi.BBDD.Modelos;
using Icp.TiendaApi.Controllers.DTO.Article;
using Icp.TiendaApi.Controllers.DTO.Item;
using Icp.TiendaApi.Controllers.DTO.Order;
using Icp.TiendaApi.Controllers.DTO.Stock;
using Icp.TiendaApi.Controllers.DTO.User;
using Profile = AutoMapper.Profile;

namespace Icp.Tienda.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDTO>();
            CreateMap<User, UserCreacionDTO>().ReverseMap();

            CreateMap<Article, ArticleDTO>();
            CreateMap<ArticleCreacionDTO, Article>()
                .ForMember(x => x.Foto, options => options.Ignore());
            CreateMap<ArticlesStockDTO, Article>().ReverseMap();

            CreateMap<Order, OrderDTO>();
            CreateMap<OrderCreacionDTO, Order>()
                .ForMember(x => x.Items, opciones => opciones.MapFrom(MapItem));
            CreateMap<OrderDTO, Order>().ReverseMap();

            CreateMap<Item, ItemDTO>();

            CreateMap<Stock, StockDTO>();
            CreateMap<StockCreacionDTO, Stock>();
            CreateMap<User, UserDTO>().ReverseMap();
        }

        private List<Item> MapItem(OrderCreacionDTO orderCreacionDTO, Order order)
        {
            var resultado = new List<Item>();

            if(orderCreacionDTO.ArticleIds == null)
            {
                return resultado;
            }

            foreach (var article in orderCreacionDTO.ArticleIds)
            {
                resultado.Add(new Item() { ArticleId = article.IdArticle, Quantity =  article.Quantity});
            }

            return resultado;
        }
    }
}
