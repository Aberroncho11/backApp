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
    // Servicio de usuario
    public class UsuarioServicio : ControllerBase
    {
        // Definir el contexto
        private readonly TiendaContext context;
        // Definir el mapeador
        private IMapper mapper;
        // Definir la configuración
        private readonly IConfiguration configuration;

        // Constructor de la clase UsuarioServicio
        public UsuarioServicio(TiendaContext context, IMapper mapper, IConfiguration configuration)
        {
            // Inicializar el contexto
            this.context = context;
            // Inicializar el mapeador
            this.mapper = mapper;
            // Inicializar la configuración
            this.configuration = configuration;
        }

        /// <summary>
        /// Realiza el inicio de sesión del usuario.
        /// </summary>
        /// <param name="usuarioCredencialesDTO">Las credenciales del usuario.</param>
        /// <returns>La respuesta de autenticación del usuario.</returns>
        public async Task<ActionResult<UsuarioRespuestaAutenticacionDTO>> LoginServicio(UsuarioCredencialesDTO usuarioCredencialesDTO)
        {
            // Verificar si el email ya existe
            var existeEmail = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == usuarioCredencialesDTO.Email);

            // Verificar si el email ya existe
            if (existeEmail == null)
            {
                // Si ya existe, retornar error
                return BadRequest($"No existe un usuario con el email {usuarioCredencialesDTO.Email}");
            }

            // Buscar usuario en la base de datos
            var usuarioDB = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == usuarioCredencialesDTO.Email
            && x.Password == usuarioCredencialesDTO.Password);
            
            // Verificar si el usuario existe
            if (usuarioDB == null)
            {
                // Si no existe, retornar error
                return BadRequest("Login incorrecto el email no coincide con la contraseña puesta o la contraseña es incorrecta");
            }
            // Verificar si el usuario está eliminado
            else if (usuarioDB.EstadoUsuario == "Eliminado")
            {
                // Si está eliminado, retornar error
                return BadRequest("El usuario con el que se quiere acceder está eliminado");
            }
            else
            {
                // Mapear usuario a DTO
                var usuarioDTO = mapper.Map<UsuarioDTO>(usuarioDB);

                // Si el usuario existe y no está eliminado, construir token
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
            // Definir el rol del usuario
            string rol = "";
            // Si el usuario es administrador
            if (usuarioDTO.Perfil == 1)
            {
                rol = "Administrador";
            }
            // Si el usuario es gestor
            else if (usuarioDTO.Perfil == 2)
            {
                rol = "Gestor";
            }
            // Si el usuario es operador
            else if (usuarioDTO.Perfil == 3)
            {
                rol = "Operador";
            }

            // Definir las claims del usuario
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, rol)
            };

            // Definir la llave de seguridad
            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));

            // Definir las credenciales de seguridad
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            // Definir la expiración del token
            var expiracion = DateTime.UtcNow.AddYears(1);

            // Definir el token
            var securityToken = new JwtSecurityToken(claims: claims, expires: expiracion, signingCredentials: creds);

            // Definir la respuesta de autenticación
            var respuestaAutenticacionDTO = new UsuarioRespuestaAutenticacionDTO()
            {
                // Definir el token
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),

                // Definir la expiración
                Expiracion = TimeZoneInfo.ConvertTimeFromUtc(expiracion, TimeZoneInfo.Local)
            };

            return respuestaAutenticacionDTO;
        }


        /// <summary>
        /// Obtiene todos los usuarios.
        /// </summary>
        /// <returns>Una lista de los usuarios</returns>
        public async Task<ActionResult<List<UsuarioDTO>>> GetServicio()
        {
            // Buscar usuarios en la base de datos
            var usuariosDB = await context.Usuarios.ToListAsync();

            // Mapear usuarios a DTO
            return mapper.Map<List<UsuarioDTO>>(usuariosDB);
        }

        /// <summary>
        /// Obtiene un usuario por su Id.
        /// </summary>
        /// <param name="IdUsuario">El Id del usuario.</param>
        /// <returns>El usuario encontrado.</returns>
        public async Task<ActionResult<UsuarioGetPorIdDTO>> GetByIdServicio(int IdUsuario)
        {
            // Buscar usuario en la base de datos
            var usuarioDB = await context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == IdUsuario);

            // Verificar si el usuario existe
            if (usuarioDB == null)
            {
                // Si no existe, retornar error
                return NotFound($"El usuario con el id {IdUsuario} no existe");
            }

            // Mapear usuario a DTO
            return Ok(mapper.Map<UsuarioGetPorIdDTO>(usuarioDB));
        }


        /// <summary>
        /// Crea un nuevo usuario.
        /// </summary>
        /// <param name="usuarioPostDTO">Los datos del usuario a crear.</param>
        /// <returns>El resultado de la operación.</returns>
        public async Task<ActionResult> PostServicio([FromForm] UsuarioPostDTO usuarioPostDTO)
        {
            // Verificar si el nickname ya existe
            var existeNickname = await context.Usuarios.FirstOrDefaultAsync(x => x.Nickname == usuarioPostDTO.Nickname);

            // Verificar si el nickname ya existe
            if (existeNickname != null)
            {
                // Si ya existe, retornar error
                return BadRequest($"Ya existe un usuario con el nombre {usuarioPostDTO.Nickname}");
            }

            // Verificar si el email ya existe
            var existeEmail = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == usuarioPostDTO.Email);

            // Verificar si el email ya existe
            if (existeEmail != null)
            {
                // Si ya existe, retornar error
                return BadRequest($"Ya existe un usuario con el email {usuarioPostDTO.Email}");
            }

            // Mapear DTO a entidad
            var usuarioDB = mapper.Map<Usuario>(usuarioPostDTO);

            // Definir el estado del usuario
            usuarioDB.EstadoUsuario = "Disponible";

            // Agregar usuario a la base de datos
            context.Add(usuarioDB);

            // Guardar cambios
            await context.SaveChangesAsync();

            // Retornar Ok
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
            // Buscar el usuario en la base de datos
            var usuarioDB = await context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == IdUsuario);

            // Verificar si el usuario existe
            if (usuarioDB == null)
            {
                // Si no existe, retornar error
                return NotFound($"El usuario con el id ${IdUsuario} no existe");
            }

            // Mapear los datos del DTO al usuario existente
            usuarioDB = mapper.Map(usuarioPostDTO, usuarioDB);

            // Actualizar el usuario en la base de datos
            context.Update(usuarioDB);

            // Guardar los cambios
            await context.SaveChangesAsync();

            // Retornar Ok
            return Ok();
        }

        /// <summary>
        /// Elimina un usuario existente.
        /// </summary>
        /// <param name="IdUsuario">El Id del usuario a eliminar.</param>
        /// <returns>El resultado de la operación.</returns>
        public async Task<ActionResult> DeleteServicio(int IdUsuario)
        {
            // Buscar el usuario en la base de datos
            var usuarioDB = await context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == IdUsuario);

            // Verificar si el usuario existe
            if (usuarioDB == null)
            {
                // Si no existe, retornar error
                return NotFound($"El usuario con el id {IdUsuario} no existe");
            }
            // Verificar si el usuario ya está eliminado
            if (usuarioDB.EstadoUsuario == "Eliminado")
            {
                // Si ya está eliminado, retornar error
                return BadRequest($"El usuario con el id {IdUsuario} ya está eliminado");
            }

            // Cambiar el estado del usuario a "Eliminado"
            usuarioDB.EstadoUsuario = "Eliminado";

            // Guardar los cambios en la base de datos
            await context.SaveChangesAsync();

            // Retornar Ok
            return Ok();
        }
    }
}
