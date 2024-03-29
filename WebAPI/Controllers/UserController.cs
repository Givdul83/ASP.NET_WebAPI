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
    public class UserController(DataContext context) : ControllerBase
    {
        private readonly DataContext _context = context;



        [HttpPost]

        public async Task<IActionResult> Create(string email)
        {
            if(ModelState.IsValid)
            {
                var newUser = new UserEntity
                {
                    Email = email,
                };

                if(newUser !=  null)
                {
                    var newUserEntity = await _context.Users.AddAsync(newUser);
                    await _context.SaveChangesAsync();

                    return Ok(newUserEntity);
                }

                else
                {
                    return NoContent();
                }
                                        
            }
            return BadRequest();

        }




        [HttpGet("{email}")]

        public async Task<IActionResult> GetOne(string email)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

                if (user != null!)
                {
                    return Ok(user);
                }

                return NotFound();
            }
            return BadRequest();

        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _context.Users.ToListAsync();

                if (users != null)
                {
                    return Ok(users);
                }

                else
                {
                    return NoContent();
                }
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
