using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HourglassServer.Data;
using HourglassServer.Models.Persistent;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HourglassServer
{
    [DefaultControllerRoute]
    public class PersonController : ControllerBase
    {
        private HourglassContext _context;
        private readonly ILogger _logger;

        public PersonController(HourglassContext context, ILogger<PersonController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<Person> Get(int id)
        {
            if (Utilities.UserHasPermission(User, id))
            {
                return await _context.FindAsync<Person>(id);
            }
            else
            {
                return null;
            }
        }

        private async Task<bool> DuplicateEmail(RegisterModel model)
        {
            return await _context.Person.Where(p => p.Email == model.Email).FirstOrDefaultAsync() != null;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegisterModel model)
        {
            Role newRole = Role.BaseUser;

            if (!Enum.TryParse(model.Role, out newRole))
            {
                ModelState.AddModelError("role", "Invalid role type");
                return BadRequest(ModelState);
            }
            if (model.Role == Role.Admin.ToString() && !User.HasRole(Role.Admin))
            {
                return Forbid();
            }
            if (await DuplicateEmail(model))
            {
                return BadRequest(new { errorName = "duplicateEmail" });
            }

            var (salt, hash) = Utilities.HashPassword(model.Password);
            await _context.InsertAsync(new Person
            {
                Name = model.Name,
                Email = model.Email,
                Role = (int) newRole,
                Salt = salt,
                PasswordHash = hash,
                Occupation = model.Occupation == "" ? null : model.Occupation
            });

            return Ok(new { email = model.Email });
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [UserExists]
        [HttpDelete("{email}")]
        public async Task<IActionResult> Delete(string email)
        {
            Person person;
            try
            {
                person = await _context.Person.SingleAsync(p => p.Email == email);
            }
            catch
            {
                return NotFound();
            }
            if (!HttpContext.User.HasRole(Role.Admin))
            {
                if (HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).First().Value == person.Email)
                {
                    await _context.DeleteAsync(person);
                    return Ok(new { email });
                }
                return Forbid();
            }
            await _context.DeleteAsync(person);
            return Ok(new { email });
        }
    }
}
