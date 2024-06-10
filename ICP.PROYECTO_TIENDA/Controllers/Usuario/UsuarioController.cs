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

        /// <summary>
        /// Realiza el inicio de sesión del usuario.
        /// </summary>
        /// <param name="usuarioCredencialesDTO"></param>
        /// <returns>La respuesta de autenticación del usuario.</returns>
        [HttpPost("/login")]
        public async Task<ActionResult<UsuarioRespuestaAutenticacionDTO>> Login(UsuarioCredencialesDTO usuarioCredencialesDTO)
        {
            return await usuarioServicio.LoginServicio(usuarioCredencialesDTO);
        }

        [HttpGet("/checkEmail/{Email}")]
        public async Task<ActionResult<bool>> CheckEmail(string Email)
        {
            return await usuarioServicio.CheckEmailService(Email);
        }

        [HttpGet("/checkNickname/{Nickname}")]
        public async Task<ActionResult<bool>> CheckNickname(string Nickname)
        {
            return await usuarioServicio.CheckNicknameService(Nickname);
        }


        // VER USUARIOS
        [HttpGet("/verUsuarios")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<List<UsuarioDTO>>> Get()
        {
            return await usuarioServicio.GetServicio();
        }

        //VER USUARIOS POR Nickname
        [HttpGet("/verUsuarioPorNickname/{Nickname}")]
        public async Task<ActionResult<UsuarioGetPorNicknameDTO>> Get(string Nickname)
        {
            return await usuarioServicio.GetByNicknameServicio(Nickname);
        }

        // CREAR USUARIOS
        [HttpPost("/crearUsuario")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Post([FromForm] UsuarioPostDTO userCreacionDTO)
        {
            return await usuarioServicio.PostServicio(userCreacionDTO);
        }

        // MODIFICAR USUARIOS
        [HttpPut("/modificarUsuario/{Nickname}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Put([FromForm] UsuarioPostDTO userCreacionDTO, string Nickname)
        {
            return await usuarioServicio.PutServicio(userCreacionDTO, Nickname);
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
