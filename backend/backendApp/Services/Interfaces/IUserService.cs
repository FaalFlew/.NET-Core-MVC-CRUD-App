using backendApp.Dtos;
using backendApp.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backendApp.Services.Interfaces
{
    public interface IUserService
    {
        bool IsValidEmail(string email);
        Task<bool> IsEmailInUseAsync(string email);
        Task<User> AuthenticateUser(string username, string password);
        List<Claim> GetUserClaims(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(CreateUserRequestDto userDto);
        Task<User> UpdateUserAsync(int id, UpdateUserRequestDto userDto);
        Task<User> DeleteUserAsync(int id);
    }
}