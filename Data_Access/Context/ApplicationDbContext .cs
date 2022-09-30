using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Access.Context
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbFlight> TbFlight { get; set; }
        public virtual DbSet<TbJourney> TbJourney { get; set; }
        public virtual DbSet<TbJourneyFlight> TbJourneyFlight { get; set; }
        public virtual DbSet<TbTransport> TbTransport { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=ASUS;Database=NewShore;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TbFlight>(entity =>
            {
                entity.HasKey(e => e.IdFlight)
                    .HasName("pk_IdFlight");

                entity.ToTable("TB_Flight");

                entity.Property(e => e.Destination)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Origin)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdTransportNavigation)
                    .WithMany(p => p.TbFlight)
                    .HasForeignKey(d => d.IdTransport)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_TB_Transport");
            });

            modelBuilder.Entity<TbJourney>(entity =>
            {
                entity.HasKey(e => e.IdJourney);

                entity.ToTable("TB_Journey");

                entity.Property(e => e.Destination)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Origin)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TbJourneyFlight>(entity =>
            {
                entity.HasKey(e => e.IdJourneyFlight)
                    .HasName("pk_IdJourneyFlight");

                entity.ToTable("TB_JourneyFlight");

                entity.HasOne(d => d.IdFlightNavigation)
                    .WithMany(p => p.TbJourneyFlight)
                    .HasForeignKey(d => d.IdFlight)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_IdFlight");

                entity.HasOne(d => d.IdJourneyNavigation)
                    .WithMany(p => p.TbJourneyFlight)
                    .HasForeignKey(d => d.IdJourney)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_TB_Journey");
            });

            modelBuilder.Entity<TbTransport>(entity =>
            {
                entity.HasKey(e => e.IdTransport);

                entity.ToTable("TB_Transport");

                entity.Property(e => e.FlightCarrier)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FlightNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}