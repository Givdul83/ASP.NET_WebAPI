using Infrastructure.Contexts;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Entities;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SubscriberController(DataContext context) : ControllerBase
    {
        private readonly DataContext _context = context;


        #region Create
        [HttpPost]
        [UseApiKey]
        public async Task<IActionResult> Create(SubscriberDto dto)
        {
            if (ModelState.IsValid)
            {
                if (!await _context.Subscribers.AnyAsync(x => x.Email == dto.Email))
                {
                    var subscriber = new SubscriberEntity
                    {
                        Email = dto.Email,
                        DailyNewsletter = dto.DailyNewsletter,
                        StartupsWeekly = dto.StartupsWeekly,
                        AdvertisingUpdates = dto.AdvertisingUpdates,
                        Podcasts = dto.Podcasts,
                        EventUpdates = dto.EventUpdates,
                        WeekInReview = dto.WeekInReview
                    };
                    await _context.Subscribers.AddAsync(subscriber);
                    await _context.SaveChangesAsync();
                    return Created("You have been registered", subscriber);

                }

                return Conflict("Email is already registered");
            }

            return BadRequest("Invalid email");
        }
        #endregion

        #region Delete
        [HttpDelete]

        public async Task<IActionResult> Delete(int id)
        {
            var subscriber = await _context.Subscribers.FindAsync(id);
            if (subscriber == null)
            {
                return NotFound("Subscriber not found");
            }
            else
            {
                _context.Subscribers.Remove(subscriber);
                await _context.SaveChangesAsync();

                return Ok("Subscriber was deleted");
            }
        }

        [HttpDelete("email")]
        [UseApiKey]
        public async Task<IActionResult> DeleteWithEmail(string email)
        {
            var result = await FindByEmail(email);

            if (result is OkObjectResult okResult && okResult.Value is SubscriberEntity sub)
            {
                _context.Subscribers.Remove(sub);
                await _context.SaveChangesAsync();
                return Ok("Subscriber was deleted");
            }

            return result;
        }
        #endregion

        #region GET

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var subscribers = await _context.Subscribers.ToListAsync();

            return Ok(subscribers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var subscriber = await _context.Subscribers.FindAsync(id);

            if (subscriber == null)
            {
                return NotFound("Subscriber not found");
            }

            return Ok(subscriber);
        }

        [HttpGet("email/{email}")]

        public async Task<IActionResult> FindByEmail(string email)
        {

            var sub = await _context.Subscribers.FirstOrDefaultAsync(x => x.Email == email);

            if (sub != null)
            {
                return Ok(sub);
            }

            return NotFound();


        }
        #endregion

        #region UPDATE

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, SubscriberDto dto)
        {
            var subscriber = await _context.Subscribers.FindAsync(id);

            if (subscriber == null)
            {
                return NotFound("Subscriber not found");
            }

            subscriber.Email = dto.Email;
            subscriber.DailyNewsletter = dto.DailyNewsletter;
            subscriber.AdvertisingUpdates = dto.AdvertisingUpdates;
            subscriber.WeekInReview = dto.WeekInReview;
            subscriber.EventUpdates = dto.EventUpdates;
            subscriber.StartupsWeekly = dto.StartupsWeekly;
            subscriber.Podcasts = dto.Podcasts;

            await _context.SaveChangesAsync();

            return Ok("Subscriber was updated");
        }
        #endregion
    }
}
