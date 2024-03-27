

namespace Infrastructure.Entities;

public class SavedCoursesEntity
{
    public int UserId { get; set; }

    public virtual UserEntity User { get; set; } = null!;

    public int CourseId {  get; set; }

    public virtual CourseEntity Course { get; set; } = null!;


}
