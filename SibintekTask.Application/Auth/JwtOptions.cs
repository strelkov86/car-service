namespace SibintekTask.Application.Auth
{
    public class JwtOptions
    {
        public string Key { get; set; } = null;
        public int ExpiredHours { get; set; }
    }
}
