using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backendApp.Data;
using backendApp.Dtos;
using backendApp.Interfaces;
using backendApp.Models;
using Microsoft.EntityFrameworkCore;

namespace backendApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateAsync(User userModel)
        {
            await _context.Users.AddAsync(userModel);
            await _context.SaveChangesAsync();
            return userModel;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> DeleteAsync(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return null;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> UpdateAsync(int id, UpdateUserRequestDto userDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return null;
            }

            user.Username = userDto.Username ?? user.Username;
            user.Email = userDto.Email ?? user.Email;
            user.DateJoined = userDto.DateJoined ?? user.DateJoined;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}