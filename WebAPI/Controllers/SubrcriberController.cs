using Infrastructure.Contexts;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Entities;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriberController(DataContext context) : ControllerBase
    {
        private readonly DataContext _context = context;


        #region Create
        [HttpPost]

       

        public async Task<IActionResult> Create(SubscriberDTO dto)
        {
            if (ModelState.IsValid)
            {
                if(! await _context.Subscribers.AnyAsync(x => x.Email == dto.Email))
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
                    return Created("You have been registered",subscriber);

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
            if(subscriber ==null)
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
        #endregion

        #region UPDATE

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, SubscriberDTO dto)
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
