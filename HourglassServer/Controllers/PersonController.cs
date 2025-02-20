﻿using HourglassServer.Data;
using HourglassServer.Models.Persistent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


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
                return BadRequest(new { tag = "duplicateEmail" });
            }

            var (salt, hash) = UserPasswordHasher.HashPassword(model.Password);
            await _context.InsertAsync(new Person
            {
                Name = model.Name,
                Email = model.Email,
                Role = (int)newRole,
                Salt = salt,
                PasswordHash = hash,
                Occupation = model.Occupation == "" ? null : model.Occupation,
                NotificationsEnabled = model.NotificationsEnabled,
                IsThirdParty = model.IsThirdParty
            });

            return Ok(new { email = model.Email });
        }

        [HttpPut("{email}")]
        public async Task<IActionResult> Put(string email, [FromBody]UserSettingsModel userSettings)
        {
            Person person;
            try
            {
                person = await _context.Person.SingleAsync(p => p.Email == email);
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new { errorName = "unusedEmail" });
            }

            if (userSettings.PasswordChangeRequest != null)
            {
                bool enteredCorrectPassword = UserPasswordHasher.PasswordMatches(
                    userSettings.PasswordChangeRequest.CurrentPassword,
                    person.Salt,
                    person.PasswordHash);

                if (!enteredCorrectPassword)
                {
                    return Unauthorized(new { tag = "badLogin" });
                }

                var (salt, hash) = UserPasswordHasher.HashPassword(userSettings.PasswordChangeRequest.NewPassword);
                person.Salt = salt;
                person.PasswordHash = hash;
            }

            person.Name = userSettings.Name ?? person.Name;
            person.Occupation = userSettings.Occupation ?? person.Occupation;
            person.NotificationsEnabled = userSettings.NotificationsEnabled ?? person.NotificationsEnabled;

            await _context.UpdateAsync(person);
            return Ok(new { email });
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
