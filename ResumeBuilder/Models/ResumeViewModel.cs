using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models
{
    public class ResumeViewModel
    {
        public UserDetails User { get; set; }
        public List<Education> EducationList { get; set; }
        public List<Skills> SkillsList { get; set; }
        public List<WorkExperience> WorkExperienceList { get; set; }
        public List<Projects> ProjectsList { get; set; }
        public List<Certifications> CertificationsList { get; set; }
    }

}
