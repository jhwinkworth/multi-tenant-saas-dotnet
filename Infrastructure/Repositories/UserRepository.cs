using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<User> GetAllForTenant(Guid tenantId) =>
            _dbContext.Users
                .Where(u => u.TenantId == tenantId)
                .AsNoTracking()
                .ToList();

        public User? GetById(Guid id, Guid tenantId) =>
            _dbContext.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == id && u.TenantId == tenantId);

        public User Add(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }

        public void Update(User user)
        {
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
        }

        public void Delete(User user)
        {
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users
                .IgnoreQueryFilters() // ignore tenant filter for login
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
