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

        //VER ARTÍCULOS
        public async Task<ActionResult<List<ArticuloAlmacenDTO>>> GetServicio()
        {
            var articulosDB = await context.Articulos
                .Include(x => x.Almacen).ToListAsync();

            return mapper.Map<List<ArticuloAlmacenDTO>>(articulosDB);
        }
        //VER ARTÍCULOS POR ID
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

        //CREAR ARTÍCULO
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


        //MODIFICAR ARTÍCULO
        public async Task<ActionResult> PutServicio([FromForm] ArticuloPutDTO articlePutDTO, int IdArticulo)
        {
            var articuloDB = await context.Articulos.FirstOrDefaultAsync(x => x.IdArticulo == IdArticulo);

            var articuloDBFoto = mapper.Map<ArticuloDTO>(articlePutDTO);

            string foto = articuloDBFoto.Foto;

            if (articuloDB == null) { return NotFound(); }

            articuloDB = mapper.Map(articlePutDTO, articuloDB);

            if (articlePutDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await articlePutDTO.Foto.CopyToAsync(memoryStream);

                    var contenido = memoryStream.ToArray();

                    var extension = Path.GetExtension(articlePutDTO.Foto.FileName);

                    await almacenadorArchivos.BorrarAchivo($"./wwwroot/Imagenes/{foto}", contenedor);

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

        //BORRAR FOTO
        public async Task<ActionResult> DeleteFotoServicio(int IdArticulo)
        {
            var articuloDB = await context.Articulos.FirstOrDefaultAsync(x => x.IdArticulo == IdArticulo);

            if (articuloDB == null)
            {
                return NotFound();
            }

            var articuloDBFoto = mapper.Map<ArticuloDTO>(articuloDB);

            await almacenadorArchivos.BorrarAchivo($"./wwwroot/Imagenes/{articuloDBFoto.Foto}", contenedor);

            await context.SaveChangesAsync();

            return Ok();
        }


        //ELIMINAR ARTÍCULO
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

            articuloDB.EstadoArticulo = "Pendiente de eliminar";
            
            await context.SaveChangesAsync(); ;
            return Ok();
        }

    }
}
