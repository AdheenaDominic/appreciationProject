using Microsoft.EntityFrameworkCore;

namespace AppreciationCards.Models
{
    public partial class AppreciationContext : DbContext
    {
        public AppreciationContext()
        {
        }

        public AppreciationContext(DbContextOptions<AppreciationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<XeroValues> XeroValues { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Appreciation;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Messages>(entity =>
            {
                entity.HasKey(e => e.MessageId);

                entity.Property(e => e.MessageId).HasColumnName("message_id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasMaxLength(2000);

                entity.Property(e => e.FromName)
                    .HasColumnName("from_name")
                    .HasMaxLength(100);

                entity.Property(e => e.MessageDate)
                    .HasColumnName("message_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ToName)
                    .IsRequired()
                    .HasColumnName("to_name")
                    .HasMaxLength(100);

                entity.Property(e => e.ValueId).HasColumnName("value_id");

                entity.Property(d => d.Value)
                   .HasColumnName("value")
                   .HasMaxLength(100);

            });

            modelBuilder.Entity<XeroValues>(entity =>
            {
                entity.HasKey(e => e.ValueId);

                entity.ToTable("Xero_Values");

                entity.Property(e => e.ValueId).HasColumnName("value_id");

                entity.Property(e => e.ValueName)
                    .IsRequired()
                    .HasColumnName("value_name")
                    .HasMaxLength(100);
            });
        }
    }
}
