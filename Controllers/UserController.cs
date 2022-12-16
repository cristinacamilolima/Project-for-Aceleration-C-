using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_for_Aceleration_Csharp_Tryitter.Context;
using Project_for_Aceleration_Csharp_Tryitter.DTO;
using System.Security.Claims;

namespace Project_for_Aceleration_Csharp_Tryitter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<UserDTO>> GetAll()
        {
            var users = _context.Users!.AsNoTracking();
            if (users is null)
                return NotFound("Users not found.");

            return Ok(users.Select(u => new
            {
                Id = u.UserId,
                Name = u.Name,
                Email = u.Email,
                Admin = u.Admin
            }));
        }

        [HttpGet("{id:Guid}", Name = "GetUser")]
        public ActionResult<dynamic> GetById(Guid id)
        {
            var user = _context.GetUser(id);
            if (Validate.Validate.ValidateUser(user))
            {
                var output = new
                {
                    id = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    Admin = user.Admin
                };

                return Ok(output);
            }               

            return NotFound("User not found.");
        }

        [HttpPut("{id:Guid}", Name = "Update To Admin")]
        [Authorize(Policy = "TryitterAdministrators")]
        public ActionResult UpdateByAdmin(Guid id, UserDTO user)
        {
            var userData = _context.GetUser(id);
            if (!Validate.Validate.ValidateUser(userData))
                return BadRequest("User not found.");

            userData.UserId = id;
            userData.Name = user.Name;
            userData.Email = user.Email;
            userData.Password = user.Password;
            userData.Admin = user.Admin;

            _context.Entry(userData).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(userData);
        }

        [HttpPut]
        public ActionResult UpdateByUser(UserDTO user)
        {
            var claims = HttpContext.User.Identity as ClaimsIdentity;
            var email = claims.Claims.FirstOrDefault(c => c.Type == "Email")!.Value;
            var userData = _context.GetUser(email);

            userData.Email = user.Email;
            userData.Name = user.Name;
            userData.Password = user.Password;

            _context.Entry(userData).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id:Guid}", Name = "Delete Admin")]
        [Authorize(Policy = "TryitterAdministrators")]
        public ActionResult DeleteByAdmin(Guid id)
        {
            var user = _context.Users!.FirstOrDefault(user => user.UserId == id);
            if (user is null)
                return NotFound("User not found.");

            _context.Users!.Remove(user);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete]
        public ActionResult DeleteByUser()
        {
            var claims = HttpContext.User.Identity as ClaimsIdentity;
            var email = claims.Claims.FirstOrDefault(c => c.Type == "Email")!.Value;
            var user = _context.GetUser(email);

            if (user is null)
                return NotFound("User not found.");

            _context.Users!.Remove(user);
            _context.SaveChanges();

            return NoContent();
        }

    }
}
