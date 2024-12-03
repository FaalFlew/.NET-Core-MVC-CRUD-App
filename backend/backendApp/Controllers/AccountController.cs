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
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepo;
        private readonly UserService _userService;


        public AccountController(ApplicationDbContext context, IUserRepository userRepo, UserService userService)
        {
            _context = context;
            _userRepo = userRepo;
            _userService = userService;
        }

        // login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == loginDto.Username && u.Password == loginDto.Password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.DateOfBirth, user.DateJoined.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return Ok(new { message = "Login success" });
            }

            return Unauthorized(new { message = "Inncorrect username or password" });
        }

        // Logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logout success" });
        }

        // Validate session
        [HttpGet("validate-session")]
        public IActionResult ValidateSession()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Ok(new { message = "Session is valid" });
            }

            return Unauthorized(new { message = "Session is not valid" });
        }

        // Get all users
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {

            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { message = "User  not authenticated" });
            }

            var users = await _userRepo.GetAllAsync();

            return Ok(users);
        }

        // Add user
        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto userDto)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            if (string.IsNullOrEmpty(userDto.Username) || string.IsNullOrEmpty(userDto.Password) || string.IsNullOrEmpty(userDto.Email))
            {
                return BadRequest(new { message = " All Username, password,  email required." });
            }

            var existingUser = await _userRepo.GetByEmailAsync(userDto.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Email is already in use." });
            }

            if (!_userService.IsValidEmail(userDto.Email))
            {
                return BadRequest(new { message = "Invalid email address." });
            }

            var userModel = userDto.ToUserFromCreateDto();
            await _userRepo.CreateAsync(userModel);
            return CreatedAtAction(nameof(GetById), new { id = userModel.Id }, userModel);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var user = await _userRepo.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // Edit user
        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequestDto userDto)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { message = "User is authenticated" });
            }

            var user = await _userRepo.UpdateAsync(id, userDto);
            if (user == null)
            {
                return NotFound(new { message = "User can not found" });
            }


            return Ok(new { message = "User updated success" });
        }

        // Delete  user
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var user = await _userRepo.DeleteAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User is not found" });
            }

            return Ok(new { message = "User deleted success" });
        }
    }
}
