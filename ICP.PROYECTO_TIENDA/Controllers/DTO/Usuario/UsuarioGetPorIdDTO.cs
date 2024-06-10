namespace Icp.TiendaApi.Controllers.DTO.Usuario
{
    public class UsuarioGetPorNicknameDTO
    {
        public int Perfil { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string EstadoUsuario { get; set; }
        public string Nickname { get; set; }
    }
}
