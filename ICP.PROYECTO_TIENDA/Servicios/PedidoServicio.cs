using AutoMapper;
using Icp.TiendaApi.BBDD;
using Icp.TiendaApi.BBDD.Entidades;
using Icp.TiendaApi.Controllers.DTO.Articulo;
using Icp.TiendaApi.Controllers.DTO.Pedido;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Icp.TiendaApi.Servicios
{
    public class PedidoServicio : ControllerBase
    {
        private readonly TiendaContext context;
        private readonly IMapper mapper;

        public PedidoServicio(TiendaContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Obtiene todos los pedidos de la base de datos, incluyendo los productos asociados.
        /// </summary>
        /// <returns>Una lista de objetos PedidoDTO.</returns>
        public async Task<ActionResult<List<PedidoDTO>>> GetServicio()
        {
            var pedidosDB = await context.Pedido.Include(x => x.Producto).ToListAsync();

            if (pedidosDB.Count() == 0)
            {
                return NotFound(new { message = "No hay pedidos" });
            }

            return mapper.Map<List<PedidoDTO>>(pedidosDB);
        }

        /// <summary>
        /// Crea un nuevo pedido en la base de datos.
        /// </summary>
        /// <param name="pedidoPostDTO">El objeto PedidoPostDTO que contiene los detalles del pedido.</param>
        /// <returns>Un ActionResult que indica el resultado de la operación.</returns>
        public async Task<ActionResult> PostServicio([FromForm] PedidoPostDTO pedidoPostDTO)
        {
            string estado = "Listo para enviar";

            var existeUsuario = await context.Usuario.FirstOrDefaultAsync(x => x.IdUsuario == pedidoPostDTO.UsuarioId);

            if (existeUsuario == null)
            {
                return BadRequest(new { message = "No existe el usuario que quiere crear el pedido" });
            }

            if (pedidoPostDTO.Articulos.Count == 0)
            {
                return BadRequest(new { message = "No se puede crear un pedido sin artículos" });
            }

            var articulosIds = pedidoPostDTO.Articulos.Select(x => x.ArticuloId).ToList();

            var articulosDB = await context.Articulo
                 .Where(x => articulosIds.Contains(x.IdArticulo))
                 .ToListAsync();

            foreach (var articulo in articulosDB)
            {
                if (articulo.EstadoArticulo == "Eliminado")
                {
                    return BadRequest(new { message = "Uno de los artículos enviados está eliminado" });
                }
            }
            if (pedidoPostDTO.Articulos.Count != articulosDB.Count)
            {
                return BadRequest(new { message = "No existe uno de los artículos enviados" });
            }

            var pedidoDB = new Pedido()
            {
                UsuarioId = pedidoPostDTO.UsuarioId,
                CodigoPostal = pedidoPostDTO.CodigoPostal,
                Ciudad = pedidoPostDTO.Ciudad,
                Telefono = pedidoPostDTO.Telefono,
                Contacto = pedidoPostDTO.Contacto,
                Direccion = pedidoPostDTO.Direccion,
                Provincia = pedidoPostDTO.Provincia
            };

            foreach (var productoDTO in pedidoPostDTO.Articulos)
            {
                var pedidoProducto = new Producto
                {
                    ArticuloId = productoDTO.ArticuloId,
                    Cantidad = productoDTO.Cantidad
                };

                pedidoDB.Producto.Add(pedidoProducto);

                var estanteriaDB = await context.Almacen
                    .FirstOrDefaultAsync(x => x.ArticuloAlmacen == productoDTO.ArticuloId);

                var articuloDB = await context.Articulo
                    .FirstOrDefaultAsync(x => x.IdArticulo == productoDTO.ArticuloId);

                if (productoDTO.Cantidad > estanteriaDB.Cantidad)
                {
                    estado = "Pendiente de stock";
                }
                else if (productoDTO.Cantidad > estanteriaDB.Cantidad && articuloDB.EstadoArticulo == "Pendiente de eliminar")
                {
                    return BadRequest(new { message = "Uno de los productos enviados no tiene suficiente cantidad y está pendiente de eliminar por lo que no se va a reponer, intente de nuevo con menos cantidad" });
                }

                estanteriaDB.Cantidad -= pedidoProducto.Cantidad;
            }

            pedidoDB.EstadoPedido = estado;

            context.Add(pedidoDB);

            await context.SaveChangesAsync();

            foreach (var productoDTO in pedidoPostDTO.Articulos)
            {
                var estanteriaDB = await context.Almacen
                    .FirstOrDefaultAsync(x => x.ArticuloAlmacen == productoDTO.ArticuloId);

                var articuloDB = await context.Articulo
                    .FirstOrDefaultAsync(x => x.IdArticulo == productoDTO.ArticuloId);

                if (articuloDB.EstadoArticulo == "Pendiente de eliminar" && estanteriaDB.Cantidad == 0)
                {
                    articuloDB.EstadoArticulo = "Eliminado";

                    await context.SaveChangesAsync();
                }
            }
            return Ok(new { meesage = "Pedido creado correctamente"});
        }


    }
}
