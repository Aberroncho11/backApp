using AutoMapper;
using Icp.TiendaApi.BBDD;
using Icp.TiendaApi.Controllers.DTO.Article;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Icp.TiendaApi.BBDD.Modelos;
using Icp.TiendaApi.Servicios.Almacenador;
using Icp.TiendaApi.Controllers.DTO.User;

namespace Icp.TiendaApi.Servicios
{
    public class ArticleService : ControllerBase
    {
        private readonly TiendaContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "Imagenes";

        public ArticleService(TiendaContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        //VER ARTÍCULOS
        public async Task<ActionResult<List<ArticlesStockDTO>>> GetService()
        {
            var articles = await context.Articles
                .Include(x => x.Stocks).ToListAsync();

            return mapper.Map<List<ArticlesStockDTO>>(articles);
        }

        //VER ARTÍCULOS POR ID
        public async Task<ActionResult<ArticleDTO>> GetByIdService(int IdArticle)
        {
            var article = await context.Articles
                .FirstOrDefaultAsync(x => x.IdArticle == IdArticle);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<ArticleDTO>(article));
        }

        //CREAR ARTÍCULO
        public async Task<ActionResult> PostService([FromForm] ArticleCreacionDTO articleCreacionDTO)
        {
            var article = mapper.Map<Article>(articleCreacionDTO);

            if (articleCreacionDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await articleCreacionDTO.Foto.CopyToAsync(memoryStream);

                    var contenido = memoryStream.ToArray();

                    var extension = Path.GetExtension(articleCreacionDTO.Foto.FileName);

                    article.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor,
                        articleCreacionDTO.Foto.ContentType);
                }
            }

            context.Add(article);

            await context.SaveChangesAsync();

            var stock = new Stock
            {
                IdArticle = article.IdArticle
            };

            context.Add(stock);

            await context.SaveChangesAsync();

            return Ok();
        }

        //MODIFICAR ARTÍCULO
        public async Task<ActionResult> PutService([FromForm] ArticlePutDTO articlePutDTO, int IdArticle)
        {
            var articleDB = await context.Articles.FirstOrDefaultAsync(x => x.IdArticle == IdArticle);

            if (articleDB == null) { return NotFound(); }

            articleDB = mapper.Map(articlePutDTO, articleDB);

            if (articlePutDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await articlePutDTO.Foto.CopyToAsync(memoryStream);

                    var contenido = memoryStream.ToArray();

                    var extension = Path.GetExtension(articlePutDTO.Foto.FileName);

                    articleDB.Foto = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor,
                        articleDB.Foto,
                        articlePutDTO.Foto.ContentType);
                }
            }

            await context.SaveChangesAsync();

            return Ok();
        }

        //ELIMINAR ARTÍCULO
        public async Task<ActionResult> DeleteService(int IdArticle)
        {

            var articleDB = await context.Articles.FirstOrDefaultAsync(x => x.IdArticle == IdArticle);

            if (articleDB == null)
            {
                return NotFound();
            }

            var stockDB = await context.Stocks
                .FirstOrDefaultAsync(x => x.IdArticle == articleDB.IdArticle);

            if (stockDB == null)
            {
                return NotFound();
            }

            if (stockDB.Amount > 0)
            {
                return BadRequest("No se puede eliminar un articulo con Stock");
            }

            await almacenadorArchivos.BorrarAchivo(articleDB.Foto, contenedor);

            context.Remove(articleDB);
            await context.SaveChangesAsync();
            return Ok();
        }

    }
}
