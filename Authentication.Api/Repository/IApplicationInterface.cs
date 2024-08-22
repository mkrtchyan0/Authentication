using Authentication.Contracts.Forms;
using Authentication.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Repository
{
    public interface IApplicationInterface
    {
        bool IsUnique(string username);
        public Task<RegisterResponse> Login(LoginForm form);
        public Task<RegisterResponse> Register(UserRegisterForm registrationRequest);
        public Task<RegisterResponse> ConfirmEmailAsync(string email, string token);
        public Task<AppResposne> Logout();
    }
}
