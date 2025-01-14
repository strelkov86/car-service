using AutoMapper;
using SibintekTask.Application.DTO;
using SibintekTask.Application.Interfaces;
using SibintekTask.Core.Interfaces;
using SibintekTask.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SibintekTask.Application.Services
{
    public class VehiclesService : IVehiclesService
    {
        private readonly IVehiclesRepository _vehicles;
        private readonly IMapper _mapper;
        public VehiclesService(IVehiclesRepository r, IMapper m)
        {
            _vehicles = r;
            _mapper = m;
        }

        public async Task<VehicleDTO> Create(VehicleDTO vehicleDto)
        {
            var exists = await _vehicles.GetByPlateAndMark(vehicleDto.NumberPlate, vehicleDto.MarkId.Value);
            if (exists is not null) return _mapper.Map<VehicleDTO>(exists);

            var model = _mapper.Map<Vehicle>(vehicleDto);
            var vehicle = await _vehicles.Create(model);
            return _mapper.Map<VehicleDTO>(vehicle);
        }

        public async Task<int> Delete(int id)
        {
            return await _vehicles.Delete(id);
        }

        public async Task<IEnumerable<VehicleDTO>> GetAll()
        {
            var vehicles = await _vehicles.GetAll();
            return _mapper.Map<IEnumerable<VehicleDTO>>(vehicles);
        }

        public async Task<VehicleDTO> GetById(int id)
        {
            var vehicle = await _vehicles.GetById(id);
            return _mapper.Map<VehicleDTO>(vehicle);
        }

        public async Task<VehicleDTO> GetByNumberPlate(string numberPlate)
        {
            var vehicle = await _vehicles.GetByNumberPlate(numberPlate);
            return _mapper.Map<VehicleDTO>(vehicle);
        }

        public async Task<VehicleDTO> Update(VehicleDTO vehicleDto)
        {
            var model = _mapper.Map<Vehicle>(vehicleDto);
            var vehicle = await _vehicles.Update(model);
            return _mapper.Map<VehicleDTO>(vehicle);
        }
    }
}
