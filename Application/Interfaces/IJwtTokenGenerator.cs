namespace Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Domain.Entities.User user);
    }
}
