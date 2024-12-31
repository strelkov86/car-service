namespace SibintekTask.API.Contracts
{
    public record CreateVehicleRequest(string NumberPlate, int MarkId);
    public record UpdateVehicleRequest(string NumberPlate, int MarkId);
}
