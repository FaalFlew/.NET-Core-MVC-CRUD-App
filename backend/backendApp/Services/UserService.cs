using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using backendApp.Data;
using backendApp.Dtos;
using backendApp.Interfaces;
using backendApp.Mappers;
using backendApp.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using backendApp.Services.Interfaces;

namespace backendApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return emailRegex.IsMatch(email);
        }
        public async Task<bool> IsEmailInUseAsync(string email)
        {
            return (await _userRepo.GetAllAsync()).Any(u => u.Email == email);
        }
        public async Task<User> AuthenticateUser(string username, string password)
        {
            return (await _userRepo.GetAllAsync())
                .FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public List<Claim> GetUserClaims(User user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.DateOfBirth, user.DateJoined.ToString())
            };
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepo.GetAllAsync();
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepo.GetByIdAsync(id);
        }
        public async Task<User> CreateUserAsync(CreateUserRequestDto userDto)
        {
            var userModel = userDto.ToUserFromCreateDto();
            await _userRepo.CreateAsync(userModel);
            return userModel;
        }

        public async Task<User> UpdateUserAsync(int id, UpdateUserRequestDto userDto)
        {
            return await _userRepo.UpdateAsync(id, userDto);
        }

        public async Task<User> DeleteUserAsync(int id)
        {
            return await _userRepo.DeleteAsync(id);
        }
    }
}