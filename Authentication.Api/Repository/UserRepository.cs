using Authentication.Api.JwtTokenProviders;
using Authentication.Contracts.Forms;
using Authentication.Contracts.Responses;
using Authentication.Dal.AppContext;
using Authentication.Dal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Authentication.Api.Repository
{
    public class UserRepository(JwtTokenProvider provider, RoleManager<IdentityRole<Guid>> roleManager, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        : IApplicationInterface
    {
        private readonly JwtTokenProvider _JwtProvider = provider;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public bool IsUnique(string email)
        {
            var user = _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return true;
            }
            return false;
        }
        public async Task<RegisterResponse> Register(UserRegisterForm form)
        {
            var user = await _userManager.FindByNameAsync(form.Email);

            if (user != null)
            {
                return new RegisterResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "User with this name already exists!"
                };
            }
            user = new ApplicationUser
            {
                UserName = form.Email,
                FirstName = form.FistName,
                LastName = form.LastName,
                Email = form.Email
            };

            var result = await _userManager.CreateAsync(user, form.Password);

            if (!result.Succeeded)
            {
                return new RegisterResponse
                {
                    Message = result.Errors
                    .Select(e => "Error: " + e.Description + " code: " + e.Code + " ,")
                    .Aggregate((e, delimiter) => e + delimiter),
                    StatusCode = 400,
                    Success = false
                };
            }
            var roleresult = await _roleManager.RoleExistsAsync("user");
            if (!roleresult)
                await _roleManager.CreateAsync(new IdentityRole<Guid>("user"));

            var addtoroleresult = await _userManager.AddToRoleAsync(user, "user");
            if (!addtoroleresult.Succeeded)
            {
                return new RegisterResponse
                {
                    Message = result.Errors
                    .Select(e => "Error: " + e.Description + " code: " + e.Code + " ,")
                    .Aggregate((e, delimiter) => e + delimiter),
                    StatusCode = 50000,
                    Success = false
                };
            }
            var UserToken = _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "UserToken");
            var AuthenticationToken = _userManager.GetAuthenticationTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider, "UserToken");
            var token = _JwtProvider.Create("user", user.Email);

            var tokenresult = await _userManager.SetAuthenticationTokenAsync(user, "AuthenticationToken", "Bearer", token);
            if (!tokenresult.Succeeded)
            {
                return new RegisterResponse
                {
                    Message = result.Errors
                    .Select(e => "Error: " + e.Description + " code: " + e.Code + " ,")
                    .Aggregate((e, delimiter) => e + delimiter),
                    StatusCode = 400,
                    Success = false
                };
            }
            await GenerateEmailConfirmationToken(user);
            return new RegisterResponse
            {
                StatusCode = 201,
                Success = true,
                Message = $"User {user.UserName} successfully registered.",
                //Token = token
            };
        }

        /// <summary>
        /// Using SignIn manager
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<RegisterResponse> Login(LoginForm form)
        {
            var user = await _userManager.FindByEmailAsync(form.UserName);

            if (user == null)
            {
                return new RegisterResponse
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "User with this name Not Found!"
                };
            }
            var result = await _signInManager.PasswordSignInAsync(form.UserName, form.Password, isPersistent: true, lockoutOnFailure: false);

            if (!result.Succeeded)
                return new RegisterResponse { Success = false };

            var token = await _userManager.GetAuthenticationTokenAsync(user, "AuthenticationToken", "Bearer");
            if (token == null)
                return new RegisterResponse { Success = false, StatusCode = 400 };

            return new RegisterResponse { Success = true, Token = token };
        }

        /// <summary>
        /// SignIn Manager
        /// </summary>
        /// <returns></returns>
        public async Task<AppResposne> Logout()
        {
            await _signInManager.SignOutAsync();

            return new AppResposne { Success = true };
        }

        /// <summary>
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>

        public async Task<RegisterResponse> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new RegisterResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "User not found!"
                };
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return new RegisterResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = result.Errors
                    .Select(e => "Error: " + e.Description + " code: " + e.Code + " ,")
                    .Aggregate((e, delimiter) => e + delimiter),
                };
            }
            return new RegisterResponse
            {
                StatusCode = 200,
                Success = true,
                Message = "Email confirmed successfully!"
            };
        }
        public async Task<bool> GenerateEmailConfirmationToken(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return await SendEmailConfirmationToken(token, user.Email);
        }
        public async Task<bool> SendEmailConfirmationToken(string token, string email)
        {
            return await Task.FromResult(true);
        }
    }
}