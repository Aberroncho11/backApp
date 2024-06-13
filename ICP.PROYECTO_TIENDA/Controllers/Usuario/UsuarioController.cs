using Microsoft.AspNetCore.Mvc;
using Icp.TiendaApi.Controllers.DTO.Usuario;
using Icp.TiendaApi.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;


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

        /// <summary>
        /// Método que devuelve una lista de usuarios
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        [HttpGet("/checkEmail/{Email}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<bool>> CheckEmail(string Email)
        {
            return await usuarioServicio.CheckEmailService(Email);
        }

        /// <summary>
        /// Método que devuelve una lista de usuarios
        /// </summary>
        /// <param name="Nickname"></param>
        /// <returns></returns>
        [HttpGet("/checkNickname/{Nickname}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<bool>> CheckNickname(string Nickname)
        {
            return await usuarioServicio.CheckNicknameService(Nickname);
        }


        /// <summary>
        /// Método que devuelve una lista de usuarios
        /// </summary>
        /// <returns></returns>
        [HttpGet("/verUsuarios")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<List<UsuarioDTO>>> GetUsuario()
        {
            return await usuarioServicio.GetServicio();
        }

        /// <summary>
        /// Método que devuelve un usuario por su nickname
        /// </summary>
        /// <param name="Nickname"></param>
        /// <returns></returns>
        [HttpGet("/verUsuarioPorNickname/{Nickname}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuarioPorNickname(string Nickname)
        {
            return await usuarioServicio.GetByNicknameServicio(Nickname);
        }

        /// <summary>
        /// Método que crea un usuario
        /// </summary>
        /// <param name="userCreacionDTO"></param>
        /// <returns></returns>
        [HttpPost("/crearUsuario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> PostUsuario([FromForm] UsuarioPostDTO userCreacionDTO)
        {
            return await usuarioServicio.PostServicio(userCreacionDTO);
        }

        /// <summary>
        /// Método que modifica un usuario
        /// </summary>
        /// <param name="userCreacionDTO"></param>
        /// <param name="Nickname"></param>
        /// <returns></returns>
        [HttpPut("/modificarUsuario/{Nickname}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> PutUsuario([FromForm] UsuarioPostDTO userCreacionDTO, string Nickname)
        {
            return await usuarioServicio.PutServicio(userCreacionDTO, Nickname);
        }

        /// <summary>
        /// Método que elimina un usuario
        /// </summary>
        /// <param name="Nickname"></param>
        /// <returns></returns>
        [HttpDelete("/eliminarUsuario/{IdUsuario:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> DeleteUsuario(string Nickname)
        {
            return await usuarioServicio.DeleteServicio(Nickname);
        }
    }
}
