
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class DataContext(DbContextOptions options) : DbContext(options)
{
   public DbSet<CourseEntity> Courses { get; set; }

   public DbSet<SubscriberEntity> Subscribers { get; set; }

    public DbSet<ContactFormEntity> ContactForms { get; set; }

    public DbSet<UserEntity> Users { get; set; }

    public DbSet<SavedCoursesEntity> SavedCourses { get; set;}

    public DbSet<MyCoursesEntity> MyCourses { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

       
        modelBuilder.Entity<SavedCoursesEntity>()
            .HasKey(sc => new { sc.CourseId, sc.UserId });

        modelBuilder.Entity<SavedCoursesEntity>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.SavedCourses)
            .HasForeignKey(sc => sc.CourseId);

       
        modelBuilder.Entity<SavedCoursesEntity>()
            .HasOne(sc => sc.User)
            .WithMany(u => u.SavedCourses)
            .HasForeignKey(sc => sc.UserId);


        modelBuilder.Entity<MyCoursesEntity>()
           .HasKey(sc => new { sc.CourseId, sc.UserId });

        modelBuilder.Entity<MyCoursesEntity>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.MyCourses)
            .HasForeignKey(sc => sc.CourseId);


        modelBuilder.Entity<MyCoursesEntity>()
            .HasOne(sc => sc.User)
            .WithMany(u => u.MyCourses)
            .HasForeignKey(sc => sc.UserId);
    }
}
