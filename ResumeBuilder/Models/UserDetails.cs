
using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Models
{
    public class UserDetails
    {
        [Key]
        public int DetailsID { get; set; }
        public int UserID { get; set; }  
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address{ get; set; }
        public bool Visibility { get; set; }


    }
}
