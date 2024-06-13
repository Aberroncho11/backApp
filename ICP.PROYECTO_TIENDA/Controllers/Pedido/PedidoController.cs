using Microsoft.AspNetCore.Mvc;
using Icp.TiendaApi.Controllers.DTO.Pedido;
using Icp.TiendaApi.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Icp.TiendaApi.Controllers.Pedido
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoServicio pedidoServicio;

        public PedidoController(PedidoServicio pedidoServicio)
        {
            this.pedidoServicio = pedidoServicio;
        }

        /// <summary>
        /// Obtiene todos los pedidos de la base de datos.
        /// </summary>
        /// <returns></returns>
        [HttpGet("/verPedidos")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<List<PedidoDTO>>> GetPedidos()
        {
            return await pedidoServicio.GetServicio();
        }


        /// <summary>
        /// Crea un nuevo pedido en la base de datos.
        /// </summary>
        /// <param name="pedidoCreacionDTO"></param>
        /// <returns></returns>
        [HttpPost("/crearPedido")]
        [Authorize(Roles = "Administrador, Gestor, Operador")]
        public async Task<ActionResult> PostPedido(PedidoPostDTO pedidoCreacionDTO)
        {
            return await pedidoServicio.PostServicio(pedidoCreacionDTO);
        }

    }
}
