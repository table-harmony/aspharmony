﻿using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories {
    public class UserRepository : IUserRepository {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context) {
            _context = context;
        }

        public async Task<User> GetByIdAsync(int id) {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new Exception("User not found");

            return user;
        }

        public async Task<IEnumerable<User>> GetAllAsync() {
            return await _context.Users.ToListAsync();
        }

        public async Task CreateAsync(User user) {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user) {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            User user = await GetByIdAsync(id);

            if (user == null)
                throw new Exception("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
