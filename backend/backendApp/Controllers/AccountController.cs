using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using backendApp.Data;
using backendApp.Models;
using backendApp.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backendApp.Interfaces;
using backendApp.Mappers;
using backendApp.Services;

namespace backendApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserService _userService;

        public AccountController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userService.AuthenticateUser(loginDto.Username, loginDto.Password);
            if (user != null)
            {
                var claims = _userService.GetUserClaims(user);
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return Ok(new { message = "Login success" });
            }

            return Unauthorized(new { message = "Incorrect username or password" });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logout success" });
        }

        [HttpGet("validate-session")]
        public IActionResult ValidateSession()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Ok(new { message = "Session is valid" });
            }

            return Unauthorized(new { message = "Session is not valid" });
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto userDto)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            if (string.IsNullOrEmpty(userDto.Username) || string.IsNullOrEmpty(userDto.Password) || string.IsNullOrEmpty(userDto.Email))
            {
                return BadRequest(new { message = "All fields (username, password, email) are required" });
            }

            if (await _userService.IsEmailInUseAsync(userDto.Email))
            {
                return BadRequest(new { message = "Email is already in use" });
            }

            if (!_userService.IsValidEmail(userDto.Email))
            {
                return BadRequest(new { message = "Invalid email address" });
            }

            var user = await _userService.CreateUserAsync(userDto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequestDto userDto)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            if (await _userService.IsEmailInUseAsync(userDto.Email))
            {
                return BadRequest(new { message = "Email is already in use" });
            }

            if (!_userService.IsValidEmail(userDto.Email))
            {
                return BadRequest(new { message = "Invalid email address" });
            }

            var user = await _userService.UpdateUserAsync(id, userDto);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new { message = "User updated successfully" });
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var user = await _userService.DeleteUserAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new { message = "User deleted successfully" });
        }
    }
}
