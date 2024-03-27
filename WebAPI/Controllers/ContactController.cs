using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController(DataContext context) : ControllerBase
    {

        private readonly DataContext _context = context;



        #region Create
        [HttpPost]
        [UseApiKey]

        public async Task<IActionResult> Create(ContactFormDto dto)
        {
            if (ModelState.IsValid)
            {
                var contactForm = new ContactFormEntity
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    Service = dto.Service,
                    Message = dto.Message,
                };

                await _context.ContactForms.AddAsync(contactForm);
                await _context.SaveChangesAsync();

                return Created("Contact was created and recived by Silicon", contactForm);
            }
            return BadRequest("Invalid fields in form, try again");
        }

        #endregion
        #region GET
        [HttpGet("{id}")]

        public async Task<IActionResult> GetOne(int id)
        {
            var contactForm = await _context.ContactForms.FindAsync(id);

            if (contactForm == null)
            {
                return NotFound("contact form was not found");
            }

            return Ok(contactForm);
        }



        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var contactForms = await _context.ContactForms.ToListAsync();

            if(contactForms.Count== 0)
            {
                return NotFound("No contact forms found");
            }

            return Ok(contactForms);
         }
        #endregion

        #region DELETE

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            var contactForm = await _context.ContactForms.FindAsync(id);

            if (contactForm == null)
            {
                return NotFound("Contact form not found");
            }

            _context.ContactForms.Remove(contactForm);
            await _context.SaveChangesAsync();

            return Ok("Contact form was deleted");
        }



        #endregion

        #region UPDATE
        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, ContactFormDto dto)
        {
            var contactForm = await _context.ContactForms.FindAsync(id);

            if (contactForm == null)
            {
                return NotFound("Contact form not found");
            }

            contactForm.FullName = dto.FullName;
            contactForm.Email = dto.Email;
            contactForm.Service = dto.Service;
            contactForm.Message = dto.Message;

            await _context.SaveChangesAsync();

            return Ok("Contact form was updated");
        }
        #endregion
    }
}
