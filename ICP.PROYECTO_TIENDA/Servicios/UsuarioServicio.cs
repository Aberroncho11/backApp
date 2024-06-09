using AutoMapper;
using Icp.TiendaApi.BBDD;
using Icp.TiendaApi.BBDD.Entidades;
using Icp.TiendaApi.Controllers.DTO.Usuario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Icp.TiendaApi.Servicios
{

    public class UsuarioServicio : ControllerBase
    {
 
        private readonly TiendaContext context;
 
        private IMapper mapper;
        private readonly IConfiguration configuration;

        public UsuarioServicio(TiendaContext context, IMapper mapper, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        /// <summary>
        /// Realiza el inicio de sesión del usuario.
        /// </summary>
        /// <param name="usuarioCredencialesDTO">Las credenciales del usuario.</param>
        /// <returns>La respuesta de autenticación del usuario.</returns>
        public async Task<ActionResult<UsuarioRespuestaAutenticacionDTO>> LoginServicio(UsuarioCredencialesDTO usuarioCredencialesDTO)
        {
            var existeEmail = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == usuarioCredencialesDTO.Email);

            if (existeEmail == null)
            {
                return BadRequest(new { message = $"No existe un usuario con el email {usuarioCredencialesDTO.Email}" });
            }

            var usuarioDB = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == usuarioCredencialesDTO.Email
            && x.Password == usuarioCredencialesDTO.Password);

            if (usuarioDB == null)
            {
                return BadRequest(new { message = "Login incorrecto, el email no coincide con la contraseña puesta o la contraseña es incorrecta" });
            }
            else if (usuarioDB.EstadoUsuario == "Eliminado")
            {
                return BadRequest(new { message = "El usuario con el que se quiere acceder está eliminado" });
            }
            else
            {
                var usuarioDTO = mapper.Map<UsuarioDTO>(usuarioDB);

                return await ConstruirTokenServicio(usuarioDTO);
            }
        }

        /// <summary>
        /// Construye el token de autenticación para el usuario.
        /// </summary>
        /// <param name="usuarioDTO">El DTO del usuario.</param>
        /// <returns>La respuesta de autenticación del usuario.</returns>
        public async Task<ActionResult<UsuarioRespuestaAutenticacionDTO>> ConstruirTokenServicio(UsuarioDTO usuarioDTO)
        {
            
            string rol = "";

            // Definimos el rol según el perfil del usuario
            if (usuarioDTO.Perfil == 1)
            {
                rol = "Administrador";
            }
            else if (usuarioDTO.Perfil == 2)
            {
                rol = "Gestor";
            }
            else if (usuarioDTO.Perfil == 3)
            {
                rol = "Operador";
            }

            // Metemos los datos del usuario en el token
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, rol),
                new Claim(ClaimTypes.UserData, usuarioDTO.IdUsuario.ToString())
            };

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));

            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddHours(1);

            var securityToken = new JwtSecurityToken(claims: claims, expires: expiracion, signingCredentials: creds);

            var respuestaAutenticacionDTO = new UsuarioRespuestaAutenticacionDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),

                Expiracion = TimeZoneInfo.ConvertTimeFromUtc(expiracion, TimeZoneInfo.Local)
            };

            return respuestaAutenticacionDTO;
        }

        /// <summary>
        /// Verifica si existe un usuario con el correo electrónico dado.
        /// </summary>
        /// <param name="Email">El correo electrónico a verificar.</param>
        /// <returns>Un indicador booleano que indica si existe un usuario con el correo electrónico dado.</returns>
        public async Task<ActionResult<bool>> CheckEmailService(string Email)
        {
            var userDB = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == Email);

            if (userDB != null)
            {
                return Ok(true);
            }

            return Ok(false);
        }


        /// <summary>
        /// Verifica si existe un usuario con el nombre de usuario dado.
        /// </summary>
        /// <param name="Nickname">El nombre de usuario a verificar.</param>
        /// <returns>Un indicador booleano que indica si existe un usuario con el nombre de usuario dado.</returns>
        public async Task<ActionResult<bool>> CheckNicknameService(string Nickname)
        {
            var userDB = await context.Usuarios.FirstOrDefaultAsync(x => x.Nickname == Nickname);

            if (userDB != null)
            {
                return Ok(true);
            }

            return Ok(false);
        }


        /// <summary>
        /// Obtiene todos los usuarios.
        /// </summary>
        /// <returns>Una lista de los usuarios</returns>
        public async Task<ActionResult<List<UsuarioDTO>>> GetServicio()
        {
            var usuariosDB = await context.Usuarios.ToListAsync();

            return mapper.Map<List<UsuarioDTO>>(usuariosDB);
        }

        /// <summary>
        /// Obtiene un usuario por su Id.
        /// </summary>
        /// <param name="IdUsuario">El Id del usuario.</param>
        /// <returns>El usuario encontrado.</returns>
        public async Task<ActionResult<UsuarioGetPorIdDTO>> GetByIdServicio(int IdUsuario)
        {
            var usuarioDB = await context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == IdUsuario);

            if (usuarioDB == null)
            {
                return NotFound($"El usuario con el id {IdUsuario} no existe");
            }

            return Ok(mapper.Map<UsuarioGetPorIdDTO>(usuarioDB));
        }


        /// <summary>
        /// Crea un nuevo usuario.
        /// </summary>
        /// <param name="usuarioPostDTO">Los datos del usuario a crear.</param>
        /// <returns>El resultado de la operación.</returns>
        public async Task<ActionResult> PostServicio([FromForm] UsuarioPostDTO usuarioPostDTO)
        {
            var existeNickname = await context.Usuarios.FirstOrDefaultAsync(x => x.Nickname == usuarioPostDTO.Nickname);

            if (existeNickname != null)
            {
                return BadRequest($"Ya existe un usuario con el nombre {usuarioPostDTO.Nickname}");
            }

            var existeEmail = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == usuarioPostDTO.Email);

            if (existeEmail != null)
            {
                return BadRequest($"Ya existe un usuario con el email {usuarioPostDTO.Email}");
            }

            var usuarioDB = mapper.Map<Usuario>(usuarioPostDTO);

            usuarioDB.EstadoUsuario = "Disponible";

            context.Add(usuarioDB);

            await context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Actualiza un usuario existente.
        /// </summary>
        /// <param name="usuarioPostDTO">Los datos del usuario a actualizar.</param>
        /// <param name="IdUsuario">El Id del usuario a actualizar.</param>
        /// <returns>El resultado de la operación.</returns>
        public async Task<ActionResult> PutServicio([FromForm] UsuarioPostDTO usuarioPostDTO, int IdUsuario)
        {
            var usuarioDB = await context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == IdUsuario);

            if (usuarioDB == null)
            {
                return NotFound($"El usuario con el id ${IdUsuario} no existe");
            }

            usuarioDB = mapper.Map(usuarioPostDTO, usuarioDB);

            context.Update(usuarioDB);

            await context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Elimina un usuario existente.
        /// </summary>
        /// <param name="IdUsuario">El Id del usuario a eliminar.</param>
        /// <returns>El resultado de la operación.</returns>
        public async Task<ActionResult> DeleteServicio(int IdUsuario)
        {
            var usuarioDB = await context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == IdUsuario);

            if (usuarioDB == null)
            {
                return NotFound($"El usuario con el id {IdUsuario} no existe");
            }
            else if (usuarioDB.EstadoUsuario == "Eliminado")
            {
                return BadRequest($"El usuario con el id {IdUsuario} ya está eliminado");
            }
            else if(usuarioDB.Perfil == 1)
            {
                return BadRequest($"No se puede eliminar un usuario admnistrador");
            }

            usuarioDB.EstadoUsuario = "Eliminado";

            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
