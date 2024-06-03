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

        // LOGIN
        public async Task<ActionResult<UsuarioRespuestaAutenticacionDTO>> LoginServicio(UsuarioCredencialesDTO usuarioCredencialesDTO)
        {
            var usuarioDB = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == usuarioCredencialesDTO.Email
            && x.Password == usuarioCredencialesDTO.Password);

            var usuarioDTO = mapper.Map<UsuarioDTO>(usuarioDB);

            if (usuarioDB == null)
            {
                return BadRequest("Login incorrecto");
            }
            else if(usuarioDB.EstadoUsuario == "Eliminado"){

                return BadRequest("El usuario con el que se quiere acceder está eliminado");
            }
            else
            {
                return await ConstruirTokenServicio(usuarioDTO);
            }
        }

        // CONSTRUIR TOKEN
        public async Task<ActionResult<UsuarioRespuestaAutenticacionDTO>> ConstruirTokenServicio(UsuarioDTO usuarioDTO)
        {
            string rol = "";

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

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, usuarioDTO.Email),
                new Claim(ClaimTypes.Role, rol)
            };

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));

            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            var securityToken = new JwtSecurityToken(claims: claims, expires: expiracion, signingCredentials: creds);

            var respuestaAutenticacionDTO = new UsuarioRespuestaAutenticacionDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = TimeZoneInfo.ConvertTimeFromUtc(expiracion, TimeZoneInfo.Local)
            };

            return respuestaAutenticacionDTO;
        }

        // CHECK TOKEN
        public ActionResult<bool> CheckToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["llavejwt"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);

                return Ok(true);
            }
            catch
            {
                return Unauthorized();
            }
        }


        // VER USUARIOS
        public async Task<ActionResult<List<UsuarioDTO>>> GetServicio()
        {
            var usuariosDB = await context.Usuarios.ToListAsync();

            return mapper.Map<List<UsuarioDTO>>(usuariosDB);
        }

        //VER USUARIOS POR ID
        public async Task<ActionResult<UsuarioGetPorIdDTO>> GetByIdServicio(int IdUsuario)
        {
            var usuarioDB = await context.Usuarios
                .FirstOrDefaultAsync(x => x.IdUsuario == IdUsuario);

            if (usuarioDB == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<UsuarioGetPorIdDTO>(usuarioDB));
        }


        // CREAR USUARIOS
        public async Task<ActionResult> PostServicio([FromForm] UsuarioPostDTO userCreacionDTO)
        {
            var existeNickname = await context.Usuarios.Where(x => x.Nickname == userCreacionDTO.Nickname).ToListAsync();

            if (existeNickname == null)
            {
                return BadRequest($"Ya existe un usuario con el nombre {userCreacionDTO.Nickname}");
            }

            var existeEmail = await context.Usuarios.Where(x => x.Email == userCreacionDTO.Email).ToListAsync();

            if (existeEmail == null)
            {
                return BadRequest($"Ya existe un usuario con el email {userCreacionDTO.Email}");
            }

            var usuarioDB = mapper.Map<Usuario>(userCreacionDTO);

            usuarioDB.EstadoUsuario = "Disponible";

            context.Add(usuarioDB);

            await context.SaveChangesAsync();

            return Ok();
        }

        // MODIFICAR USUARIOS
        public async Task<ActionResult> PutServicio([FromForm] UsuarioPostDTO usuarioPostDTO, int IdUsuario)
        {
            var usuarioDB = await context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == IdUsuario);

            if (usuarioDB == null)
            {
                return NotFound($"No existe un usuario con el id ${IdUsuario}");
            }

            usuarioDB = mapper.Map(usuarioPostDTO, usuarioDB);

            context.Update(usuarioDB);

            await context.SaveChangesAsync();

            return Ok();
        }

        // ELIMINAR USUARIOS
        public async Task<ActionResult> DeleteServicio(int IdUsuario)
        {
            var usuarioDB = await context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == IdUsuario);

            if (usuarioDB == null)
            {
                return NotFound($"El usuario con el id {IdUsuario} no existe");
            }
            if(usuarioDB.EstadoUsuario == "Eliminado")
            {
                return BadRequest($"El usuario con el id {IdUsuario} ya está eliminado");
            }

            usuarioDB.EstadoUsuario = "Eliminado";

            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
