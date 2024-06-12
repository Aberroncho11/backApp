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
            // Obtener todos los pedidos de la base de datos, incluyendo los productos asociados
            var pedidosDB = await context.Pedido.Include(x => x.Producto).ToListAsync();

            // Mapear los pedidos a DTO y devolverlos como resultado
            return mapper.Map<List<PedidoDTO>>(pedidosDB);
        }

        /// <summary>
        /// Crea un nuevo pedido en la base de datos.
        /// </summary>
        /// <param name="pedidoPostDTO">El objeto PedidoPostDTO que contiene los detalles del pedido.</param>
        /// <returns>Un ActionResult que indica el resultado de la operación.</returns>
        public async Task<ActionResult> PostServicio([FromForm] PedidoPostDTO pedidoPostDTO)
        {
            // Establecer el estado del pedido como "Listo para enviar" por defecto
            string estado = "Listo para enviar";

            // Verificar si el usuario existe en la base de datos
            var existeUsuario = await context.Usuario.FirstOrDefaultAsync(x => x.IdUsuario == pedidoPostDTO.UsuarioId);

            // Si el usuario no existe, devolver un error
            if (existeUsuario == null)
            {
                // Devolver un error indicando que el usuario no existe
                return BadRequest("No existe el usuario que quiere crear el pedido");
            }

            // Verificar si se enviaron artículos en el pedido
            if (pedidoPostDTO.Articulos.Count == 0)
            {
                // Devolver un error indicando que no se pueden crear pedidos sin artículos
                return BadRequest("No se puede crear un pedido sin artículos");
            }

            // Obtener los IDs de los artículos enviados en el pedido
            var articulosIds = pedidoPostDTO.Articulos.Select(x => x.ArticuloId).ToList();

            // Obtener los artículos de la base de datos que corresponden a los IDs enviados
            var articulosDB = await context.Articulo
                 .Where(x => articulosIds.Contains(x.IdArticulo))
                 .ToListAsync();

            // Verificar si alguno de los artículos enviados está eliminado
            foreach (var articulo in articulosDB)
            {
                // Si el artículo está eliminado, devolver un error
                if (articulo.EstadoArticulo == "Eliminado")
                {
                    // Devolver un error indicando que uno de los artículos está eliminado
                    return BadRequest("Uno de los artículos enviados está eliminado");
                }
            }

            // Verificar si se enviaron todos los artículos solicitados
            if (pedidoPostDTO.Articulos.Count != articulosDB.Count)
            {
                // Devolver un error indicando que no se enviaron todos los artículos solicitados
                return BadRequest("No existe uno de los artículos enviados");
            }

            // Crear un nuevo objeto Pedido con los detalles del pedido enviado
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

            // Agregar los productos del pedido al objeto Pedido
            foreach (var productoDTO in pedidoPostDTO.Articulos)
            {
                // Crear un nuevo objeto Producto con los detalles del producto enviado
                var pedidoProducto = new Producto
                {
                    ArticuloId = productoDTO.ArticuloId,
                    Cantidad = productoDTO.Cantidad
                };

                // Agregar el producto al pedido
                pedidoDB.Producto.Add(pedidoProducto);

                // Obtener el artículo y la estantería correspondientes al producto del pedido
                var estanteriaDB = await context.Almacen
                    .FirstOrDefaultAsync(x => x.ArticuloAlmacen == productoDTO.ArticuloId);

                var articuloDB = await context.Articulo
                    .FirstOrDefaultAsync(x => x.IdArticulo == productoDTO.ArticuloId);

                // Verificar si hay suficiente stock para el producto del pedido
                if (productoDTO.Cantidad > estanteriaDB.Cantidad)
                {
                    // Si no hay suficiente stock, establecer el estado del pedido como "Pendiente de stock"
                    estado = "Pendiente de stock";
                }
                // Verificar si el artículo está pendiente de eliminar y si hay suficiente stock
                else if (productoDTO.Cantidad > estanteriaDB.Cantidad && articuloDB.EstadoArticulo == "Pendiente de eliminar")
                {
                    // Si no hay suficiente stock, establecer el estado del pedido como "Pendiente de stock"
                    return BadRequest("Uno de los productos enviados no tiene suficiente cantidad y está pendiente de eliminar por lo que no se va a reponer, intente de nuevo con menos cantidad");
                }

                // Actualizar la cantidad en la estantería
                estanteriaDB.Cantidad -= pedidoProducto.Cantidad;
            }

            // Establecer el estado del pedido
            pedidoDB.EstadoPedido = estado;

            // Agregar el pedido a la base de datos
            context.Add(pedidoDB);

            await context.SaveChangesAsync();

            // Verificar si algún artículo pendiente de eliminar se quedó sin stock
            foreach (var productoDTO in pedidoPostDTO.Articulos)
            {
                // Obtener la estantería y el artículo correspondientes al producto del pedido
                var estanteriaDB = await context.Almacen
                    .FirstOrDefaultAsync(x => x.ArticuloAlmacen == productoDTO.ArticuloId);

                var articuloDB = await context.Articulo
                    .FirstOrDefaultAsync(x => x.IdArticulo == productoDTO.ArticuloId);

                // Verificar si el artículo está pendiente de eliminar y si se quedó sin stock
                if (articuloDB.EstadoArticulo == "Pendiente de eliminar" && estanteriaDB.Cantidad == 0)
                {
                    // Establecer el estado del artículo como "Eliminado"
                    articuloDB.EstadoArticulo = "Eliminado";

                    estanteriaDB.ArticuloAlmacen = null;

                    // Actualizar el artículo en la base de datos
                    await context.SaveChangesAsync();
                }
            }
            // Devolver un mensaje de éxito
            return Ok();
        }


    }
}
