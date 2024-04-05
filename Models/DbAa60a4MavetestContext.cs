using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MAVE.Models;

public partial class DbAa60a4MavetestContext : DbContext
{
    public DbAa60a4MavetestContext()
    {
    }

    public DbAa60a4MavetestContext(DbContextOptions<DbAa60a4MavetestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Auditory> Auditories { get; set; }

    public virtual DbSet<CatArticleType> CatArticleTypes { get; set; }

    public virtual DbSet<CatEvaluation> CatEvaluations { get; set; }

    public virtual DbSet<CatQuestion> CatQuestions { get; set; }

    public virtual DbSet<CatRole> CatRoles { get; set; }

    public virtual DbSet<Mood> Moods { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionUser> QuestionUsers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Base");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.ToTable("ARTICLE");

            entity.Property(e => e.ArticleName).HasMaxLength(255);
            entity.Property(e => e.Link).HasMaxLength(255);
            entity.Property(e => e.Picture).HasMaxLength(255);
            entity.Property(e => e.Resume).HasColumnType("text");

            entity.HasOne(d => d.Type).WithMany(p => p.Articles)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ARTICLE_CAT_ARTICLE_TYPE");

            entity.HasOne(d => d.User).WithMany(p => p.Articles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ARTICLE_USER");
        });

        modelBuilder.Entity<Auditory>(entity =>
        {
            entity.HasKey(e => e.AuditId);

            entity.ToTable("AUDITORY");

            entity.Property(e => e.Action)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.NewValue).HasMaxLength(255);
            entity.Property(e => e.OldValue).HasMaxLength(255);
        });

        modelBuilder.Entity<CatArticleType>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.ToTable("CAT_ARTICLE_TYPE");

            entity.Property(e => e.ArticleType)
                .HasMaxLength(120)
                .IsFixedLength();
        });

        modelBuilder.Entity<CatEvaluation>(entity =>
        {
            entity.HasKey(e => e.EvaluationId);

            entity.ToTable("CAT_EVALUATION");

            entity.Property(e => e.Result)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CatQuestion>(entity =>
        {
            entity.ToTable("CAT_QUESTION");

            entity.Property(e => e.Question).HasColumnType("text");
        });

        modelBuilder.Entity<CatRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("CAT_ROLE");

            entity.Property(e => e.Role)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Mood>(entity =>
        {
            entity.ToTable("MOOD");

            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");

            entity.HasOne(d => d.User).WithMany(p => p.Moods)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MOOD_USER");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK_HABIT");

            entity.ToTable("QUESTION");

            entity.Property(e => e.EvaluationMax)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.EvaluationMin)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.CatQuestion).WithMany(p => p.Questions)
                .HasForeignKey(d => d.CatQuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HABIT_CAT_QUESTION");
        });

        modelBuilder.Entity<QuestionUser>(entity =>
        {
            entity.HasKey(e => e.HabitUserId).HasName("PK_HABIT_USER");

            entity.ToTable("QUESTION_USER");

            entity.HasOne(d => d.Habit).WithMany(p => p.QuestionUsers)
                .HasForeignKey(d => d.HabitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HABIT_USER_HABIT");

            entity.HasOne(d => d.User).WithMany(p => p.QuestionUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HABIT_USER_USER");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_User");

            entity.ToTable("USER");

            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.UserName).HasMaxLength(50);

            entity.HasOne(d => d.Evaluation).WithMany(p => p.Users)
                .HasForeignKey(d => d.EvaluationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_CAT_EVALUATION");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_CAT_ROLE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
