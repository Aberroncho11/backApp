using Microsoft.EntityFrameworkCore;
using Icp.TiendaApi.BBDD.Modelos;
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

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=ps17;Database=FCT_ABR_10;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(e => e.IdArticle)
                    .HasName("PK__ARTICLES__05362DC3CD2EC8D1");

                entity.ToTable("ARTICLES");

                entity.Property(e => e.IdArticle).HasColumnName("ID_ARTICLE");

                entity.Property(e => e.Description)
                    .HasMaxLength(150)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.Maker)
                    .HasMaxLength(20)
                    .HasColumnName("MAKER");

                entity.Property(e => e.Weight).HasColumnName("WEIGHT");

                entity.Property(e => e.Height).HasColumnName("HEIGHT");

                entity.Property(e => e.Width).HasColumnName("WIDTH");

                entity.Property(e => e.Price).HasColumnName("PRICE");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("('DISPONIBLE')");

                entity.Property(e => e.Foto)
                     .HasMaxLength(100)
                     .HasColumnName("FOTO");

            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ArticleId })
                    .HasName("PK__ITEMS__10D46BA7F20CA215");

                entity.ToTable("ITEMS");

                entity.Property(e => e.OrderId).HasColumnName("ORDER_ID");

                entity.Property(e => e.ArticleId).HasColumnName("ARTICLE_ID");

                entity.Property(e => e.Quantity).HasColumnName("QUANTITY");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.ArticleId)
                    .HasConstraintName("FK__ITEMS__ARTICLE_I__04A590BA");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__ITEMS__ORDER_ID__03B16C81");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.IdOrder)
                    .HasName("PK__ORDERS__D23A85657563F656");

                entity.ToTable("ORDERS");

                entity.Property(e => e.IdOrder).HasColumnName("ID_ORDER");

                entity.Property(e => e.IdUser).HasColumnName("ID_USER");

                entity.Property(e => e.PostalCode).HasColumnName("POSTAL_CODE");

                entity.Property(e => e.Town)
                     .HasMaxLength(20)
                     .HasColumnName("TOWN");

                entity.Property(e => e.PhoneNumber).HasColumnName("PHONE_NUMBER");

                entity.Property(e => e.PersonalContact)
                  .HasMaxLength(20)
                  .HasColumnName("PERSONAL_CONTACT");

                entity.Property(e => e.Address)
                   .HasMaxLength(40)
                   .HasColumnName("ADDRESS");

                entity.Property(e => e.Province)
                    .HasMaxLength(20)
                    .HasColumnName("PROVINCE");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("('LISTO PARA ENVIAR')");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__ORDERS__ID_USER__7C104AB9");
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasKey(e => e.IdProfile)
                    .HasName("PK__PROFILES__0DB303E0877480A7");

                entity.ToTable("PROFILES");

                entity.Property(e => e.IdProfile).HasColumnName("ID_PROFILE");

                entity.Property(e => e.Type).HasColumnName("TYPE");
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => e.IdShelf)
                    .HasName("PK__STOCK__E3F9DB5538C43B33");

                entity.ToTable("STOCK");

                entity.Property(e => e.IdShelf).HasColumnName("ID_SHELF");

                entity.Property(e => e.Amount).HasColumnName("AMOUNT");

                entity.Property(e => e.IdArticle).HasColumnName("ID_ARTICLE");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.IdArticle)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__STOCK__ID_ARTICL__0781FD65");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("PK__USERS__95F484405E9E5FF9");

                entity.ToTable("USERS");

                entity.HasIndex(e => e.Email, "UQ__USERS__161CF7248AC2806E")
                    .IsUnique();

                entity.HasIndex(e => e.Nickname, "UQ__USERS__AFFD7B7FFCD3F293")
                    .IsUnique();

                entity.Property(e => e.IdUser).HasColumnName("ID_USER");

                entity.Property(e => e.IdProfile).HasColumnName("ID_PROFILE");

                entity.Property(e => e.Password)
                    .HasMaxLength(20)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.Email)
                    .HasMaxLength(30)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Nickname)
                    .HasMaxLength(20)
                    .HasColumnName("NICKNAME");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdProfile)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__USERS__ID_PROFIL__6DC22B62");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
