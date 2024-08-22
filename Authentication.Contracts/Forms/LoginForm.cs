using System.ComponentModel.DataAnnotations;

namespace Authentication.Contracts.Forms
{
    public record LoginForm
    {

        [DataType(DataType.EmailAddress)]
        public required string UserName { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
