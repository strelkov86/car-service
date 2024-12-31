namespace SibintekTask.Application.Auth
{
    public class PasswordHasher
    {
        public static string GenerateHash(string password) => BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        public static bool Verify(string password, string passwordHash) => BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
    }
}
