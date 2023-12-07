using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Project2.Models
{
    public partial class project_prnContext : DbContext
    {
        public project_prnContext()
        {
        }

        public project_prnContext(DbContextOptions<project_prnContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bailam> Bailams { get; set; } = null!;
        public virtual DbSet<Cauhoi> Cauhois { get; set; } = null!;
        public virtual DbSet<Dethi> Dethis { get; set; } = null!;
        public virtual DbSet<Monhoc> Monhocs { get; set; } = null!;
        public virtual DbSet<TraloiDetail> TraloiDetails { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server =LAPTOP-0E04AKRD\\NAMSQL; database = project_prn;uid=sa;pwd=nam12345;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bailam>(entity =>
            {
                entity.ToTable("Bailam");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.DethiId).HasColumnName("Dethi_id");

                entity.Property(e => e.End).HasColumnType("datetime");

                entity.Property(e => e.Start).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("User_id");

                entity.HasOne(d => d.Dethi)
                    .WithMany(p => p.Bailams)
                    .HasForeignKey(d => d.DethiId)
                    .HasConstraintName("FK__Bailam__Dethi_id__2F10007B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bailams)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Bailam__User_id__2E1BDC42");
            });

            modelBuilder.Entity<Cauhoi>(entity =>
            {
                entity.ToTable("Cauhoi");

                entity.Property(e => e.A).HasMaxLength(2500);

                entity.Property(e => e.B).HasMaxLength(2500);

                entity.Property(e => e.C).HasMaxLength(2500);

                entity.Property(e => e.D).HasMaxLength(2500);

                entity.Property(e => e.DapAn).HasColumnName("Dap_an");

                entity.Property(e => e.DethiId).HasColumnName("Dethi_id");

                entity.Property(e => e.NoiDung)
                    .HasMaxLength(2500)
                    .HasColumnName("Noi_dung");

                entity.HasOne(d => d.Dethi)
                    .WithMany(p => p.Cauhois)
                    .HasForeignKey(d => d.DethiId)
                    .HasConstraintName("FK__Cauhoi__Dethi_id__2B3F6F97");
            });

            modelBuilder.Entity<Dethi>(entity =>
            {
                entity.ToTable("Dethi");

                entity.Property(e => e.MonhocId).HasColumnName("Monhoc_id");

                entity.HasOne(d => d.Monhoc)
                    .WithMany(p => p.Dethis)
                    .HasForeignKey(d => d.MonhocId)
                    .HasConstraintName("FK__Dethi__Monhoc_id__286302EC");
            });

            modelBuilder.Entity<Monhoc>(entity =>
            {
                entity.ToTable("Monhoc");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<TraloiDetail>(entity =>
            {
                entity.ToTable("TraloiDetail");

                entity.Property(e => e.BailamId).HasColumnName("Bailam_id");

                entity.Property(e => e.IsCorrect).HasColumnName("Is_correct");

                entity.Property(e => e.QuesId).HasColumnName("Ques_id");

                entity.HasOne(d => d.Bailam)
                    .WithMany(p => p.TraloiDetails)
                    .HasForeignKey(d => d.BailamId)
                    .HasConstraintName("FK__TraloiDet__Baila__31EC6D26");

                entity.HasOne(d => d.Ques)
                    .WithMany(p => p.TraloiDetails)
                    .HasForeignKey(d => d.QuesId)
                    .HasConstraintName("FK__TraloiDet__Ques___32E0915F");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Password)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
