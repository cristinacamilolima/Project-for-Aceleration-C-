using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_for_Aceleration_Csharp_Tryitter.Context;
using Project_for_Aceleration_Csharp_Tryitter.DTO;

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
    }
}
