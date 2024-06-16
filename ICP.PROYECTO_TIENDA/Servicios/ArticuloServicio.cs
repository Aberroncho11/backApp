using AutoMapper;
using Icp.TiendaApi.BBDD;
using Icp.TiendaApi.Controllers.DTO.Articulo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Icp.TiendaApi.BBDD.Entidades;
using Icp.TiendaApi.Servicios.Almacenador;
using System.Linq.Dynamic.Core;
using Icp.TiendaApi.Controllers.DTO.Almacen;

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
            var articulosDB = await context.Articulo
                .Include(x => x.Almacen).ToListAsync();

            if(articulosDB.Count() == 0)
            {
                return NotFound(new { message = "No hay articulos registrados"});
            }

            return mapper.Map<List<ArticuloAlmacenDTO>>(articulosDB);
        }

        /// <summary>
        /// Método que devuelve un artículo por su nombre
        /// </summary>
        /// <param name="Nombre"></param>
        /// <returns></returns>
        public async Task<ActionResult<ArticuloDTO>> GetByNombreServicio(string Nombre)
        {
            var articuloDB = await context.Articulo
                .FirstOrDefaultAsync(x => x.Nombre == Nombre);

            if (articuloDB == null)
            {
                return NotFound(new { message = $"El articulo con el nombre {Nombre} no existe"});
            }

            return Ok(mapper.Map<ArticuloDTO>(articuloDB));
        }

        /// <summary>
        /// Método que devuelve un artículo por su id
        /// </summary>
        /// <param name="IdArticulo"></param>
        /// <returns></returns>
        public async Task<ActionResult<ArticuloDTO>> GetByIdServicio(int IdArticulo)
        {
            var articuloDB = await context.Articulo
                .FirstOrDefaultAsync(x => x.IdArticulo == IdArticulo);

            if (articuloDB == null)
            {
                return NotFound(new { message = $"El articulo con el id {IdArticulo} no existe" });
            }

            return Ok(mapper.Map<ArticuloDTO>(articuloDB));
        }

        /// <summary>
        /// Método que devuelve un artículo por su id
        /// </summary>
        /// <param name="Nombre"></param>
        /// <returns></returns>
        public async Task<ActionResult<bool>> CheckNombreServicio(string Nombre)
        {
            var articuloDB = await context.Articulo
                .FirstOrDefaultAsync(x => x.Nombre == Nombre);

            if (articuloDB != null)
            {
                return Ok(true);
            }

            return Ok(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEstanteria"></param>
        /// <returns></returns>
        public async Task<ActionResult<List<AlmacenDTO>>> GetEstanteriasVaciasServicio()
        {
            var estanteriasDB = await context.Almacen
                .Where(x => x.ArticuloAlmacen == null).ToListAsync();

            if (estanteriasDB.Count() == 0)
            {
                return NotFound(new { meesage = $"No hay estanterias vacías" });
            }

            return mapper.Map<List<AlmacenDTO>>(estanteriasDB);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEstanteria"></param>
        /// <returns></returns>
        public async Task<ActionResult<List<AlmacenDTO>>> GetEstanteriasConArticuloServicio()
        {
            var estanteriasDB = await context.Almacen
                .Where(x => x.ArticuloAlmacen != null).ToListAsync();

            if (estanteriasDB.Count() == 0)
            {
                return NotFound(new { meesage = $"No hay estanterias con artículo" });
            }

            return mapper.Map<List<AlmacenDTO>>(estanteriasDB);
        }

        /// <summary>
        /// Método que devuelve una lista de artículos por su nombre
        /// </summary>
        /// <param name="articuloPostDTO"></param>
        /// <returns></returns>
        public async Task<ActionResult> PostServicio([FromForm] ArticuloPostDTO articuloPostDTO, [FromForm] AlmacenDTO almacenDTO)
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

            var estanteriaDB = await context.Almacen.FirstOrDefaultAsync(x => x.IdEstanteria == almacenDTO.IdEstanteria);

            if (estanteriaDB == null)
            {
                return NotFound(new { message = $"La estanteria con el id {almacenDTO.IdEstanteria} no existe" });
            }

            estanteriaDB.ArticuloAlmacen = articuloDB.IdArticulo;

            estanteriaDB.Cantidad = almacenDTO.Cantidad;

            await context.SaveChangesAsync();

            return Ok();
        }


        /// <summary>
        /// Método que actualiza un artículo
        /// </summary>
        /// <param name="articlePutDTO"></param>
        /// <param name="Nombre"></param>
        /// <returns></returns>
        public async Task<ActionResult> PutServicio([FromForm] ArticuloPutDTO articlePutDTO, [FromForm] AlmacenDTO? almacenDTO, string Nombre)
        {
            var articuloDB = await context.Articulo.FirstOrDefaultAsync(x => x.Nombre == Nombre);

            if (articuloDB == null) 
            {
                return NotFound( new { message = $"El artículo con el nombre {Nombre} no existe"}); 
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

            if (almacenDTO.Cantidad != 0 && almacenDTO.IdEstanteria != 0)
            {
                var estanteriaDB = await context.Almacen.FirstOrDefaultAsync(x => x.IdEstanteria == almacenDTO.IdEstanteria);

                if (estanteriaDB == null)
                {
                    return NotFound(new { message = $"La estanteria con el id {almacenDTO.IdEstanteria} no existe" });
                }

                estanteriaDB.ArticuloAlmacen = articuloDB.IdArticulo;

                estanteriaDB.Cantidad = almacenDTO.Cantidad;
            }

            await context.SaveChangesAsync();

            return Ok(new { message = "Artículo actualizado correctamente" });
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
        /// <param name="Nombre"></param>
        /// <returns></returns>
        public async Task<ActionResult> DeleteServicio(string Nombre)
        {
            var articuloDB = await context.Articulo.FirstOrDefaultAsync(x => x.Nombre == Nombre);

            if (articuloDB == null)
            {
                return NotFound(new { message = "Artículo no encontrado" });
            }

            var almacenDB = await context.Almacen
                .FirstOrDefaultAsync(x => x.ArticuloAlmacen == articuloDB.IdArticulo);

            if (articuloDB.EstadoArticulo == "Pendiente de eliminar")
            {
                return BadRequest(new { message = "Este artículo ya está en proceso de eliminarse" });
            }

            var pedidosConArticulo = await context.Pedido
                .Where(x => x.Producto.Any(p => p.ArticuloId == articuloDB.IdArticulo))
                .ToListAsync();

            foreach (var pedido in pedidosConArticulo)
            {
                if (pedido.EstadoPedido == "Pendiente de stock")
                {
                    return BadRequest(new { message = "No se puede eliminar el artículo porque forma parte de un pedido pendiente de stock" });
                }
            }

            if (articuloDB.EstadoArticulo == "Disponible" && almacenDB.Cantidad == 0)
            {
                almacenDB.ArticuloAlmacen = null;
                articuloDB.EstadoArticulo = "Eliminado";

                await context.SaveChangesAsync();

                return Ok(new { message = "Artículo eliminado correctamente" });
            }
            else if (articuloDB.EstadoArticulo == "Disponible" && almacenDB.Cantidad > 0)
            {
                articuloDB.EstadoArticulo = "Pendiente de eliminar";

                await context.SaveChangesAsync();

                return Ok(new { message = "Artículo marcado como pendiente de eliminar" });
            }

            return BadRequest(new { message = "No se pudo eliminar el artículo" });
        }

    }
}
