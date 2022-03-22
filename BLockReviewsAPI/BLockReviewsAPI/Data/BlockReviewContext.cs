using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BLockReviewsAPI.Models;

#nullable disable

namespace BLockReviewsAPI.Data
{
    public partial class BlockReviewContext : DbContext
    {
        public BlockReviewContext()
        {
        }

        public BlockReviewContext(DbContextOptions<BlockReviewContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<UserInfo> UserInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .HasComment("카테고리 ID");

                entity.Property(e => e.FnsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("수정/삭제시간");

                entity.Property(e => e.StDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("생성시간");

                entity.Property(e => e.Title)
                    .HasMaxLength(45)
                    .HasComment("카테고리 명 - 음식점, 카페");

                entity.Property(e => e.ParentId)
                    .HasMaxLength(36)
                    .HasComment("부모 CateId");

                entity.Property(e => e.Level)
                    .HasMaxLength(10)
                    .HasComment("카테고리 Level");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasIndex(e => e.UserId, "UserId");

                entity.HasIndex(e => e.CategoryId, "cate_idx");

                entity.HasIndex(e => new { e.StoreId, e.UserId }, "store_idx");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .HasComment("리뷰 ID");

                entity.Property(e => e.CategoryId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .HasComment("카테고리 ID");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasComment("내용");

                entity.Property(e => e.FnsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("수정/삭제시간");

                entity.Property(e => e.StDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("생성시간");

                entity.Property(e => e.StoreId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .HasComment("가게 ID");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("제목");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("작성자 ID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Reviews_ibfk_2");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Reviews_ibfk_3");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Reviews_ibfk_1");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Store");

                entity.HasIndex(e => e.CategoryId, "CategoryId");

                entity.HasIndex(e => e.UserId, "UserId");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .HasComment("지점 ID");

                entity.Property(e => e.BuisnessNumber)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasComment("사업자 번호");

                entity.Property(e => e.CategoryId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .HasComment("카테고리 번호");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasComment("지점 소개");

                entity.Property(e => e.ThumbNail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("썸네일");

                entity.Property(e => e.FnsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("수정/삭제시간");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("NAME")
                    .HasComment("지점 이름");

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .HasComment("지점 번호");

                entity.Property(e => e.StDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("생성시간");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("지점주 ID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Stores)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Store_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Stores)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Store_ibfk_1");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.ToTable("UserInfo");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .HasComment("사용자 ID");

                entity.Property(e => e.Age)
                    .HasColumnName("AGE")
                    .HasComment("나이");

                entity.Property(e => e.Password)
                    .HasColumnName("Password")
                    .HasComment("사용자 비밀번호");

                entity.Property(e => e.Email)
                    .HasMaxLength(125)
                    .HasComment("이메일");

                entity.Property(e => e.FnsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("수정/삭제시간");

                entity.Property(e => e.Gender)
                    .HasMaxLength(20)
                    .HasComment("남성:0, 여성:1");

                entity.Property(e => e.AccountPrivateKey)
                    .HasMaxLength(150)
                    .HasComment("지갑 Private 키");

                entity.Property(e => e.AccountPublicKey)
                    .HasMaxLength(250)
                    .HasComment("지갑 Public 키");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("NAME")
                    .HasComment("사용자이름");

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .HasComment("휴대폰 번호");


                entity.Property(e => e.StDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("생성시간");

                entity.Property(e => e.UserType).HasComment("0:일반 유저, 1: 지점주");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
