using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_for_Aceleration_Csharp_Tryitter.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [InverseProperty("User")]
        public ICollection<Post> Post { get; set; }

        [InverseProperty("User")]
        public ICollection<Course> Course { get; set; }

    }
}
