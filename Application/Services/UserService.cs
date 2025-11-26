using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITenantProvider _tenantProvider;

        public UserService(IUserRepository userRepository, ITenantProvider tenantProvider)
        {
            _userRepository = userRepository;
            _tenantProvider = tenantProvider;
        }

        public User CreateUser(string email, string password, string fullName, bool isAdmin = false)
        {
            var hashedPassword = HashPassword(password);
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = hashedPassword,
                FullName = fullName,
                IsAdmin = isAdmin,
                TenantId = _tenantProvider.TenantId
            };

            return _userRepository.Add(user);
        }

        public List<User> GetAllUsers() =>
            _userRepository.GetAllForTenant(_tenantProvider.TenantId);

        public User? GetUserById(Guid id) =>
            _userRepository.GetById(id, _tenantProvider.TenantId);

        public void UpdateUser(User user)
        {
            // Optional: validation (email format, role)
            _userRepository.Update(user);
        }

        public void DeleteUser(User user)
        {
            _userRepository.Delete(user);
        }

        public string HashPassword(string password)
        {
            // Implement a secure password hashing mechanism here
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password)); // Placeholder example
        }
    }
}
