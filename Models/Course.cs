using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_for_Aceleration_Csharp_Tryitter.Models
{
    public class Course
    {
        [Key]
        public Guid CourseId { get; set; }
        public string Name { get; set; }
        
    }
}
