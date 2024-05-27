using AutoMapper;
using Icp.TiendaApi.BBDD;
using Icp.TiendaApi.Controllers.DTO.Article;
using Icp.TiendaApi.Servicios;
using Icp.TiendaApi.Servicios.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icp.TiendaApi.Controllers.Articles
{
    [ApiController]
    [Route("api/articles")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticleService articleService;
        private readonly TiendaContext context;
        private readonly IMapper mapper;

        public ArticlesController(ArticleService articleService, TiendaContext context, IMapper mapper)
        {
            this.articleService = articleService;
            this.context = context;
            this.mapper = mapper;
        }

        //Ver artículos
        //[HttpGet]
        //public async Task<List<ArticleDTO>> Get([FromQuery] PaginacionDTO paginacionDTO)
        //{
        //    var queryable = context.Articles.AsQueryable();

        //    await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegsitrosPorPagina);

        //    var articles = await queryable.Paginar(paginacionDTO).ToListAsync();

        //    return mapper.Map<List<ArticleDTO>>(articles);
        //}

        //VER ARTÍCULOS
        [HttpGet("/verArticulos")]
        public async Task<ActionResult<List<ArticlesStockDTO>>> Get()
        {
            return await articleService.GetService();
        }

        //CREAR ARTÍCULO
        [HttpPost("/crearArticulo")]
        public async Task<ActionResult> Post([FromForm] ArticleCreacionDTO articleCreacionDTO)
        {
            return await articleService.PostService(articleCreacionDTO);
        }

        //MODIFICAR ARTÍCULO
        [HttpPut("/modificarArticulo/{IdArticle:int}")]
        public async Task<ActionResult> Put([FromForm] ArticleCreacionDTO articleCreacionDTO, int IdArticle)
        {
            return await articleService.PutService(articleCreacionDTO, IdArticle);
        }

        //ELIMINAR ARTÍCULO
        [HttpDelete("/eliminarArticulo/{IdArticle:int}")]
        public async Task<ActionResult> Delete(int IdArticle)
        {
            return await articleService.DeleteService(IdArticle);
        }
    }
}
