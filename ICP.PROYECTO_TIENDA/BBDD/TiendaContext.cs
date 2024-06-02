using Microsoft.EntityFrameworkCore;
using Icp.TiendaApi.BBDD.Entidades;
namespace Icp.TiendaApi.BBDD
{
    public partial class TiendaContext : DbContext
    {
        public TiendaContext()
        {
        }

        public TiendaContext(DbContextOptions<TiendaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Articulo> Articulos { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<Perfil> Perfiles { get; set; }
        public virtual DbSet<Almacen> Almacen { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=Gabriel;Database=ICP_TIENDA;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Almacen>(entity =>
            {
                entity.HasKey(e => e.IdEstanteria)
                    .HasName("PK__ALMACEN__C408528E64F9575F");

                entity.ToTable("ALMACEN");

                entity.Property(e => e.IdEstanteria).HasColumnName("ID_ESTANTERIA");

                entity.Property(e => e.ArticuloAlmacen).HasColumnName("ARTICULO_ALMACEN");

                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");

                entity.HasOne(d => d.ArticuloAlmacenNavigation)
                    .WithMany(p => p.Almacen)
                    .HasForeignKey(d => d.ArticuloAlmacen)
                    .HasConstraintName("FK__ALMACEN__ARTICUL__7E02B4CC");
            });

            modelBuilder.Entity<Articulo>(entity =>
            {
                entity.HasKey(e => e.IdArticulo)
                    .HasName("PK__ARTICULO__41ADBDE51890E449");

                entity.ToTable("ARTICULOS");

                entity.Property(e => e.IdArticulo).HasColumnName("ID_ARTICULO");

                entity.Property(e => e.Altura).HasColumnName("ALTURA");

                entity.Property(e => e.Ancho).HasColumnName("ANCHO");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.EstadoArticulo)
                    .IsRequired()
                    .HasColumnName("ESTADO_ARTICULO");

                entity.Property(e => e.Fabricante)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("FABRICANTE");

                entity.Property(e => e.Foto).HasColumnName("FOTO");

                entity.Property(e => e.Peso).HasColumnName("PESO");

                entity.Property(e => e.Precio).HasColumnName("PRECIO");
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(e => e.IdPedido)
                    .HasName("PK__PEDIDOS__A05C2F2A45C4D190");

                entity.ToTable("PEDIDOS");

                entity.Property(e => e.IdPedido).HasColumnName("ID_PEDIDO");

                entity.Property(e => e.Ciudad)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CIUDAD");

                entity.Property(e => e.CodigoPostal)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("CODIGO_POSTAL");

                entity.Property(e => e.Contacto)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CONTACTO");

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("DIRECCION");

                entity.Property(e => e.EstadoPedido)
                    .IsRequired()
                    .HasColumnName("ESTADO_PEDIDO");

                entity.Property(e => e.Provincia)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("PROVINCIA");

                entity.Property(e => e.Telefono)
                    .IsRequired()
                    .HasMaxLength(12)
                    .HasColumnName("TELEFONO");

                entity.Property(e => e.UsuarioId).HasColumnName("USUARIO_ID");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PEDIDOS__USUARIO__681373AD");
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.HasKey(e => e.IdPerfil)
                    .HasName("PK__PERFILES__90BDE8092F31C242");

                entity.ToTable("PERFILES");

                entity.Property(e => e.IdPerfil).HasColumnName("ID_PERFIL");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasColumnName("TIPO");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => new { e.PedidoId, e.ArticuloId })
                    .HasName("PK__PRODUCTO__C892A37005F53F3A");

                entity.ToTable("PRODUCTOS");

                entity.Property(e => e.PedidoId).HasColumnName("PEDIDO_ID");

                entity.Property(e => e.ArticuloId).HasColumnName("ARTICULO_ID");

                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");

                entity.HasOne(d => d.Articulo)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.ArticuloId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PRODUCTOS__ARTIC__7849DB76");

                entity.HasOne(d => d.Pedido)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.PedidoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PRODUCTOS__PEDID__7755B73D");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__USUARIOS__91136B90D469B8B0");

                entity.ToTable("USUARIOS");

                entity.HasIndex(e => e.Email, "UQ__USUARIOS__161CF724DAE4B14C")
                    .IsUnique();

                entity.HasIndex(e => e.Nickname, "UQ__USUARIOS__AFFD7B7FD728C7ED")
                    .IsUnique();

                entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Nickname)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("NICKNAME");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.Perfil).HasColumnName("PERFIL");

                entity.HasOne(d => d.PerfilNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.Perfil)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__USUARIOS__PERFIL__6166761E");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
