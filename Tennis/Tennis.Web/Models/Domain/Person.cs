using System.ComponentModel.DataAnnotations;

namespace Tennis.Web.Models.Domain
{
    public class Person
    {
        // id, firstname, lastname, email, age, role
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string? FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string? LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? EmailAddress { get; set; }
        // Birth date
        [Required]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        // Profile picture
        [Required]
        [DataType(DataType.ImageUrl)]
        public string? ProfilePic { get; set; }
        //password
        [Required]
        public string? Password { get; set; }
        // Role is either admin, coach, user
        public string? Role { get; set; }

    }
}
