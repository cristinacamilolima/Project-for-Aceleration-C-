using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_for_Aceleration_Csharp_Tryitter.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [MinLength(3), MaxLength(10)]
        public string? Name { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        public string? Email { get; set; }

        [Required]
        [MinLength(3), MaxLength(10)]
        public string? Password { get; set; }
        public bool Admin { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public User()
        {
            Posts = new List<Post>();
        }

    }
}
