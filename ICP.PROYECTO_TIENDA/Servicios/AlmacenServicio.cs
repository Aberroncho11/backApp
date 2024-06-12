using AutoMapper;
using Icp.TiendaApi.BBDD;
using Icp.TiendaApi.Controllers.DTO.Almacen;
using Icp.TiendaApi.Controllers.DTO.Articulo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Icp.TiendaApi.Servicios
{
    public class AlmacenServicio : ControllerBase
    {
        private readonly TiendaContext context;

        private readonly IMapper mapper;

        private ArticuloServicio articuloServicio;

        public AlmacenServicio(TiendaContext context, IMapper mapper, ArticuloServicio articuloServicio)
        {
            this.context = context;
            this.mapper = mapper;
            this.articuloServicio = articuloServicio;
        }

        // AÑADIR STOCK
        public async Task<ActionResult> AddServicio(AlmacenAddDTO almacenAddDTO)
        {
            var estanteriaDB = await context.Almacen.FirstOrDefaultAsync(x => x.IdEstanteria == almacenAddDTO.IdEstanteria);

            if (estanteriaDB == null)
            {
                return NotFound($"La estanteria con el id {almacenAddDTO.IdEstanteria} no existe");
            }

            var articuloDB = await context.Articulo.FirstOrDefaultAsync(x => x.IdArticulo == estanteriaDB.ArticuloAlmacen);

            if (articuloDB.EstadoArticulo == "Pendiente de eliminar")
            {
                return BadRequest("No se puede añadir a la estantería de este artículo porque se va a eliminar");
            }
            else if(articuloDB.EstadoArticulo == "Eliminado")
            {
                return BadRequest("No se puede añadir a la estantería de este artículo porque está eliminado");
            }

            estanteriaDB.Cantidad += almacenAddDTO.Cantidad;

            await context.SaveChangesAsync();

            var pedidosPendientes = await context.Pedido
               .Where(x => x.EstadoPedido == "Pendiente de stock")
               .Include(x => x.Producto)
               .ToListAsync();

            var almacen = await context.Almacen.ToListAsync();

            foreach (var pedido in pedidosPendientes)
            {
                bool todosLosArticulosDisponibles = true;

                foreach (var producto in pedido.Producto)
                {
                    var almacenDB = almacen.FirstOrDefault(x => x.ArticuloAlmacen == producto.ArticuloId);

                    if (almacenDB != null && almacenDB.Cantidad < producto.Cantidad)
                    {
                        todosLosArticulosDisponibles = false;
                        break;
                    }

                }

                if (todosLosArticulosDisponibles)
                {
                    pedido.EstadoPedido = "Listo para enviar";

                    foreach (var producto in pedido.Producto)
                    {
                        var almacenDB = almacen.FirstOrDefault(s => s.ArticuloAlmacen == producto.ArticuloId);

                        if (almacenDB != null)
                        {
                            almacenDB.Cantidad -= producto.Cantidad;
                        }
                    }

                }

            }

            await context.SaveChangesAsync();

            return Ok();
        }

        //public async Task<ActionResult> DeleteServicio(int ArticuloAlmacen)
        //{
        //    var existe = await context.Almacen.AnyAsync(x => x.ArticuloAlmacen == ArticuloAlmacen);

        //    if (!existe)
        //    {
        //        return NotFound();
        //    }

        //    context.Remove(new Almacen() { ArticuloAlmacen = ArticuloAlmacen });

        //    await context.SaveChangesAsync();

        //    return Ok();
        //}
    }
}
