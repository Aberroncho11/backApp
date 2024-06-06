using Icp.TiendaApi.Controllers.DTO.Articulo;
using Icp.TiendaApi.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;


namespace Icp.TiendaApi.Controllers.Articulo
{
    [ApiController]
    [Route("api/articulos")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
    public class ArticuloController : ControllerBase
    {
        private readonly ArticuloServicio articuloServicio;

        public ArticuloController(ArticuloServicio articuloServicio)
        {
            this.articuloServicio = articuloServicio;
        }

        //VER ARTÍCULOS
        [HttpGet("/verArticulos")]
        public async Task<ActionResult<List<ArticuloAlmacenDTO>>> GetServicio()
        {
            return await articuloServicio.GetServicio();
        }

        //VER ARTÍCULOS POR ID
        [HttpGet("/verArticuloPorId/{IdArticulo:int}")]
        public async Task<ActionResult<ArticuloDTO>> Get(int IdArticulo)
        {
            return await articuloServicio.GetByIdServicio(IdArticulo);
        }

        //CREAR ARTÍCULO
        [HttpPost("/crearArticulo")]
        public async Task<ActionResult> Post([FromForm] ArticuloPostDTO articuloPostDTO)
        {
            return await articuloServicio.PostServicio(articuloPostDTO);
        }

        //MODIFICAR ARTÍCULO
        [HttpPut("/modificarArticulo/{IdArticulo:int}")]
        public async Task<ActionResult> Put([FromForm] ArticuloPutDTO articuloPutDTO, int IdArticulo)
        {
            return await articuloServicio.PutServicio(articuloPutDTO, IdArticulo);
        }

        //BORRAR FOTO
        [HttpDelete("/borrarFoto/{IdArticulo:int}")]
        public async Task<ActionResult> DeleteFoto(int IdArticulo)
        {
            return await articuloServicio.DeleteFotoServicio(IdArticulo);
        }

        //ELIMINAR ARTÍCULO
        [HttpDelete("/eliminarArticulo/{IdArticulo:int}")]
        public async Task<ActionResult> Delete(int IdArticulo)
        {
            return await articuloServicio.DeleteServicio(IdArticulo);
        }
    }
}
