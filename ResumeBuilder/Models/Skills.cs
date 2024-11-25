using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models
{
    public class Skills
    {
        [Key] // Specifies that this property is the primary key
        public int SkillID { get; set; }  // Auto-generated key
        public int UserID { get; set; }
        public string Name { get; set; }
        
    }
}
