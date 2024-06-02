using Microsoft.AspNetCore.Mvc;
using Icp.TiendaApi.Controllers.DTO.Almacen;
using Icp.TiendaApi.Servicios;
using Icp.TiendaApi.Controllers.DTO.Usuario;
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

        // VER ESTANTERIA POR ID
        [HttpGet("/verEstanteriaPorId/{IdEstanteria:int}")]
        //[Authorize(Roles = "Administrador, Gestor")]
        public async Task<ActionResult> Get(int IdEstanteria)
        {
            return await almacenServicio.GetByIdServicio(IdEstanteria);
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
