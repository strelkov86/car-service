using SibintekTask.Core.Models;

namespace SibintekTask.Application.Auth
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
