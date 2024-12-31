using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SibintekTask.Application.Interfaces;

namespace SibintekTask.API.Controllers
{
    [ApiController]
    [Route("reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService _reportsService;
        public ReportsController(IReportsService r)
        {
            _reportsService = r;
        }

        [HttpGet("customers/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> GetCustomerReport(int id, DateTime? startDate, DateTime? endDate)
        {
            var report = await _reportsService.GetReportByCustomerId(id, startDate, endDate);
            if (report is null) return NotFound($"Ремонтных данных по запрашиваемому заказчику {id} не найдено. Проверьте выбранного заказчика/даты");
            return Ok(new { Report = report });
        }


        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> GetTotalReport(DateTime? startDate, DateTime? endDate)
        {
            var report = await _reportsService.GetTotalReport(startDate, endDate);
            if (report is null) return NotFound($"Ремонтных данных не найдено. Проверьте вводимые даты");
            return Ok(new { Report = report });
        }


        [HttpGet("executors/{id}")]
        [Authorize(Roles = "Manager, Executor")]
        public async Task<ActionResult> GetExecutorReport(int id, DateTime? startDate, DateTime? endDate)
        {
            var (currentUserId, isManager) = GetUserInfo();

            if (!IsAuthorized(id, currentUserId, isManager)) return Forbid();

            var report = await _reportsService.GetReportByExecutorId(id, startDate, endDate);
            if (report is null) return NotFound($"Ремонтных данных по запрашиваемому исполнителю {id} не найдено. Проверьте выбранного исполнителя/даты");
            return Ok(new { Report = report });
        }


        [HttpGet("vehicles/{id}")]
        [Authorize(Roles = "Manager, Customer")]
        public async Task<ActionResult> GetVehicleReport(int id, DateTime? startDate, DateTime? endDate)
        {
            var (currentUserId, isManager) = GetUserInfo();

            if (!IsAuthorized(id, currentUserId, isManager)) return Forbid();

            var report = await _reportsService.GetReportByVehicleId(id, startDate, endDate,
                isManager ? null : currentUserId);

            if (report is null) return NotFound($"Ремонтных данных по запрашиваемому автомобилю {id} не найдено. Проверьте вводимый автомобиль/даты");

            return Ok(new { Report = report });
        }


        private (int currentUserId, bool isManager) GetUserInfo()
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = int.TryParse(userIdClaim, out int currentUserId);
            if (!result) throw new Exception("Пользователь не имеет идентификатора");

            var isManager = HttpContext.User.FindAll(ClaimTypes.Role).Any(role => role.Value == "Manager");

            return (currentUserId, isManager);
        }

        private static bool IsAuthorized(int id, int currentUserId, bool isManager)
        {
            return id == currentUserId || isManager;
        }
    }
}
