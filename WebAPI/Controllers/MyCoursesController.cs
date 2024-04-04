using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyCoursesController(DataContext context) : ControllerBase
    {
        private readonly DataContext _context = context;

        [HttpPost]

        public async Task<IActionResult> SaveCourse(CourseToSaveDto dto)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.UserEmail);

                if (user == null)
                {
                    var newUser = new UserEntity
                    {
                        Email = dto.UserEmail,
                    };

                    await _context.Users.AddAsync(newUser);
                    await _context.SaveChangesAsync();

                    var courseToSave = new MyCoursesEntity
                    {
                        CourseId = dto.CourseId,
                        UserId = newUser.Id,
                    };

                    var result = await _context.MyCourses.AddAsync(courseToSave);
                    await _context.SaveChangesAsync();

                    if (result != null)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }

                }

                else if (user != null)
                {
                    var courseToSave = new MyCoursesEntity
                    {
                        UserId = user.Id,
                        CourseId = dto.CourseId,
                    };



                    var result = await _context.MyCourses.AddAsync(courseToSave);
                    await _context.SaveChangesAsync();

                    if (result != null)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }


                }

            }
            return BadRequest();
        }


        [HttpGet("{email}")]
        public async Task<IActionResult> GetAllForOneUser(string email)
        {
            if (ModelState.IsValid)
            {
                var courses = await GetUserCoursesAsync(email);

                if (courses == null || !courses.Any())
                {
                    return Ok(new List<CourseEntity>());
                }

                var courseList = new List<CourseEntity>();
                foreach (var myCourse in courses)
                {
                    var foundCourse = await _context.Courses.FirstOrDefaultAsync(x => x.Id == myCourse.CourseId);
                    if (foundCourse != null)
                    {
                        courseList.Add(foundCourse);
                    }
                }

                return Ok(courseList);
            }

            return BadRequest("Invalid request");
        }



        [HttpDelete("{email}")]

        public async Task<IActionResult> Delete(string email, [FromBody] CourseToSaveDto dto)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.UserEmail);
                var courses = await GetUserCoursesAsync(user!.Email);

                if (courses != null)
                {

                    var courseToDelete = courses.FirstOrDefault(x => x.CourseId == dto.CourseId);

                    if (courseToDelete != null)
                    {
                        var savedCourseEntityToDelete = await _context.MyCourses.FirstOrDefaultAsync(c => c.CourseId == courseToDelete.CourseId && c.UserId == user.Id);

                        if (savedCourseEntityToDelete != null)
                        {
                            _context.MyCourses.Remove(savedCourseEntityToDelete);
                            await _context.SaveChangesAsync();
                            return Ok("Course removed from users list");
                        }
                    }
                    return NotFound("no course to remove was found");
                }
            }
            return BadRequest();
        }

        [HttpDelete("{email}/courses")]
        public async Task<IActionResult> DeleteAllCourses(string email)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
                var courses = await GetUserCoursesAsync(user!.Email);

                if (courses != null)
                {
                    foreach (var course in courses)
                    {
                        var savedCourseEntityToDelete = await _context.MyCourses.FirstOrDefaultAsync(c => c.CourseId == course.CourseId && c.UserId == user.Id);

                        if (savedCourseEntityToDelete != null)
                        {
                            _context.MyCourses.Remove(savedCourseEntityToDelete);
                        }
                    }
                    await _context.SaveChangesAsync();
                    return Ok("All courses removed from user's list");
                }
            }
            return BadRequest();
        }

        private async Task<IEnumerable<MyCoursesEntity>> GetUserCoursesAsync(string userEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            if (user == null)
            {
                return new List<MyCoursesEntity> { };
            }

            return await _context.MyCourses.Where(sc => sc.UserId == user.Id).ToListAsync();
        }


    }
}

