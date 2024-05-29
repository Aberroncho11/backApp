using AutoMapper;
using Icp.TiendaApi.BBDD;
using Icp.TiendaApi.BBDD.Modelos;
using Icp.TiendaApi.Controllers.DTO.Article;
using Icp.TiendaApi.Controllers.DTO.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;


namespace Icp.TiendaApi.Servicios
{
    public class UserService : ControllerBase
    {
        private readonly TiendaContext context;
        private IMapper mapper;
        private readonly IConfiguration configuration;

        public UserService(TiendaContext context, IMapper mapper, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;

        }

        // LOGIN
        public async Task<ActionResult<UserRespuestaAutenticacionDTO>> LoginService(UserCredencialesDTO userCredencialesDTO)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == userCredencialesDTO.Email
            && x.Password == userCredencialesDTO.Password);

            var userDTO = mapper.Map<UserDTO>(user);

            if (user != null)
            {
                return await ConstruirTokenService(userDTO);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }
        }

        // CONSTRUIR TOKEN
        public async Task<ActionResult<UserRespuestaAutenticacionDTO>> ConstruirTokenService(UserDTO userDTO)
        {
            string rol = "";

            if (userDTO.IdProfile == 1)
            {
                rol = "Administrador";
            }
            else if (userDTO.IdProfile == 2)
            {
                rol = "Gestor";
            }
            else if (userDTO.IdProfile == 3)
            {
                rol = "Operador";
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userDTO.Email),
                new Claim(ClaimTypes.Role, rol)
            };

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));

            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            var securityToken = new JwtSecurityToken(claims: claims, expires: expiracion, signingCredentials: creds);

            var respuestaAutenticacionDTO = new UserRespuestaAutenticacionDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion
            };

            return respuestaAutenticacionDTO;
        }

        // VER USUARIOS
        public async Task<ActionResult<List<UserDTO>>> GetService()
        {
            var users = await context.Users.ToListAsync();

            return mapper.Map<List<UserDTO>>(users);
        }

        //VER USUARIOS POR ID
        public async Task<ActionResult<UserGetPorIdDTO>> GetByIdService(int IdUser)
        {
            var usuario = await context.Users
                .FirstOrDefaultAsync(x => x.IdUser == IdUser);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<UserGetPorIdDTO>(usuario));
        }


        // CREAR USUARIOS
        public async Task<ActionResult> PostService([FromForm] UserCreacionDTO userCreacionDTO)
        {
            var existeNickname = await context.Users.Where(x => x.Nickname == userCreacionDTO.Nickname).ToListAsync();

            if (existeNickname == null)
            {
                return BadRequest($"Ya existe un usuario con el nombre {userCreacionDTO.Nickname}");
            }

            var existeEmail = await context.Users.Where(x => x.Email == userCreacionDTO.Email).ToListAsync();

            if (existeEmail == null)
            {
                return BadRequest($"Ya existe un usuario con el email {userCreacionDTO.Email}");
            }

            var user = mapper.Map<User>(userCreacionDTO);

            context.Add(user);

            await context.SaveChangesAsync();

            return Ok();
        }

        // MODIFICAR USUARIOS
        public async Task<ActionResult> PutService([FromForm] UserCreacionDTO userCreacionDTO, int IdUser)
        {
            var existe = await context.Users.AnyAsync(x => x.IdUser == IdUser);

            if (!existe)
            {
                return NotFound();
            }

            var user = mapper.Map<User>(userCreacionDTO);

            user.IdUser = IdUser;

            context.Update(user);

            await context.SaveChangesAsync();

            return Ok();
        }

        // ELIMINAR USUARIOS
        public async Task<ActionResult> DeleteService(int IdUser)
        {
            var existe = await context.Users.AnyAsync(x => x.IdUser == IdUser);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new User() { IdUser = IdUser });

            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
