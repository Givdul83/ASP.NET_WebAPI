
using System.Text.Json.Serialization;

namespace Infrastructure.Entities;

public class UserEntity
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;


    [JsonIgnore]
    public IEnumerable <SavedCoursesEntity> SavedCourses { get; set; } = new List<SavedCoursesEntity>();

}
