using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MySchool.Models;

public partial class SchoolLabbDbContext : DbContext
{
    public SchoolLabbDbContext()
    {
    }

    public SchoolLabbDbContext(DbContextOptions<SchoolLabbDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<SchoolClass> SchoolClasses { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Course__C92D71A7044834B4");

            entity.ToTable("Course");

            entity.HasIndex(e => e.CourseName, "UQ__Course__9526E277B86BB832").IsUnique();

            entity.Property(e => e.CourseName)
                .IsRequired()
                .HasMaxLength(40);

            entity.HasOne(d => d.FkTeacher).WithMany(p => p.Courses)
                .HasForeignKey(d => d.FkTeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Course__FkTeache__440B1D61");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04F11DD1F7677");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.PersonalNumber, "UQ__Employee__AC2CC42E2B1494F2").IsUnique();

            entity.Property(e => e.EmployeeRole)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(64);
            entity.Property(e => e.PersonalNumber)
                .IsRequired()
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.GradeId).HasName("PK__Grade__54F87A5735B91B18");

            entity.ToTable("Grade");

            entity.HasOne(d => d.FkCourse).WithMany(p => p.Grades)
                .HasForeignKey(d => d.FkCourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Grade__FkCourseI__47DBAE45");

            entity.HasOne(d => d.FkStudent).WithMany(p => p.Grades)
                .HasForeignKey(d => d.FkStudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Grade__FkStudent__46E78A0C");

            entity.HasOne(d => d.FkTeacher).WithMany(p => p.Grades)
                .HasForeignKey(d => d.FkTeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Grade__FkTeacher__49C3F6B7");
        });


        modelBuilder.Entity<SchoolClass>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__SchoolCl__CB1927C00E4A6265");

            entity.ToTable("SchoolClass");

            entity.HasIndex(e => e.ClassName, "UQ__SchoolCl__F8BF561BC27134CE").IsUnique();

            entity.Property(e => e.ClassName)
                .IsRequired()
                .HasMaxLength(20);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52B998344E814");

            entity.ToTable("Student");

            entity.HasIndex(e => e.PersonalNumber, "UQ__Student__AC2CC42E65771770").IsUnique();

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Gender)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.PersonalNumber)
                .IsRequired()
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.FkClass).WithMany(p => p.Students)
                .HasForeignKey(d => d.FkClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Student__FkClass__403A8C7D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
