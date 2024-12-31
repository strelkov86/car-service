namespace SibintekTask.API.Contracts
{
    public record RegisterRequest(string Name, string Surname, string Itn, string Password = null);
    public record LoginRequest(string Itn, string Password);
    public record RegisterExistingCustomerRequest(string Itn, string Password);
}
