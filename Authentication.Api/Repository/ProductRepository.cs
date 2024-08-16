using Authentication.Api.JwtToken;
using Authentication.Contracts.Forms;
using Authentication.Contracts.Responses;
using Authentication.Dal.AppContext;
using Authentication.Dal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Authentication.Repository
{
    public class ProductRepository : IApplicationInterface
    {
        private readonly AccountDbContext _context;
        private readonly JwtToken _token;
        public ProductRepository(AccountDbContext context, JwtToken token)
        {
            _context = context;
            _token = token;
        }
        public bool IsUnique(string username)
        {
            var user = _context.Costumers.FirstOrDefault(x => x.Name == username);

            if (user == null)
            {
                return true;
            }
            return false;
        }
        public async Task<RegisterResponse> Register(UserRegisterForm form)
        {
            var user = await _context.Costumers.FirstOrDefaultAsync(x => x.Name.ToLower() == form.Name.ToLower());

            if (user != null)
            {
                return new RegisterResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "User with this name already exists!"
                };
            }
            user = new ApplicationUserDal
            {
                SurName = form.Name,
                Name = form.Name,
                Password = form.Password,
                Role = form.Role
            };
            _context.Costumers.Add(user);
            await _context.SaveChangesAsync();

            var token = _token.Create();
            return new RegisterResponse
            {
                StatusCode = 201,
                Success = true,
                Message = "User successfully registered.",
                Token = token
            };
        }
        public async Task<RegisterResponse> Login(LoginForm loginRequest)
        {
            var user = await _context.Costumers.FirstOrDefaultAsync(x => x.Name == loginRequest.UserName);

            if (user == null)
            {
                return new RegisterResponse
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "User not found!"
                };
            }
            var token = _token.Create();
            return new RegisterResponse { Success = true, Token = token };
        }
    }
}
