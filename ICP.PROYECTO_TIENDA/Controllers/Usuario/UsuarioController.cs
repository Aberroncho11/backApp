using Microsoft.AspNetCore.Mvc;
using Icp.TiendaApi.Controllers.DTO.Usuario;
using Icp.TiendaApi.Servicios;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace Icp.TiendaApi.Controllers.User
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsuarioServicio usuarioServicio;

        public UsersController(UsuarioServicio usuarioServicio)
        {
            this.usuarioServicio = usuarioServicio;
        }

        // LOGIN
        [HttpPost("/login")]
        public async Task<ActionResult<UsuarioRespuestaAutenticacionDTO>> Login(UsuarioCredencialesDTO usuarioCredencialesDTO)
        {
            return await usuarioServicio.LoginServicio(usuarioCredencialesDTO);
        }

        // VER USUARIOS
        [HttpGet("/verUsuarios")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<List<UsuarioDTO>>> Get()
        {
            return await usuarioServicio.GetServicio();
        }

        //VER USUARIOS POR ID
        [HttpGet("/verUsuarioPorId/{IdUsuario:int}")]
        public async Task<ActionResult<UsuarioGetPorIdDTO>> Get(int IdUsuario)
        {
            return await usuarioServicio.GetByIdServicio(IdUsuario);
        }

        // CREAR USUARIOS
        [HttpPost("/crearUsuario")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Post([FromForm] UsuarioPostDTO userCreacionDTO)
        {
            return await usuarioServicio.PostServicio(userCreacionDTO);
        }

        // MODIFICAR USUARIOS
        [HttpPut("/modificarUsuario/{IdUsuario:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Put([FromForm] UsuarioPostDTO userCreacionDTO, int IdUsuario)
        {
            return await usuarioServicio.PutServicio(userCreacionDTO, IdUsuario);
        }

        // ELIMINAR USUARIOS
        [HttpDelete("/eliminarUsuario/{IdUsuario:int}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Delete(int IdUsuario)
        {
            return await usuarioServicio.DeleteServicio(IdUsuario);
        }
    }
}
