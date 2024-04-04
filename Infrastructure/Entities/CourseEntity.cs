

namespace Infrastructure.Entities;

public class CourseEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = null!; 

    public decimal? Price {  get; set; }
    
    public decimal? DiscountPrice { get; set; }

    public int? Hours { get; set; }

    public bool IsBestSeller { get; set; } 

    public decimal? LikesInNumbers {  get; set; }

    public decimal? LikesInPercent { get; set; }

    public string? Author {  get; set; }

    public string? CoursePictureUrl { get; set; }


    public virtual IEnumerable<SavedCoursesEntity> SavedCourses { get; set; } = new List<SavedCoursesEntity>();

    public virtual IEnumerable<MyCoursesEntity> MyCourses { get; set; } = new List<MyCoursesEntity>();
}
