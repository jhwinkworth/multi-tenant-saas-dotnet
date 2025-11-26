using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        public User CreateUser(string email, string passwordHash, string fullName, bool isAdmin = false);
        public List<User> GetAllUsers();
        public User? GetUserById(Guid id);
        public void UpdateUser(User user);
        public void DeleteUser(User user);
    }
}
