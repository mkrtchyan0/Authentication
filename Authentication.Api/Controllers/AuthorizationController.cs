using Authentication.Contracts.Forms;
using Authentication.Contracts.Responses;
using Authentication.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [ApiController]
    [Route("/")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IApplicationInterface _repository;
        public AuthorizationController(IApplicationInterface repository)
        {
            _repository = repository;
        }
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
    }
}
