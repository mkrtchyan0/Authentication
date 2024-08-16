using Authentication.Contracts.Forms;
using Authentication.Contracts.Responses;

namespace Authentication.Repository
{
    public interface IApplicationInterface
    {
        bool IsUnique(string username);
        public Task<RegisterResponse> Login(LoginForm form);
        public Task<RegisterResponse> Register(UserRegisterForm registrationRequest);
    }
}
