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
        /// Método que devuelve una lista de artículos
        /// </summary>
        /// <returns>Devuelve una lista de artículos</returns>
        public async Task<ActionResult<List<ArticuloAlmacenDTO>>> GetServicio()
        {
            var articulosDB = await context.Articulos
                .Include(x => x.Almacen).ToListAsync();

            return mapper.Map<List<ArticuloAlmacenDTO>>(articulosDB);
        }

        /// <summary>
        /// Método que devuelve un artículo por su id
        /// </summary>
        /// <param name="IdArticulo"></param>
        /// <returns></returns>
        public async Task<ActionResult<ArticuloDTO>> GetByIdServicio(int IdArticulo)
        {
            var articuloDB = await context.Articulos
                .FirstOrDefaultAsync(x => x.IdArticulo == IdArticulo);

            if (articuloDB == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<ArticuloDTO>(articuloDB));
        }

        /// <summary>
        /// Método que devuelve una lista de artículos por su nombre
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
        /// Método que actualiza un artículo
        /// </summary>
        /// <param name="articlePutDTO"></param>
        /// <param name="IdArticulo"></param>
        /// <returns></returns>
        public async Task<ActionResult> PutServicio([FromForm] ArticuloPutDTO articlePutDTO, int IdArticulo)
        {
            var articuloDB = await context.Articulos.FirstOrDefaultAsync(x => x.IdArticulo == IdArticulo);

            if (articuloDB == null) 
            {
                return NotFound( new { message = $"El artículo con el id {IdArticulo} no existe"}); 
            }

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
        /// Método que elimina una foto
        /// </summary>
        /// <param name="foto"></param>
        /// <returns></returns>
        public async Task<ActionResult> DeleteFotoServicio(string foto)
        {
            await almacenadorArchivos.BorrarAchivo($"./wwwroot/Imagenes/{foto}", contenedor);

            await context.SaveChangesAsync();

            return Ok();
        }


        /// <summary>
        /// Método que elimina un artículo
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

            if (articuloDB.EstadoArticulo == "Disponible" && almacenDB.Cantidad == 0)
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
