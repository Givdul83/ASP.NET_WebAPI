
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class DataContext(DbContextOptions options) : DbContext(options)
{
   public DbSet<CourseEntity> Courses { get; set; }

   public DbSet<SubscriberEntity> Subscribers { get; set; }
}
