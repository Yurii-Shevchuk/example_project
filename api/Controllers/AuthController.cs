using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.UserDtos;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;    
        }

        [HttpPost]
        [Route("create-roles")]
        public async Task<IActionResult> CreateRoles()
        {
            var createRoles = await _authService.CreateRoles();

            return Ok(createRoles);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);

            if(result.IsSuccessful)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

         [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);

            if(result.IsSuccessful)
                return Ok(result);

            return Unauthorized(result);
        }


        
    }
}