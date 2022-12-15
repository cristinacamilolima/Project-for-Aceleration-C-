using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Project_for_Aceleration_Csharp_Tryitter.Models
{
    public class Post
    {
        [Key]
        [JsonIgnore]
        public Guid PostId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string? Title { get; set; }
        [MaxLength(500)]
        public string? Content { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 10)]
        public string? ImageUrl { get; set; }
        
        [JsonIgnore]
        public DateTime PostDate { get; set; }
        
        [JsonIgnore]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
    }
}
