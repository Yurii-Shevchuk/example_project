using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.UserDtos;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _config = config;
            _roleManager = roleManager;
            _userManager = userManager;
            
        }
        public async Task<AuthResponseDto> CreateRoles()
        {
            bool isUser = await _roleManager.RoleExistsAsync("user");

            if(isUser)
            {
                return new AuthResponseDto()
                {
                    IsSuccessful = true,
                    Message = "Role is already in the db"
                };
            }

            await _roleManager.CreateAsync(new IdentityRole("user"));

            return new AuthResponseDto()
                {
                    IsSuccessful = true,
                    Message = "Seeding successful"
                };
        }

        public async Task<bool> IsRealUser(string username)
        {
            return await _userManager.FindByNameAsync(username) != null;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if(user is null)
            {
                return new AuthResponseDto()
                {
                    IsSuccessful = false,
                    Message = "Invalid UserName"
                };
            }

            var isCorrectPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if(!isCorrectPassword)
            {
                return new AuthResponseDto()
                {
                    IsSuccessful = false,
                    Message = "Invalid Password"
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTID", Guid.NewGuid().ToString()),
            };

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = CreateJWT(claims);

            return new AuthResponseDto()
            {
                IsSuccessful = true,
                Message = token
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var isUserExists = await _userManager.FindByNameAsync(registerDto.UserName);

            if(isUserExists is not null)
            {
                return new AuthResponseDto()
                {
                    IsSuccessful = false,
                    Message = "UserName is not available"
                };
            }

            AppUser newUser = new AppUser()
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var tryToCreateUser = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!tryToCreateUser.Succeeded)
            {
                string errors = "We couldn't create a user:";
                foreach(var err in tryToCreateUser.Errors)
                {
                    errors += $"# {err.Description} ";
                }
                return new AuthResponseDto()
                {
                    IsSuccessful = false,
                    Message = errors
                };
            }

            await _userManager.AddToRoleAsync(newUser, "user");

            return new AuthResponseDto()
            {
                IsSuccessful = true,
                Message = "User created"
            };
        }

        private string CreateJWT(List<Claim> claims)
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));

            var tokenObj = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: claims,
                signingCredentials: new SigningCredentials(secret, SecurityAlgorithms.HmacSha512)
            );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObj);

            return token;
        }
    }
}