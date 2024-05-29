using AutoMapper;
using Icp.TiendaApi.BBDD;
using Icp.TiendaApi.BBDD.Modelos;
using Icp.TiendaApi.Controllers.DTO.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Icp.TiendaApi.Servicios
{
    public class OrderService : ControllerBase
    {
        private readonly TiendaContext context;
        private readonly IMapper mapper;

        public OrderService(TiendaContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // VER PEDIDOS
        public async Task<ActionResult<List<OrderDTO>>> Get()
        {
            var orders = await context.Orders
                .Include(x => x.Items).ToListAsync();
            return mapper.Map<List<OrderDTO>>(orders);
        }

        // CREAR PEDIDO
        public async Task<ActionResult> PostService([FromForm] OrderCreacionDTO orderCreacionDTO)
        {
            string status = "";

            var existeUser = await context.Users
                .Where(x => x.IdUser == orderCreacionDTO.IdUser)
                .ToListAsync();

            if (existeUser == null)
            {
                return BadRequest("No existe el usuario que quiere crear el pedido");
            }

            if (orderCreacionDTO.Articles == null)
            {
                return BadRequest("No se puede crear un pedido sin articulos");
            }

            var articleIds = orderCreacionDTO.Articles.Select(x => x.IdArticle).ToList();
            var articles = await context.Articles
                 .Where(x => articleIds.Contains(x.IdArticle))
                 .Select(x => x.IdArticle)
                 .ToListAsync();

            if (orderCreacionDTO.Articles.Count != articles.Count)
            {
                return BadRequest("No existe uno de los artículos enviados");
            }

            var order = new Order()
            {
                IdUser = orderCreacionDTO.IdUser,
                PostalCode = orderCreacionDTO.PostalCode,
                Town = orderCreacionDTO.Town,
                PhoneNumber = orderCreacionDTO.PhoneNumber,
                PersonalContact = orderCreacionDTO.PersonalContact,
                Address = orderCreacionDTO.Address,
                Province = orderCreacionDTO.Province
            };

            foreach (var itemDto in orderCreacionDTO.Articles)
            {
                var orderItem = new Item
                {
                    ArticleId = itemDto.IdArticle,
                    Quantity = itemDto.Quantity
                };

                order.Items.Add(orderItem);

                var shelfDB = await context.Stocks
                    .FirstOrDefaultAsync(x => x.IdArticle == itemDto.IdArticle);

                if (itemDto.Quantity > shelfDB.Amount)
                {
                    status = "PENDIENTE DE STOCK";
                }
                else
                {
                    status = "LISTO PARA ENVIAR";
                    shelfDB.Amount = shelfDB.Amount - itemDto.Quantity;
                }
            }

            order.Status = status;

            context.Add(order);

            await context.SaveChangesAsync();

            return Ok();
        }


    }
}
