namespace Icp.TiendaApi.BBDD.Entidades
{
    public partial class Usuario
    {
        public Usuario()
        {
            Pedidos = new HashSet<Pedido>();
        }

        public int IdUsuario { get; set; }
        public int Perfil { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }

        public Perfil PerfilNavigation { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
    }
}
