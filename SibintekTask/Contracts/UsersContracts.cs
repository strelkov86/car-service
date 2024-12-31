using System.ComponentModel.DataAnnotations;

namespace SibintekTask.API.Contracts
{
    public record UpdateUserRequest(string Name, string Surname, string Itn);
    public record CreateCustomerRequest([Required] string Name, [Required] string Surname, [Required] string Itn);
}
