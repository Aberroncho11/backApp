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
            CreateMap<Usuario, UsuarioDTO>();
            CreateMap<Usuario, UsuarioPostDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioGetPorIdDTO>().ReverseMap();

            CreateMap<Articulo, ArticuloDTO>().ReverseMap();
            CreateMap<ArticuloPostDTO, Articulo>()
                .ForMember(x => x.Foto, options => options.Ignore());
            CreateMap<ArticuloAlmacenDTO, Articulo>().ReverseMap();
            CreateMap<ArticuloPutDTO, Articulo>().ReverseMap();

            CreateMap<Pedido, PedidoDTO>();
            CreateMap<PedidoPostDTO, Pedido>()
                .ForMember(x => x.Productos, opciones => opciones.MapFrom(MapItem));
            CreateMap<PedidoDTO, Pedido>().ReverseMap();

            CreateMap<Producto, ProductoDTO>();

            CreateMap<Almacen, AlmacenDTO>();
            CreateMap<AlmacenCreacionDTO, Almacen>();
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
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
