using SibintekTask.Application.DTO;
using System.Collections.Generic;
using System.Linq;

namespace SibintekTask.API.Helpers
{
    public static class VehiclesHelper
    {
        public static object PrepareVehicleListResponse(IEnumerable<VehicleDTO> vehicles)
        {
            return new
            {
                Vehicles = vehicles.Select(v => new
                {
                    v.Id,
                    v.NumberPlate,
                    v.Mark,
                })
            };
        }
        public static object PrepareVehicleResponse(VehicleDTO vehicle)
        {
            return new
            {
                vehicle.Id,
                vehicle.NumberPlate,
                vehicle.Mark,
            };
        }
    }
}
