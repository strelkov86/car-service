using SibintekTask.Application.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SibintekTask.Application.Interfaces
{
    public interface IVehiclesService
    {
        public Task<IEnumerable<VehicleDTO>> GetAll();
        public Task<VehicleDTO?> GetById(int id);
        public Task<VehicleDTO?> GetByNumberPlate(string numberPlate);
        public Task<VehicleDTO> Create(VehicleDTO vehicleDto);
        public Task<VehicleDTO?> Update(VehicleDTO vehicleDto);
        public Task<int> Delete(int id);
    }
}
