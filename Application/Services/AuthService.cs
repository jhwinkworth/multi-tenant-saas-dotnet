using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public AuthService(IUserRepository userRepo, IJwtTokenGenerator tokenGenerator)
    {
        _userRepo = userRepo;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await _userRepo.GetByEmailAsync(email);
        if (user == null || !VerifyPassword(password, user.PasswordHash))
            return null;

        return _tokenGenerator.GenerateToken(user);
    }

    private bool VerifyPassword(string password, string hash)
    {
        // TODO: replace with real hash verification (BCrypt recommended)
        return password == hash;
    }
}
