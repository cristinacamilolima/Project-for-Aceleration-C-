using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_for_Aceleration_Csharp_Tryitter.Context;
using Project_for_Aceleration_Csharp_Tryitter.DTO;
using Project_for_Aceleration_Csharp_Tryitter.Models;
using Project_for_Aceleration_Csharp_Tryitter.Utils;

namespace Project_for_Aceleration_Csharp_Tryitter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("/register")]
        public ActionResult Post(UserDTO user)
        {
            if (!Validate.Validate.ValidateEmail(user.Email!))
                return BadRequest("The email is invalid");

            var exist = _context.Users!.FirstOrDefault(userData => userData.Email == user.Email);
            if (exist != null)
                return BadRequest("The email is already registered");

            if (user.Admin)
            {
                var hasToken = Request.Headers.TryGetValue("Authorization", out var bearerToken);
                if (!hasToken)
                    return Unauthorized("Token not found");

                if (!Validate.Validate.IsAdminUser(bearerToken))
                    return Unauthorized("Access denied");
            }

            User userModel = new()
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Admin = user.Admin,
            };

            _context.Users!.Add(userModel);
            _context.SaveChanges();

            var result = new CreatedAtRouteResult("GetUser",
                new { id = userModel.UserId }, user);            

            return result;
        }

        [HttpPost]
        [Route("/Login")]
        [AllowAnonymous]
        public ActionResult Login(AuthDTO req)
        {
            var user = _context.Users!.FirstOrDefault(user => user.Email == req.Email && user.Password == req.Password);
            if (user is null)
                return Unauthorized("Invalid User.");

            var jwtAuthenticationManager = new TokenGenerator();
            var authResult = jwtAuthenticationManager.CreateToken(user.Email, user.Admin);

            return Ok(authResult);
        }
    }
}
