namespace Icp.TiendaApi.Controllers.DTO.User
{
    public class UserDTO
    {
        public int IdUser { get; set; }
        public int IdProfile { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
    }
}
