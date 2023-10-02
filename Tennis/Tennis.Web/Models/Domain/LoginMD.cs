using System.ComponentModel.DataAnnotations;

namespace Tennis.Web.Models.Domain
{
    public class LoginMD
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? EmailAddress { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
