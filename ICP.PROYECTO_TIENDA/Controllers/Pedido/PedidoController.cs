using Microsoft.AspNetCore.Mvc;
using Icp.TiendaApi.Controllers.DTO.Pedido;
using Icp.TiendaApi.Servicios;

namespace Icp.TiendaApi.Controllers.Pedido
{
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoServicio pedidoServicio;

        public PedidoController(PedidoServicio pedidoServicio)
        {
            this.pedidoServicio = pedidoServicio;
        }

        // VER PEDIDOS
        [HttpGet("/verPedidos")]
        //[Authorize(Roles = "Administrador")]
        public async Task<ActionResult<List<PedidoDTO>>> Get()
        {
            return await pedidoServicio.GetServicio();
        }


        // CREAR PEDIDO
        [HttpPost("/crearPedido")]
        //[Authorize(Roles = "Administrador, Operador, Gestor")]
        public async Task<ActionResult> Post(PedidoPostDTO pedidoCreacionDTO)
        {
            return await pedidoServicio.PostServicio(pedidoCreacionDTO);
        }

    }
}
