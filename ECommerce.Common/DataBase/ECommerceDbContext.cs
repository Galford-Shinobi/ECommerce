using ECommerce.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Common.DataBase
{
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext()
        {}
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options)
            : base(options)
        { }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<Barra> Barras { get; set; }
        public virtual DbSet<Bodega> Bodegas { get; set; }
        public virtual DbSet<BodegaProducto> BodegaProductos { get; set; }
        public virtual DbSet<Concepto> Conceptos { get; set; }
        public virtual DbSet<Departamento> Departamentos { get; set; }
        public virtual DbSet<Genero> Generos { get; set; }
        public virtual DbSet<HistorialRefreshToken> HistorialRefreshTokens { get; set; }
        public virtual DbSet<Iva> Ivas { get; set; }
        public virtual DbSet<Medidum> Medida { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Proveedor> Proveedors { get; set; }
        public virtual DbSet<RolMenu> RolMenus { get; set; }
        public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasKey(e => e.RolId)
                    .HasName("PK__AspNetRo__F92302F1EC38ACED");

                entity.HasIndex(e => e.Rnombre, "UQ__AspNetRo__67C54CA9104E3E19")
                    .IsUnique();

                entity.Property(e => e.RolId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.NormalizedName)
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.Rnombre)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("RNombre");
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__AspNetUs__1788CC4C1F8423BB");

                entity.HasIndex(e => e.NickName, "UQ__AspNetUs__01E67C8BD6B50815")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__AspNetUs__A9D10534596BD761")
                    .IsUnique();

                entity.HasIndex(e => e.Dni, "UQ__AspNetUs__C035B8DD12A6418D")
                    .IsUnique();

                entity.HasIndex(e => e.UserName, "UQ__AspNetUs__C9F284569098A82D")
                    .IsUnique();

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Age)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Dni)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("DNI");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(175)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.LastAccessedDate).HasColumnType("datetime");

                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");

                entity.Property(e => e.NickName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.NormalizedEmail)
                    .HasMaxLength(175)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.PasswordSalt).IsRequired();

                entity.Property(e => e.PicturePath).IsUnicode(false);

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.SecondSurName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.SurName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.UserTimeZone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Genero");
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => e.UserRolId)
                    .HasName("PK__AspNetUs__80906A4CF0DD0769");

                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AspNetRoles");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AspNetUsers");
            });

            modelBuilder.Entity<Barra>(entity =>
            {
                entity.HasKey(e => new { e.BarraId, e.Idproducto })
                    .HasName("PK__Barra__6FA65B67854875FD");

                entity.ToTable("Barra");

                entity.HasIndex(e => e.Barcode, "UQ__Barra__177800D3E0E3E070")
                    .IsUnique();

                entity.Property(e => e.BarraId).ValueGeneratedOnAdd();

                entity.Property(e => e.Idproducto).HasColumnName("IDProducto");

                entity.Property(e => e.Barcode)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdproductoNavigation)
                    .WithMany(p => p.Barras)
                    .HasForeignKey(d => d.Idproducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Barra_Producto");
            });

            modelBuilder.Entity<Bodega>(entity =>
            {
                entity.ToTable("Bodega");

                entity.HasIndex(e => e.Descripcion, "UQ__Bodega__92C53B6CB960531B")
                    .IsUnique();

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(175)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<BodegaProducto>(entity =>
            {
                entity.HasKey(e => new { e.Idproducto, e.BodegaId })
                    .HasName("PK__BodegaPr__E8A0DB1DFCF45016");

                entity.ToTable("BodegaProducto");

                entity.Property(e => e.Idproducto).HasColumnName("IDProducto");

                entity.Property(e => e.CantidadMinima).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Maximo).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Minimo).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Stock).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Bodega)
                    .WithMany(p => p.BodegaProductos)
                    .HasForeignKey(d => d.BodegaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BodegaProducto_Bodega");

                entity.HasOne(d => d.IdproductoNavigation)
                    .WithMany(p => p.BodegaProductos)
                    .HasForeignKey(d => d.Idproducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BodegaProducto_Producto");
            });

            modelBuilder.Entity<Concepto>(entity =>
            {
                entity.ToTable("Concepto");

                entity.HasIndex(e => e.Descripcion, "UQ__Concepto__92C53B6C9A7F70B3")
                    .IsUnique();

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Departamento>(entity =>
            {
                entity.ToTable("Departamento");

                entity.HasIndex(e => e.Descripcion, "UQ__Departam__92C53B6C3267FCB5")
                    .IsUnique();

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Genero>(entity =>
            {
                entity.HasKey(e => e.GenderId)
                    .HasName("PK__Genero__4E24E9F749F2F08F");

                entity.ToTable("Genero");

                entity.HasIndex(e => e.GeneroName, "UQ__Genero__8BC59BC0D50AEF22")
                    .IsUnique();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(65)
                    .IsUnicode(false);

                entity.Property(e => e.GeneroName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<HistorialRefreshToken>(entity =>
            {
                entity.HasKey(e => e.IdHistorialToken)
                    .HasName("PK__Historia__03DC48A57E256263");

                entity.ToTable("HistorialRefreshToken");

                entity.Property(e => e.EsActivo).HasComputedColumnSql("(case when [FechaExpiracion]<getdate() then CONVERT([bit],(0)) else CONVERT([bit],(1)) end)", false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaExpiracion).HasColumnType("datetime");

                entity.Property(e => e.RefreshToken)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Token)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HistorialRefreshTokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Historial__UserI__71D1E811");
            });

            modelBuilder.Entity<Iva>(entity =>
            {
                entity.ToTable("IVA");

                entity.HasIndex(e => e.Descripcion, "UQ__IVA__92C53B6C222B84C0")
                    .IsUnique();

                entity.Property(e => e.Ivaid).HasColumnName("IVAId");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(175)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Tarifa).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Medidum>(entity =>
            {
                entity.HasKey(e => e.MedidaId)
                    .HasName("PK__Medida__5F7A0C027DF5A235");

                entity.HasIndex(e => e.Descripcion, "UQ__Medida__92C53B6CF851FD34")
                    .IsUnique();

                entity.HasIndex(e => e.Escala, "UQ__Medida__9B63C44766751796")
                    .IsUnique();

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(175)
                    .IsUnicode(false);

                entity.Property(e => e.Escala)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => e.IdMenu)
                    .HasName("PK__Menu__C26AF483A9F8F8E2");

                entity.ToTable("Menu");

                entity.Property(e => e.IdMenu).HasColumnName("idMenu");

                entity.Property(e => e.Controlador)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("controlador");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.EsActivo).HasColumnName("esActivo");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Icono)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("icono");

                entity.Property(e => e.IdMenuPadre).HasColumnName("idMenuPadre");

                entity.Property(e => e.PaginaAccion)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("paginaAccion");

                entity.HasOne(d => d.IdMenuPadreNavigation)
                    .WithMany(p => p.InverseIdMenuPadreNavigation)
                    .HasForeignKey(d => d.IdMenuPadre)
                    .HasConstraintName("FK__Menu__idMenuPadr__693CA210");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.IdProducto);

                entity.ToTable("Producto");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Ivaid).HasColumnName("IVAId");

                entity.Property(e => e.Medida).HasDefaultValueSql("((1))");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Notas).IsUnicode(false);

                entity.Property(e => e.PathImagen).IsUnicode(false);

                entity.Property(e => e.Pieza).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Precio).HasColumnType("money");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Departamento)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.DepartamentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Producto_Departamento");

                entity.HasOne(d => d.Iva)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.Ivaid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Producto_IVA");

                entity.HasOne(d => d.MedidaNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.MedidaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Producto_Medida");
            });

            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.HasKey(e => e.IDProveedor);

                entity.ToTable("Proveedor");

                entity.HasIndex(e => e.Correo, "UQ__Proveedo__60695A193708DD75")
                    .IsUnique();

                entity.Property(e => e.IDProveedor).HasColumnName("IDProveedor");

                entity.Property(e => e.ApellidosContacto)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Correo)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion).IsUnicode(false);

                entity.Property(e => e.Documento)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.NombresContacto)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Notas).HasColumnType("text");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Telefono1)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono2)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.TipoDocumento)
                    .WithMany(p => p.Proveedors)
                    .HasForeignKey(d => d.TipoDocumentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Proveedor_TipoDocumento");
            });

            modelBuilder.Entity<RolMenu>(entity =>
            {
                entity.HasKey(e => e.IdRolMenu)
                    .HasName("PK__RolMenu__CD2045D83018FBE0");

                entity.ToTable("RolMenu");

                entity.Property(e => e.IdRolMenu).HasColumnName("idRolMenu");

                entity.Property(e => e.EsActivo).HasColumnName("esActivo");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdMenu).HasColumnName("idMenu");

                entity.HasOne(d => d.IdMenuNavigation)
                    .WithMany(p => p.RolMenus)
                    .HasForeignKey(d => d.IdMenu)
                    .HasConstraintName("FK__RolMenu__idMenu__6E01572D");

                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.RolMenus)
                    .HasForeignKey(d => d.RolId)
                    .HasConstraintName("FK__RolMenu__RolId__6D0D32F4");
            });

            modelBuilder.Entity<TipoDocumento>(entity =>
            {
                entity.ToTable("TipoDocumento");

                entity.HasIndex(e => e.Descripcion, "UQ__TipoDocu__92C53B6C09C94391")
                    .IsUnique();

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
            });
        }
    }
}
