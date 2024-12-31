using SibintekTask.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SibintekTask.Core.Interfaces
{
    public interface IVehiclesRepository
    {
        public Task<IEnumerable<Vehicle>> GetAll(CancellationToken token = default);
        public Task<Vehicle?> GetById(int id, CancellationToken token = default);
        public Task<Vehicle?> GetByNumberPlate(string numberPlate, CancellationToken token = default);
        public Task<Vehicle?> GetByPlateAndMark(string numberPlate, int markId, CancellationToken token = default);
        public Task<Vehicle> Create(Vehicle vehicle, CancellationToken token = default);
        public Task<Vehicle?> Update(Vehicle vehicle, CancellationToken token = default);
        public Task<int> Delete(int id, CancellationToken token = default);
    }
}
