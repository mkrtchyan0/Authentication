using Microsoft.AspNetCore.Identity;

namespace Authentication.Dal.Models
{
    public class ApplicationUserDal : IdentityUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
