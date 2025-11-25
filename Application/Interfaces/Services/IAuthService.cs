using Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string email, string password);
    }
}
