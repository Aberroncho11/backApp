using AutoMapper;
using Icp.TiendaApi.BBDD;
using Icp.TiendaApi.BBDD.Modelos;
using Icp.TiendaApi.Controllers.DTO.Stock;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Icp.TiendaApi.Servicios
{
    public class StockService : ControllerBase
    {
        private readonly TiendaContext context;
        private readonly IMapper mapper;

        public StockService(TiendaContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //public async Task<List<StockDTO>> GetService()
        //{
        //    var shelfs = await context.Stocks.ToListAsync();

        //    return mapper.Map<List<StockDTO>>(shelfs);
        //}

        //public async Task<ActionResult<List<StockDTO>>> GetByIdService(int IdShelf)
        //{
        //    var shelf = await context.Stocks
        //        .Where(x => x.IdShelf == IdShelf).ToListAsync();

        //    return mapper.Map<List<StockDTO>>(shelf);
        //}

        //public async Task<ActionResult> PostService(StockCreacionDTO stockCreacionDTO)
        //{
        //    var existeArticle = await context.Articles.Where(x => x.IdArticle == stockCreacionDTO.IdArticle).ToListAsync();

        //    if(existeArticle == null)
        //    {
        //        return BadRequest("El articulo no existe");
        //    }

        //    var shelf = mapper.Map<Stock>(stockCreacionDTO);

        //    context.Add(shelf);

        //    await context.SaveChangesAsync();

        //    return Ok();
        //}

        //public async Task<ActionResult> PutService(StockCreacionDTO stockCreacionDTO, int IdShelf)
        //{
        //    var shelf = mapper.Map<Stock>(stockCreacionDTO);

        //    shelf.IdShelf = IdShelf;

        //    context.Update(shelf);

        //    await context.SaveChangesAsync();

        //    return NoContent();
        //}

        // AÑADIR STOCK
        public async Task<ActionResult> AddStockService(StockAddDTO stockAddDTO)
        {
            var shelfDB = await context.Stocks.FindAsync(stockAddDTO.IdShelf);

            if (shelfDB == null)
            {
                return NotFound();
            }

            Type orderType = typeof(Stock);

            PropertyInfo property = orderType.GetProperty("Amount");

            var articuloDB = await context.Articles.FirstOrDefaultAsync(x => x.IdArticle == shelfDB.IdArticle);

            if (articuloDB.Status == "PENDIENTE DE ELIMINAR")
            {
                return BadRequest("No se puede modificar el stock de este artículo porque se va a eliminar");
            }

            property.SetValue(shelfDB, shelfDB.Amount + stockAddDTO.Amount);

            await context.SaveChangesAsync();

            var pedidosPendientes = await context.Orders
               .Where(x => x.Status == "PENDIENTE DE STOCK")
               .Include(x => x.Items)
               .ToListAsync();

            var stocks = await context.Stocks.ToListAsync();

            foreach (var pedido in pedidosPendientes)
            {
                bool todosLosArticulosDisponibles = true;

                foreach (var item in pedido.Items)
                {
                    var stockDB = stocks.FirstOrDefault(x => x.IdArticle == item.ArticleId);

                    if (stockDB != null && stockDB.Amount < item.Quantity)
                    {
                        todosLosArticulosDisponibles = false;
                        break;
                    }

                }

                if (todosLosArticulosDisponibles)
                {
                    pedido.Status = "LISTO PARA ENVIAR";

                    foreach (var item in pedido.Items)
                    {
                        var stockDB = stocks.FirstOrDefault(s => s.IdArticle == item.ArticleId);

                        if (stockDB != null)
                        {
                            property.SetValue(stockDB, stockDB.Amount - item.Quantity);
                        }
                    }

                }

            }

            await context.SaveChangesAsync();

            return Ok();
        }

        //public async Task<ActionResult> DeleteService(int IdShelf)
        //{
        //    var existe = await context.Stocks.AnyAsync(x => x.IdShelf == IdShelf);

        //    if (!existe)
        //    {
        //        return NotFound();
        //    }

        //    context.Remove(new Stock() { IdShelf = IdShelf });

        //    await context.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}
