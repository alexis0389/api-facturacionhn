using Facturacion.Feature.Clientes;
using Facturacion.Feature.Facturas;
using Facturacion.Feature.Movimientos;
using Facturacion.Feature.Productos;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Infraestructure
{
    public class FacturacionDbContext : DbContext
    {
        public FacturacionDbContext(DbContextOptions<FacturacionDbContext> options) : base(options)
        {

        }
        public DbSet<Cliente> Clientes { get; set; } = null!;
        public DbSet<Producto> Productos { get; set; } = null!;
        public DbSet<Factura> Facturas { get; set; } = null!;
        public DbSet<FacturaDetalle> FacturasDetalle { get; set; } = null!;
        public DbSet<Movimiento> Movimientos { get; set; } = null!;
        public DbSet<TipoMovimiento> Tipos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity => {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).HasColumnType("int");

                entity.Property(c => c.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
                
                entity.Property(c => c.Direccion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(c => c.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Producto>(entity => {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id).HasColumnType("int");

                entity.Property(p => p.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(p => p.Categoria)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(p => p.PrecioUnitario)
                    .IsRequired()
                    .HasColumnName("Precio")
                    .HasColumnType("int");

                entity.Property(p => p.Existencia)
                    .IsRequired()
                    .HasColumnType("int");
            });

            modelBuilder.Entity<Factura>(entity => {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.Id).HasColumnType("int");

                entity.HasOne<Cliente>(c => c.Cliente)
                    .WithMany(f => f.Facturas)
                    .HasForeignKey(f => f.ClienteId);

                entity.Property(f => f.Fecha)
                    .IsRequired()
                    .HasColumnType("Date")
                    .HasDefaultValueSql("GetDate()");

                entity.Property(f => f.Descuento)
                    .HasDefaultValue(0.00)
                    .HasColumnType("decimal(10,2)");

                entity.Property(f => f.IVA)
                    .HasDefaultValue(0.00)
                    .HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<FacturaDetalle>(entity => {
                entity.ToTable("Facturas_Detalle");

                entity.HasKey(df => df.Id);

                entity.Property(df => df.Id)
                    .HasColumnType("int");

                entity.HasOne<Factura>(f => f.Factura)
                    .WithMany(df => df.FacturaDetalle)
                    .HasForeignKey(df => df.FacturaId);

                entity.HasOne<Producto>(p => p.Producto)
                    .WithMany(df => df.FacturaDetalle)
                    .HasForeignKey(df => df.ProductoId);

                entity.Property(fd => fd.PrecioUnitario)
                    .IsRequired()
                    .HasColumnType("int")
                    .HasColumnName("Precio")
                    .HasDefaultValue(0);

                entity.Property(fd => fd.Cantidad)
                    .IsRequired()
                    .HasColumnType("int")
                    .HasDefaultValue(0);
            });

            modelBuilder.Entity<Movimiento>(entity => {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Id).HasColumnType("int");

                entity.Property(m => m.Fecha)
                    .HasColumnType("Date")
                    .HasDefaultValueSql("GetDate()")
                    .IsRequired();

                entity.Property(m => m.Hora)
                    .HasColumnType("DateTime")
                    .HasDefaultValueSql("GetDate()")
                    .IsRequired();

                entity.HasOne<Producto>(p => p.Producto)
                    .WithMany(m => m.Movimiento)
                    .HasForeignKey(m => m.ProductoId);

                entity.Property(m => m.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(m => m.Tipo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(m => m.Cantidad)
                    .HasColumnType("int")
                    .HasDefaultValue(0)
                    .IsRequired();

                entity.Property(m => m.Total)
                    .HasColumnType("int")
                    .HasDefaultValue(0)
                    .IsRequired();
            });

            modelBuilder.Entity<TipoMovimiento>(entity => {
                entity.ToTable("Movimiento_Tipos");

                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).HasColumnType("int");

                entity.HasOne<Movimiento>(m => m.Movimiento)
                    .WithMany(t => t.TipoMovimiento)
                    .HasForeignKey(t => t.MovimientoId);

                entity.Property(t => t.Entrada_Salida)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();
            });
        }
    }
}