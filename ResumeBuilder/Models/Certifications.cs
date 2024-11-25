using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models
{
    public class Certifications
    {
        [Key] // Specifies that this property is the primary key
        public int CertificationID { get; set; }  // Auto-generated key
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Issuer { get; set; }
    }
}
