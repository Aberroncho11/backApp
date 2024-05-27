using AutoMapper;
using Icp.TiendaApi.BBDD;
using Microsoft.AspNetCore.Mvc;
using Icp.TiendaApi.Controllers.DTO.Stock;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Icp.TiendaApi.Servicios;
using Microsoft.AspNetCore.JsonPatch;

namespace Icp.TiendaApi.Controllers.Stock
{
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StockController : ControllerBase
    {
        private readonly StockService stockService;
        private readonly TiendaContext context;
        private readonly IMapper mapper;

        public StockController(StockService stockService, TiendaContext context, IMapper mapper)
        {
            this.stockService = stockService;
            this.context = context;
            this.mapper = mapper;
        }

        // AÑADIR STOCK
        [HttpPatch("/añadirStock")]
        //[Authorize(Roles = "Administrador, Gestor")]
        public async Task<ActionResult> AddStock(StockAddDTO stockAddDTO)
        {
            return await stockService.AddStockService(stockAddDTO);
        }
    }
}
