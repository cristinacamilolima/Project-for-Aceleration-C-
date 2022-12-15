using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_for_Aceleration_Csharp_Tryitter.Context;
using Project_for_Aceleration_Csharp_Tryitter.Models;

namespace Project_for_Aceleration_Csharp_Tryitter.Controllers
{
    public class PostController : Controller
    {
        [Route("[controller]")]
        [ApiController]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public class PostsController : ControllerBase
        {
            private readonly AppDbContext _context;
            public PostsController(AppDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            [AllowAnonymous]
            public ActionResult<IEnumerable<Post>> GetAll()
            {
                var posts = _context.Posts!.AsNoTracking();
                if (posts is null)
                    return NotFound("Posts not found.");

                return Ok(posts);
            }
        }
    }
}
