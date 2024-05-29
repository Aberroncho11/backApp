using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Metadata;
using Icp.TiendaApi.Controllers.DTO.User;
using Icp.TiendaApi.BBDD;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Icp.TiendaApi.Servicios;


namespace Icp.TiendaApi.Controllers.User
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService userService;
        private readonly TiendaContext context;
        private readonly IMapper mapper; 

        public UsersController(UserService userService, TiendaContext context, IMapper mapper)
        {
            this.userService = userService;
            this.context = context;
            this.mapper = mapper;
        }

        // LOGIN
        [HttpPost("/login")]
        public async Task<ActionResult<UserRespuestaAutenticacionDTO>> Login(UserCredencialesDTO userCredencialesDTO)
        {
            return await userService.LoginService(userCredencialesDTO);
        }

        // VER USUARIOS
        [HttpGet("/verUsuarios")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<List<UserDTO>>> Get()
        {
            return await userService.GetService();
        }

        //VER USUARIOS POR ID
        [HttpGet("/verUsuariosPorId/{IdUsuario:int}")]
        public async Task<ActionResult<UserGetPorIdDTO>> Get(int IdUsuario)
        {
            return await userService.GetByIdService(IdUsuario);
        }

        // CREAR USUARIOS
        [HttpPost("/crearUsuarios")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Post([FromForm] UserCreacionDTO userCreacionDTO)
        {
            return await userService.PostService(userCreacionDTO);
        }

        // MODIFICAR USUARIOS
        [HttpPut("/modificarUsuarios/{IdUser:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Put([FromForm] UserCreacionDTO userCreacionDTO, int IdUser)
        {
            return await userService.PutService(userCreacionDTO, IdUser);
        }

        // ELIMINAR USUARIOS
        [HttpDelete("/eliminarUsuarios/{IdUser:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Delete(int IdUser)
        {
            return await userService.DeleteService(IdUser);
        }
    }
}
