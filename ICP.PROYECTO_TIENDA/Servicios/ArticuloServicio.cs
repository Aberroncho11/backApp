using AutoMapper;
using Icp.TiendaApi.BBDD;
using Icp.TiendaApi.Controllers.DTO.Articulo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Icp.TiendaApi.BBDD.Entidades;
using Icp.TiendaApi.Servicios.Almacenador;
using System.Linq.Dynamic.Core;
using Icp.TiendaApi.Controllers.DTO;
using Icp.TiendaApi.Controllers.DTO.Pedido;
using System;

namespace Icp.TiendaApi.Servicios
{
    public class ArticuloServicio : ControllerBase
    {
        private readonly TiendaContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "Imagenes";

        public ArticuloServicio(TiendaContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        /// <summary>
        /// Obtiene la lista de artículos almacenados.
        /// </summary>
        /// <returns>La lista de artículos almacenados.</returns>
        public async Task<ActionResult<List<ArticuloAlmacenDTO>>> GetServicio()
        {
            var articulosDB = await context.Articulos
                .Include(x => x.Almacen).ToListAsync();
            
            if(!articulosDB.Any())
            {
                return NotFound(new { message = "No hay artículos" });
            }

            return mapper.Map<List<ArticuloAlmacenDTO>>(articulosDB);
        }

        /// <summary>
        /// Obtiene un artículo por su id.
        /// </summary>
        /// <param name="IdArticulo"></param>
        /// <returns>Los datos del artículo con el id recibido.</returns>
        public async Task<ActionResult<ArticuloDTO>> GetByIdServicio(int IdArticulo)
        {
            var articuloDB = await context.Articulos
                .FirstOrDefaultAsync(x => x.IdArticulo == IdArticulo);

            if (articuloDB == null)
            {
                return NotFound(new { message = $"No existe ningún artículo con el id {IdArticulo}" });
            }

            return Ok(mapper.Map<ArticuloDTO>(articuloDB));
        }

        /// <summary>
        /// Obtiene los artículos que coincidan con los filtros recibidos.
        /// </summary>
        /// <param name="articuloPostDTO"></param>
        /// <returns></returns>
        public async Task<ActionResult> PostServicio([FromForm] ArticuloPostDTO articuloPostDTO)
        {
            var articuloDB = mapper.Map<Articulo>(articuloPostDTO);

            if (articuloPostDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await articuloPostDTO.Foto.CopyToAsync(memoryStream);

                    var contenido = memoryStream.ToArray();

                    var extension = Path.GetExtension(articuloPostDTO.Foto.FileName);

                    articuloDB.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor, articuloPostDTO.Foto.ContentType);
                }
            }

            articuloDB.EstadoArticulo = "Disponible";

            context.Add(articuloDB);

            await context.SaveChangesAsync();

            var almacenDB = new Almacen
            {
                ArticuloAlmacen = articuloDB.IdArticulo
            };

            context.Add(almacenDB);

            await context.SaveChangesAsync();

            return Ok();
        }


        /// <summary>
        /// Actualiza un artículo existente.
        /// </summary>
        /// <param name="articlePutDTO"></param>
        /// <param name="IdArticulo"></param>
        /// <returns></returns>
        public async Task<ActionResult> PutServicio([FromForm] ArticuloPutDTO articlePutDTO, int IdArticulo)
        {
            var articuloDB = await context.Articulos.FirstOrDefaultAsync(x => x.IdArticulo == IdArticulo);

            if (articuloDB == null) { return NotFound(); }

            string foto = articuloDB.Foto;

            articuloDB = mapper.Map(articlePutDTO, articuloDB);

            if (articlePutDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await articlePutDTO.Foto.CopyToAsync(memoryStream);

                    var contenido = memoryStream.ToArray();

                    var extension = Path.GetExtension(articlePutDTO.Foto.FileName);

                    await almacenadorArchivos.BorrarAchivo($"./wwwroot/Imagenes/{Path.GetFileName(foto)}", contenedor);

                    articuloDB.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor, articlePutDTO.Foto.ContentType);

                }
            }
            else
            {
                articuloDB.Foto = null;
            }

            await context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Elimina un artículo existente.
        /// </summary>
        /// <param name="IdArticulo"></param>
        /// <returns></returns>
        public async Task<ActionResult> DeleteServicio(int IdArticulo)
        {

            var articuloDB = await context.Articulos.FirstOrDefaultAsync(x => x.IdArticulo == IdArticulo);

            if (articuloDB == null)
            {
                return NotFound();
            }

            var almacenDB = await context.Almacen
                .FirstOrDefaultAsync(x => x.ArticuloAlmacen == articuloDB.IdArticulo);

            if (articuloDB.EstadoArticulo == "Pendiente de eliminar")
            {
                return BadRequest("Este artículo ya está en proceso de eliminarse");
            }

            var pedidosConArticulo = await context.Pedidos
            .Where(x => x.Productos.Any(x => x.ArticuloId == articuloDB.IdArticulo))
            .ToListAsync();

            foreach (var pedido in pedidosConArticulo)
            {
                if(pedido.EstadoPedido == "Pendiente de stock")
                {
                    return BadRequest("No se puede eliminar el artículo porque forma parte de un pedido pendiente de stock");
                }
            }

            if(articuloDB.EstadoArticulo == "Disponible" && almacenDB.Cantidad == 0)
            {
                almacenDB.ArticuloAlmacen = null;

                articuloDB.EstadoArticulo = "Eliminado";

                await context.SaveChangesAsync();

                return Ok();
            }
            else if (articuloDB.EstadoArticulo == "Disponible" && almacenDB.Cantidad > 0)
            {
                articuloDB.EstadoArticulo = "Pendiente de eliminar";

                await context.SaveChangesAsync();

                return Ok();
            }

            return Ok();
        }


    }
}
