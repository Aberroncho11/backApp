using Icp.TiendaApi.BBDD.Entidades;
using Icp.TiendaApi.Controllers.DTO.Articulo;
using Icp.TiendaApi.Controllers.DTO.Producto;
using Icp.TiendaApi.Controllers.DTO.Pedido;
using Icp.TiendaApi.Controllers.DTO.Almacen;
using Icp.TiendaApi.Controllers.DTO.Usuario;
using Profile = AutoMapper.Profile;
using Icp.TiendaApi;

namespace Icp.Tienda.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioPostDTO>().ReverseMap();

            CreateMap<Articulo, ArticuloDTO>().ReverseMap();
            CreateMap<Articulo, ArticuloPostDTO>()
                .ForMember(x => x.Foto, options => options.Ignore());
            CreateMap<Articulo, ArticuloAlmacenDTO>().ReverseMap();
            CreateMap<Articulo, ArticuloPutDTO>().ReverseMap();
            CreateMap<ArticuloDTO, ArticuloPutDTO>().ReverseMap();
            CreateMap<Articulo, ArticuloPostDTO>().ReverseMap();

            CreateMap<Pedido, PedidoDTO>();
            CreateMap<PedidoPostDTO, Pedido>()
                .ForMember(x => x.Producto, opciones => opciones.MapFrom(MapItem));
            CreateMap<Pedido, PedidoDTO>().ReverseMap();

            CreateMap<Producto, ProductoDTO>();

            CreateMap<Almacen, AlmacenDTO>().ReverseMap();
            CreateMap<Almacen, AlmacenAddDTO>().ReverseMap();
        }

        private List<Producto> MapItem(PedidoPostDTO pedidoPostDTO, Pedido pedido)
        {
            var resultado = new List<Producto>();

            if(pedidoPostDTO.Articulos == null)
            {
                return resultado;
            }

            foreach (var article in pedidoPostDTO.Articulos)
            {
                resultado.Add(new Producto() { ArticuloId = article.ArticuloId, Cantidad =  article.Cantidad });
            }

            return resultado;
        }
    }
}
