﻿using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.DbContexts;

public partial class FplmsManagementContext : DbContext
{
    public FplmsManagementContext()
    {
    }

    public FplmsManagementContext(DbContextOptions<FplmsManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<CycleReport> CycleReports { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Lecturer> Lecturers { get; set; }

    public virtual DbSet<Meeting> Meetings { get; set; }

    public virtual DbSet<ProgressReport> ProgressReports { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Semester> Semesters { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentGroup> StudentGroups { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<StudentAnswerUpvote> StudentAnswerUpvotes { get; set; }

    public virtual DbSet<StudentUpvote> StudentUpvotes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL("Server=localhost;Database=fplms;User=root;Password=Password1234;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("class");

            entity.HasIndex(e => e.LecturerId, "fk_CLASS_LECTURER1_idx");

            entity.HasIndex(e => e.SubjectId, "fk_CLASS_SUBJECT1_idx");

            entity.HasIndex(e => e.SemesterCode, "fk_class_SEMESTER1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CycleDuration).HasColumnName("cycle_duration");
            entity.Property(e => e.EnrollKey)
                .HasMaxLength(45)
                .HasColumnName("enroll_key");
            entity.Property(e => e.IsDisable)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_disable");
            entity.Property(e => e.LecturerId).HasColumnName("LECTURER_id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.SemesterCode)
                .HasMaxLength(10)
                .HasColumnName("SEMESTER_code");
            entity.Property(e => e.SubjectId).HasColumnName("SUBJECT_id");

            entity.HasOne(d => d.Lecturer).WithMany(p => p.Classes)
                .HasForeignKey(d => d.LecturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_CLASS_LECTURER1");

            entity.HasOne(d => d.SemesterCodeNavigation).WithMany(p => p.Classes)
                .HasForeignKey(d => d.SemesterCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_class_SEMESTER1");

            entity.HasOne(d => d.Subject).WithMany(p => p.Classes)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_CLASS_SUBJECT1");
        });

        modelBuilder.Entity<CycleReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cycle_report");

            entity.HasIndex(e => e.GroupId, "fk_PROGRESS_REPORT_copy1_GROUP1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.CycleNumber).HasColumnName("cycle_number");
            entity.Property(e => e.Feedback)
                .HasColumnType("text")
                .HasColumnName("feedback");
            entity.Property(e => e.GroupId).HasColumnName("GROUP_id");
            entity.Property(e => e.Mark).HasColumnName("mark");
            entity.Property(e => e.ResourceLink)
                .HasColumnType("text")
                .HasColumnName("resource_link");
            entity.Property(e => e.Title)
                .HasColumnType("text")
                .HasColumnName("title");

            entity.HasOne(d => d.Group).WithMany(p => p.CycleReports)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_PROGRESS_REPORT_copy1_GROUP1");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("group");

            entity.HasIndex(e => e.ClassId, "fk_GROUP_CLASS1_idx");

            entity.HasIndex(e => e.ProjectId, "fk_GROUP_PROJECT1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClassId).HasColumnName("CLASS_id");
            entity.Property(e => e.EnrollTime)
                .HasColumnType("timestamp")
                .HasColumnName("enroll_time");
            entity.Property(e => e.IsDisable)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_disable");
            entity.Property(e => e.MemberQuantity).HasColumnName("member_quantity");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.ProjectId).HasColumnName("PROJECT_id");

            entity.HasOne(d => d.Class).WithMany(p => p.Groups)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_GROUP_CLASS1");

            entity.HasOne(d => d.Project).WithMany(p => p.Groups)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("fk_GROUP_PROJECT1");
        });

        modelBuilder.Entity<Lecturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("lecturer");

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.ImageUrl)
                .HasColumnType("text")
                .HasColumnName("image_url");
            entity.Property(e => e.IsDisable).HasColumnName("is_disable");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Meeting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("meeting");

            entity.HasIndex(e => e.GroupId, "fk_MEETING_GROUP1_idx");

            entity.HasIndex(e => e.LecturerId, "fk_MEETING_LECTURER1_idx");

            entity.HasIndex(e => e.Id, "id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Feedback)
                .HasColumnType("text")
                .HasColumnName("feedback");
            entity.Property(e => e.GroupId).HasColumnName("GROUP_id");
            entity.Property(e => e.LecturerId).HasColumnName("LECTURER_id");
            entity.Property(e => e.Link)
                .HasColumnType("text")
                .HasColumnName("link");
            entity.Property(e => e.ScheduleTime)
                .HasColumnType("timestamp")
                .HasColumnName("schedule_time");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Group).WithMany(p => p.Meetings)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_MEETING_GROUP1");

            entity.HasOne(d => d.Lecturer).WithMany(p => p.Meetings)
                .HasForeignKey(d => d.LecturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_MEETING_LECTURER1");
        });

        modelBuilder.Entity<ProgressReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("progress_report");

            entity.HasIndex(e => e.GroupId, "fk_PROGRESS_REPORT_GROUP1_idx");

            entity.HasIndex(e => e.StudentId, "fk_PROGRESS_REPORT_STUDENT_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.GroupId).HasColumnName("GROUP_id");
            entity.Property(e => e.ReportTime)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("report_time");
            entity.Property(e => e.StudentId).HasColumnName("STUDENT_id");
            entity.Property(e => e.Title)
                .HasColumnType("text")
                .HasColumnName("title");

            entity.HasOne(d => d.Group).WithMany(p => p.ProgressReports)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_PROGRESS_REPORT_GROUP1");

            entity.HasOne(d => d.Student).WithMany(p => p.ProgressReports)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_PROGRESS_REPORT_STUDENT");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("project");

            entity.HasIndex(e => e.SubjectId, "fk_PROJECT_SUBJECT1_idx");

            entity.HasIndex(e => e.LecturerId, "fk_project_lecturer1_idx");

            entity.HasIndex(e => e.SemesterCode, "fk_project_semester1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Actors)
                .HasColumnType("text")
                .HasColumnName("actors");
            entity.Property(e => e.Context)
                .HasColumnType("text")
                .HasColumnName("context");
            entity.Property(e => e.IsDisable).HasColumnName("is_disable");
            entity.Property(e => e.LecturerId).HasColumnName("LECTURER_id");
            entity.Property(e => e.Name)
                .HasColumnType("text")
                .HasColumnName("name");
            entity.Property(e => e.Problem)
                .HasColumnType("text")
                .HasColumnName("problem");
            entity.Property(e => e.Requirements)
                .HasColumnType("text")
                .HasColumnName("requirements");
            entity.Property(e => e.SemesterCode)
                .HasMaxLength(10)
                .HasColumnName("SEMESTER_code");
            entity.Property(e => e.SubjectId).HasColumnName("SUBJECT_id");
            entity.Property(e => e.Theme)
                .HasColumnType("text")
                .HasColumnName("theme");

            entity.HasOne(d => d.Lecturer).WithMany(p => p.Projects)
                .HasForeignKey(d => d.LecturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_project_lecturer1");

            entity.HasOne(d => d.SemesterCodeNavigation).WithMany(p => p.Projects)
                .HasForeignKey(d => d.SemesterCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_project_semester1");

            entity.HasOne(d => d.Subject).WithMany(p => p.Projects)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("fk_PROJECT_SUBJECT1");
        });

        modelBuilder.Entity<Semester>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("semester");

            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .HasColumnName("code");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("end_date");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("start_date");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("student");

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .HasColumnName("code");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.ImageUrl)
                .HasColumnType("text")
                .HasColumnName("image_url");
            entity.Property(e => e.IsDisable).HasColumnName("is_disable");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");

            entity.HasMany(d => d.Classes).WithMany(p => p.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentClass",
                    r => r.HasOne<Class>().WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_STUDENT_has_CLASS_CLASS1"),
                    l => l.HasOne<Student>().WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_STUDENT_has_CLASS_STUDENT1"),
                    j =>
                    {
                        j.HasKey("StudentId", "ClassId").HasName("PRIMARY");
                        j.ToTable("student_class");
                        j.HasIndex(new[] { "ClassId" }, "fk_STUDENT_has_CLASS_CLASS1_idx");
                        j.HasIndex(new[] { "StudentId" }, "fk_STUDENT_has_CLASS_STUDENT1_idx");
                        j.IndexerProperty<int>("StudentId").HasColumnName("STUDENT_id");
                        j.IndexerProperty<int>("ClassId").HasColumnName("CLASS_id");
                    });
        });

        modelBuilder.Entity<StudentGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("student_group");

            entity.HasIndex(e => e.ClassId, "fk_STUDENT_GROUP_CLASS1_idx");

            entity.HasIndex(e => e.GroupId, "fk_STUDENT_has_GROUP_GROUP1_idx");

            entity.HasIndex(e => e.StudentId, "fk_STUDENT_has_GROUP_STUDENT1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClassId).HasColumnName("CLASS_id");
            entity.Property(e => e.GroupId).HasColumnName("GROUP_id");
            entity.Property(e => e.IsLeader).HasColumnName("is_leader");
            entity.Property(e => e.StudentId).HasColumnName("STUDENT_id");

            entity.HasOne(d => d.Class).WithMany(p => p.StudentGroups)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_STUDENT_GROUP_CLASS1");

            entity.HasOne(d => d.Group).WithMany(p => p.StudentGroups)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_STUDENT_has_GROUP_GROUP1");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentGroups)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_STUDENT_has_GROUP_STUDENT1");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("subject");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsDisable).HasColumnName("is_disable");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<StudentUpvote>()
               .HasKey(x => new { x.StudentId, x.QuestionId });
        modelBuilder.Entity<StudentUpvote>()
            .HasOne(pt => pt.Student)
            .WithMany(p => p.UpvotedQuestions)
            .HasForeignKey(pt => pt.StudentId);
        modelBuilder.Entity<StudentUpvote>()
            .HasOne(pt => pt.Question)
            .WithMany(t => t.Upvoters)
            .HasForeignKey(pt => pt.QuestionId);

        modelBuilder.Entity<StudentAnswerUpvote>()
            .HasKey(x => new { x.StudentId, x.AnswerId });
        modelBuilder.Entity<StudentAnswerUpvote>()
            .HasOne(pt => pt.Student)
            .WithMany(p => p.UpvotedAnswers)
            .HasForeignKey(pt => pt.StudentId);
        modelBuilder.Entity<StudentAnswerUpvote>()
            .HasOne(pt => pt.Answer)
            .WithMany(t => t.Upvoters)
            .HasForeignKey(pt => pt.AnswerId);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}