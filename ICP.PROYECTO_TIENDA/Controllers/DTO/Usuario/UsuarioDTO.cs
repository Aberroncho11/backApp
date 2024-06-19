namespace Icp.TiendaApi.Controllers.DTO.Usuario
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public int Perfil { get; set; }
        public string Email { get; set; }
        public string EstadoUsuario { get; set; }
        public string Nickname { get; set; }
    }
}
