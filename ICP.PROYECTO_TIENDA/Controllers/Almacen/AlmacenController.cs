using Microsoft.AspNetCore.Mvc;
using Icp.TiendaApi.Controllers.DTO.Almacen;
using Icp.TiendaApi.Servicios;
using Icp.TiendaApi.Controllers.DTO.Usuario;
using Icp.TiendaApi.BBDD.Entidades;

namespace Icp.TiendaApi.Controllers.Almacen
{
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AlmacenController : ControllerBase
    {
        private readonly AlmacenServicio almacenServicio;

        public AlmacenController(AlmacenServicio almacenServicio)
        {
            this.almacenServicio = almacenServicio;
        }

        // AÑADIR STOCK
        [HttpPatch("/addAlmacen")]
        //[Authorize(Roles = "Administrador, Gestor")]
        public async Task<ActionResult> AddAlmacen(AlmacenAddDTO almacenAddDTO)
        {
            return await almacenServicio.AddServicio(almacenAddDTO);
        }
    }
}
