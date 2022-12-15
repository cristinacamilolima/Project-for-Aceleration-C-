using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_for_Aceleration_Csharp_Tryitter.Models
{
    public class Post
    {
        [Key]
        public Guid PostId { get; set; }
        public string Content { get; set; }
    }
}
