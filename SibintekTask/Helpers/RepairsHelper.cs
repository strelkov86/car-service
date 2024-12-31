using SibintekTask.Application.DTO;
using SibintekTask.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace SibintekTask.API.Helpers
{
    public static class RepairsHelper
    {
        public static object PrepareRepairResponse(RepairDTO repair)
        {
            return new
            {
                repair.Id,
                repair.Cost,
                repair.RepairType,
                Vehicle = new { repair.Vehicle.Id, repair.Vehicle.NumberPlate, repair.Vehicle.Mark },
                repair.Mileage,
                Customer = new { repair.CustomerId, repair.Customer.Name, repair.Customer.Surname, repair.Customer.ITN },
                Executor = new { repair.ExecutorId, repair.Executor.Name, repair.Executor.Surname, repair.Executor.ITN },
            };
        }

        public static object PrepareRepairListResponse(IEnumerable<RepairDTO> repairs)
        {
            return new
            {
                Repairs = repairs.Select(r => new
                {
                    r.Id,
                    r.Cost,
                    r.RepairType,
                    Vehicle = new { r.Vehicle.Id, r.Vehicle.NumberPlate, r.Vehicle.Mark },
                    r.Mileage,
                    Customer = new { r.CustomerId, r.Customer.Name, r.Customer.Surname, r.Customer.ITN },
                    Executor = new { r.ExecutorId, r.Executor.Name, r.Executor.Surname, r.Executor.ITN },
                })                
            };
        }
    }
}
