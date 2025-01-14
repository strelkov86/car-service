using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SibintekTask.API.Contracts;
using SibintekTask.Application.DTO;
using SibintekTask.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SibintekTask.API.Controllers
{
    [ApiController]
    [Route("users")]
    [Authorize(Roles = "Manager, Executor")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        public UsersController(IUsersService u)
        {
            _usersService = u;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _usersService.GetAll();
            if (!users.Any()) return NoContent();
            return Ok(new { Users = users });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            var user = await _usersService.GetById(id);
            return Ok(new { User = user });
        }

        [HttpGet("itn/{itn}")]
        public async Task<ActionResult<UserDTO>> GetUserByITN(string itn)
        {
            var user = await _usersService.GetByITN(itn);
            if (user is null) return NotFound($"Пользователь с ИНН {itn} не найден");
            return Ok(new { User = user });
        }

        [HttpGet("roles/{roleId}")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersByRole(int roleId)
        {
            var users = await _usersService.GetByRole(roleId);
            return Ok(new { Users = users });
        }

        [HttpPost("customer")]
        public async Task<ActionResult<UserDTO>> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            var dto = new UserDTO()
            {
                Name = request.Name,
                Surname = request.Surname,
                ITN = request.Itn
            };
            var user = await _usersService.CreateCustomer(dto);
            if (user is null) return BadRequest($"Пользователь с ИНН {request.Itn} уже существует либо ему не удалось прикрепить роль заказчика");
            return Ok(new { User = user });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            var dto = new UserDTO() { Name = request.Name, Surname = request.Surname, ITN = request.Itn };
            var updated = await _usersService.Update(dto);
            return Ok(new { Updated = updated });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var deleted = await _usersService.Delete(id);
            return Ok(new { DeletedCount = deleted });
        }

        [HttpPost("{userId}/roles/{roleId}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> GrantRoleToUser(int userId, int roleId)
        {
            var result = await _usersService.GrantRoleToUser(userId, roleId);
            if (!result) return BadRequest($"Не удалось назначить роль {roleId} пользователю {userId}. Возможно, она ему уже присвоена");
            return Ok($"Роль {roleId} назначена пользователю {userId}");
        }

        [HttpDelete("{userId}/roles/{roleId}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> RevokeRoleFromUser(int userId, int roleId)
        {
            var result = await _usersService.RevokeRoleFromUser(userId, roleId);
            if (!result) return BadRequest($"Не удалось отозвать роль {roleId} для пользователя {userId}. Возможно, у него нет данной роли. Проверьте введенные значения");
            return Ok($"Роль {roleId} отозвана у пользователя {userId}");
        }
    }
}
