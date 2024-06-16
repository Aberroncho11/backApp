﻿using Icp.TiendaApi.Controllers.DTO.Almacen;
using Icp.TiendaApi.Controllers.DTO.Articulo;
using Icp.TiendaApi.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Icp.TiendaApi.Controllers.Articulo
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        /// Método que devuelve un artículo por su nombre
        /// </summary>
        /// <param name="Nombre"></param>
        /// <returns></returns>
        [HttpGet("/verArticuloPorId/{IdArticulo:int}")]
        [Authorize(Roles = "Administrador, Gestor")]
        public async Task<ActionResult<ArticuloDTO>> GetArticuloPorId(int IdArticulo)
        {
            return await articuloServicio.GetByIdServicio(IdArticulo);
        }

        /// <summary>
        /// Método que devuelve una lista de usuarios
        /// </summary>
        /// <param name="Nickname"></param>
        /// <returns></returns>
        [HttpGet("/checkNombre/{Nombre}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<bool>> CheckNombre(string Nombre)
        {
            return await articuloServicio.CheckNombreServicio(Nombre);
        }

        /// <summary>
        /// Método que devuelve una lista de estanterías vacías
        /// </summary>
        /// <returns></returns>
        [HttpGet("/verEstanteriasVacias")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<List<AlmacenDTO>>> GetEstanteriasVacias()
        {
            return await articuloServicio.GetEstanteriasVaciasServicio();
        }

        /// <summary>
        /// Método que devuelve una lista de estanterías vacías
        /// </summary>
        /// <returns></returns>
        [HttpGet("/verEstanteriasConArticulos")]
        public async Task<ActionResult<List<AlmacenDTO>>> GetEstanteriasConArticulos()
        {
            return await articuloServicio.GetEstanteriasConArticuloServicio();
        }

        /// <summary>
        /// Método que crea un artículo
        /// </summary>
        /// <param name="articuloPostDTO"></param>
        /// <returns></returns>
        [HttpPost("/crearArticulo")]
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> PutArticulo([FromForm] ArticuloPutDTO articuloPutDTO, [FromForm] AlmacenDTO? almacenDTO, string Nombre)
        {
            return await articuloServicio.PutServicio(articuloPutDTO, almacenDTO, Nombre);
        }

        /// <summary>
        /// Método que añade una foto a un artículo
        /// </summary>
        /// <param name="foto"></param>
        /// <returns></returns>
        [HttpDelete("/borrarFoto/{foto}")]
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> DeleteArticulo(string Nombre)
        {
            return await articuloServicio.DeleteServicio(Nombre);
        }
    }
}
