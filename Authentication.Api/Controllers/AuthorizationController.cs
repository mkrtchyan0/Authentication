using Authentication.Contracts.Forms;
using Authentication.Contracts.Responses;
using Authentication.Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [ApiController]
    [Route("/")]
    public class AuthorizationController(IApplicationInterface repository) : ControllerBase
    {
        private readonly IApplicationInterface _repository = repository;

        [HttpPost]
        [Route("register")]
        public async Task<RegisterResponse> Register(UserRegisterForm register)
        {
            return await _repository.Register(register);
        }
        [HttpPost]
        [Route("login")]
        public async Task<RegisterResponse> Login(LoginForm loginRequest)
        {
            return await _repository.Login(loginRequest);
        }
        [HttpPost]
        [Route("confirmemail")]
        public async Task<RegisterResponse> ConfirmEmail(string email, string token)
        {
            return await _repository.ConfirmEmailAsync(email, token);
        }
    }
}
