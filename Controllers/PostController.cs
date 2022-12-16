using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_for_Aceleration_Csharp_Tryitter.Context;
using Project_for_Aceleration_Csharp_Tryitter.Models;

namespace Project_for_Aceleration_Csharp_Tryitter.Controllers
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

        [HttpPost]
        public ActionResult Post(Post post)
        {
            if (post is null)
                return BadRequest();

            Request.Headers.TryGetValue("Authorization", out var bearerToken);

            var user = _context.GetUser(bearerToken);

            if (!Validate.Validate.ValidateUser(user))
                return NotFound("User not found.");

            post.UserId = user!.UserId;
            post.PostDate = DateTime.Now;

            _context.Posts!.Add(post);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetPost",
                new { id = post.PostId }, post);
        }

        [HttpGet("{id:Guid}", Name = "GetPost")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Post>> GetById(Guid id)
        {
            var post = _context.Posts!.AsNoTracking().FirstOrDefault(post => post.PostId == id);
            if (post is null)
                return NotFound("Post not found.");

            return Ok(post);
        }

        [HttpGet("last", Name = "GetLastPost")]
        [AllowAnonymous]
        public ActionResult<Post> GetLastPost()
        {
            var post = _context.Posts!.OrderBy(x => x.PostId).LastOrDefault();
            if (post is null)
                return NotFound("Post not found.");

            return Ok(post);
        }


    }
}
