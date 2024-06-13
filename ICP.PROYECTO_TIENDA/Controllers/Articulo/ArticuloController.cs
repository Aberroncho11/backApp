using Icp.TiendaApi.Controllers.DTO.Almacen;
using Icp.TiendaApi.Controllers.DTO.Articulo;
using Icp.TiendaApi.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Icp.TiendaApi.Controllers.Articulo
{
    [ApiController]
    [Route("api/articulos")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
    public class ArticuloController : ControllerBase
    {
        private readonly ArticuloServicio articuloServicio;

        public ArticuloController(ArticuloServicio articuloServicio)
        {
            this.articuloServicio = articuloServicio;
        }

        /// <summary>
        /// Método que devuelve una lista de artículos
        /// </summary>
        /// <returns></returns>
        [HttpGet("/verArticulos")]
        public async Task<ActionResult<List<ArticuloAlmacenDTO>>> GetArticulos()
        {
            return await articuloServicio.GetServicio();
        }

        /// <summary>
        /// Método que devuelve un artículo por su nombre
        /// </summary>
        /// <param name="Nombre"></param>
        /// <returns></returns>
        [HttpGet("/verArticuloPorNombre/{Nombre}")]
        public async Task<ActionResult<ArticuloDTO>> GetArticuloPorNombre(string Nombre)
        {
            return await articuloServicio.GetByNombreServicio(Nombre);
        }

        /// <summary>
        /// Método que devuelve una lista de estanterías vacías
        /// </summary>
        /// <returns></returns>
        [HttpGet("/verEstanteriasVacias")]
        public async Task<ActionResult<List<AlmacenDTO>>> GetEstanteriasVacias()
        {
            return await articuloServicio.GetEstanteriasVaciasServicio();
        }

        /// <summary>
        /// Método que crea un artículo
        /// </summary>
        /// <param name="articuloPostDTO"></param>
        /// <returns></returns>
        [HttpPost("/crearArticulo")]
        public async Task<ActionResult> PostArticulo([FromForm] ArticuloPostDTO articuloPostDTO, [FromForm] AlmacenDTO almacenDTO)
        {
            return await articuloServicio.PostServicio(articuloPostDTO, almacenDTO);
        }

        /// <summary>
        /// Método que modifica un artículo
        /// </summary>
        /// <param name="articuloPutDTO"></param>
        /// <param name="Nombre"></param>
        /// <returns></returns>
        [HttpPut("/modificarArticulo/{Nombre}")]
        public async Task<ActionResult> PutArticulo([FromForm] ArticuloPutDTO articuloPutDTO, string Nombre)
        {
            return await articuloServicio.PutServicio(articuloPutDTO, Nombre);
        }

        /// <summary>
        /// Método que añade una foto a un artículo
        /// </summary>
        /// <param name="foto"></param>
        /// <returns></returns>
        [HttpDelete("/borrarFoto/{foto}")]
        public async Task<ActionResult> DeleteFoto(string foto)
        {
            return await articuloServicio.DeleteFotoServicio(foto);
        }

        /// <summary>
        /// Método que elimina un artículo
        /// </summary>
        /// <param name="Nombre"></param>
        /// <returns></returns>
        [HttpDelete("/eliminarArticulo/{Nombre}")]
        public async Task<ActionResult> DeleteArticulo(string Nombre)
        {
            return await articuloServicio.DeleteServicio(Nombre);
        }
    }
}
