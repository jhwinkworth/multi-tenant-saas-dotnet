using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAllForTenant(Guid tenantId);
        User? GetById(Guid id, Guid tenantId);
        User Add(User user);
        void Update(User user);
        void Delete(User user);

        Task<User?> GetByEmailAsync(string email);
    }
}
