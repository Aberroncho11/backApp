namespace Icp.TiendaApi.BBDD.Entidades
{
    public partial class Perfil
    {
        public Perfil()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int IdPerfil { get; set; }
        public string Tipo { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }
}
