﻿using BD.Repositories.Interfaces;
using BD.Repositories.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BD.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly BloodDonationDbContext _context;
        public UserRepository(BloodDonationDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(User user)
        {
            user.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                        .Include(u => u.Role)
                        .Where(f => f.IsDeleted != true)
                        .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                                .Include(u => u.Role)
                                .FirstOrDefaultAsync(f => f.UserId == id && f.IsDeleted != true);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                                .Include(u => u.Role)
                                .FirstOrDefaultAsync(f => f.Email != null && f.Email.ToLower() == email.ToLower() && f.IsDeleted != true);
        }

        public async Task<User?> GetByPhoneAsync(string phone)
        {
            return await _context.Users
                                .Include(u => u.Role)
                                .FirstOrDefaultAsync(f => f.Phone != null && f.Phone == phone && f.IsDeleted != true);
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
