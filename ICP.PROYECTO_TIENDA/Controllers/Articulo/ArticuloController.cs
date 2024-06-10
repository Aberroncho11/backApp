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

        //VER ARTÍCULOS POR Nombre
        [HttpGet("/verArticuloPorNombre/{Nombre}")]
        public async Task<ActionResult<ArticuloDTO>> Get(string Nombre)
        {
            return await articuloServicio.GetByNombreServicio(Nombre);
        }

        //CREAR ARTÍCULO
        [HttpPost("/crearArticulo")]
        public async Task<ActionResult> Post([FromForm] ArticuloPostDTO articuloPostDTO)
        {
            return await articuloServicio.PostServicio(articuloPostDTO);
        }

        //MODIFICAR ARTÍCULO
        [HttpPut("/modificarArticulo/{Nombre}")]
        public async Task<ActionResult> Put([FromForm] ArticuloPutDTO articuloPutDTO, string Nombre)
        {
            return await articuloServicio.PutServicio(articuloPutDTO, Nombre);
        }

        //BORRAR FOTO
        [HttpDelete("/borrarFoto/{foto}")]
        public async Task<ActionResult> DeleteFoto(string foto)
        {
            return await articuloServicio.DeleteFotoServicio(foto);
        }

        //ELIMINAR ARTÍCULO
        [HttpDelete("/eliminarArticulo/{Nombre}")]
        public async Task<ActionResult> Delete(string Nombre)
        {
            return await articuloServicio.DeleteServicio(Nombre);
        }
    }
}
