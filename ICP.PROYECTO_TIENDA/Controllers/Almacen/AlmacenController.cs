using Microsoft.AspNetCore.Mvc;
using Icp.TiendaApi.Controllers.DTO.Almacen;
using Icp.TiendaApi.Servicios;
using Icp.TiendaApi.Controllers.DTO.Usuario;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
namespace Icp.TiendaApi.Controllers.Almacen
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador, Gestor")]
    public class AlmacenController : ControllerBase
    {
        private readonly AlmacenServicio almacenServicio;

        public AlmacenController(AlmacenServicio almacenServicio)
        {
            this.almacenServicio = almacenServicio;
        }

        /// <summary>
        /// Añadir cantidad a un articulo del almacen
        /// </summary>
        /// <param name="almacenAddDTO"></param>
        /// <returns></returns>
        [HttpPatch("/addAlmacen")]
        public async Task<ActionResult> AddAlmacen(AlmacenAddDTO almacenAddDTO)
        {
            return await almacenServicio.AddAlmacenServicio(almacenAddDTO);
        }
    }
}
