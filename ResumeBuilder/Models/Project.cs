using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models
{
    public class Projects
    {
        [Key]
        public int ProjectID { get; set; }  // Auto-generated key
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public DateTime StartDate{ get; set; }
        public DateTime EndDate{ get; set; }
        public string Description { get; set; } 
    }
}

