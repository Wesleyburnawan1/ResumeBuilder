namespace ResumeBuilder.Models
{
    public class Education
    {
        public int EducationID { get; set; }  // Auto-generated key
        public int UserID { get; set; }
        public string InstituteName { get; set; }
        public string Degree { get; set; }
        public int StartingYear { get; set; }
        public int EndingYear { get; set; }
        public decimal GPA { get; set; } 
    }
}
