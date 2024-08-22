using System.ComponentModel.DataAnnotations;

namespace Authentication.Contracts.Forms
{
    public class UserRegisterForm
    {
        [StringLength(32)]
        public string FistName { get; set; } = string.Empty;
        [StringLength(64)]
        public string LastName { get; set; } = string.Empty;

        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
