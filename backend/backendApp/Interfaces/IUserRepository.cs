using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backendApp.Dtos;
using backendApp.Models;

namespace backendApp.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<List<User>> GetAllAsync();
        Task<User> CreateAsync(User userModel);
        Task<User?> UpdateAsync(int id, UpdateUserRequestDto userDto);
        Task<User?> DeleteAsync(int id);




    }
}