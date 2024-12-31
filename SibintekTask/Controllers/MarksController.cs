using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SibintekTask.API.Contracts;
using SibintekTask.Application.DTO;
using SibintekTask.Application.Interfaces;
using System.Threading.Tasks;

namespace SibintekTask.API.Controllers
{
    [ApiController]
    [Route("marks")]
    [Authorize(Roles = "Manager, Executor")]
    public class MarksController : ControllerBase
    {
        private readonly IMarksService _marksService;
        public MarksController(IMarksService s)
        {
            _marksService = s;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllMarks()
        {
            var marks = await _marksService.GetAll();
            if (marks is null) return NoContent();
            return Ok(new { Marks = marks});
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetMarkById(int id)
        {
            var mark = await _marksService.GetById(id);
            if (mark is null) return NotFound($"Марка {id} автомобиля не найдена");
            return Ok(new { Mark = mark });
        }

        [HttpPost]
        public async Task<ActionResult> CreateMark([FromBody] SimpleEntityContract request)
        {
            var markDto = new MarkDTO() { Name = request.Name };
            var created = await _marksService.Create(markDto);
            return CreatedAtAction(nameof(GetMarkById), new { id = created.Id }, new { Created = created });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMark(int id, [FromBody] SimpleEntityContract request)
        {
            var markDto = new MarkDTO() { Id = id, Name = request.Name };
            var updated = await _marksService.Update(markDto);
            if (updated is null) return NotFound($"Марка {id} автомобиля не найдена");
            return Ok(new { Updated = updated });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> DeleteMark(int id)
        {
            var deleted = await _marksService.Delete(id);
            if (deleted == 0) return NotFound($"Марка {id} автомобиля не найдена");
            return Ok(new { DeletedCount = deleted });
        }
    }
}
