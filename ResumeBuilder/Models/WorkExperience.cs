namespace ResumeBuilder.Models
{
    public class WorkExperience
    {
        public int WorkExperienceID { get; set; }  // Auto-generated key
        public int UserID { get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; } 
    }
}

