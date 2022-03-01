using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TestAPI.Models
{
    public partial class testdbContext : DbContext
    {
        public testdbContext()
        {
        }

        public testdbContext(DbContextOptions<testdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Musteri> Musteri { get; set; }
        public virtual DbSet<Sepet> Sepet { get; set; }
        public virtual DbSet<SepetUrun> SepetUrun { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-30HTPH4F;Initial Catalog=testdb;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Musteri>(entity =>
            {
                entity.Property(e => e.Ad)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sehir)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Soyad)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Sepet>(entity =>
            {
                entity.HasOne(d => d.Musteri)
                    .WithMany(p => p.Sepet)
                    .HasForeignKey(d => d.MusteriId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sepet_Musteri");
            });

            modelBuilder.Entity<SepetUrun>(entity =>
            {
                entity.Property(e => e.Aciklama)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tutar).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.Sepet)
                    .WithMany(p => p.SepetUrun)
                    .HasForeignKey(d => d.SepetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SepetUrun_Sepet");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
