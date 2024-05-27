using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Icp.TiendaApi.Controllers.DTO.Order;
using Icp.TiendaApi.Controllers.DTO.Article;
using System.Reflection;
using Icp.TiendaApi.BBDD;
using Icp.TiendaApi.BBDD.Modelos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Icp.TiendaApi.Servicios;
using Microsoft.AspNetCore.JsonPatch;

namespace Icp.TiendaApi.Controllers.Order
{
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController : ControllerBase
    {
        private readonly OrderService orderService;
        private readonly TiendaContext context;
        private readonly IMapper mapper;

        public OrderController(OrderService orderService, TiendaContext context, IMapper mapper)
        {
            this.orderService = orderService;
            this.context = context;
            this.mapper = mapper;
        }

        // VER PEDIDOS
        [HttpGet("/verPedidos")]
        //[Authorize(Roles = "Administrador")]
        public async Task<ActionResult<List<OrderDTO>>> Get()
        {
            return await orderService.Get();
        }


        // CREAR PEDIDO
        [HttpPost("/crearPedido")]
        //[Authorize(Roles = "Administrador, Operador, Gestor")]
        public async Task<ActionResult> Post(OrderCreacionDTO orderCreacionDTO)
        {
            return await orderService.PostService(orderCreacionDTO);
        }

    }
}
