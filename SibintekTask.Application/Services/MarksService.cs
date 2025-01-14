using AutoMapper;
using SibintekTask.Application.DTO;
using SibintekTask.Application.Interfaces;
using SibintekTask.Core.Interfaces;
using SibintekTask.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SibintekTask.Application.Services
{
    public class MarksService : IMarksService
    {
        private readonly IMarksRepository _marks;
        private readonly IMapper _mapper;
        public MarksService(IMarksRepository r, IMapper m)
        {
            _marks = r;
            _mapper = m;
        }

        public async Task<MarkDTO> Create(MarkDTO dto)
        {
            var exists = await _marks.GetByName(dto.Name);
            if (exists is not null) return _mapper.Map<MarkDTO>(exists);

            var model = _mapper.Map<Mark>(dto);
            var mark = await _marks.Create(model);
            return _mapper.Map<MarkDTO>(mark);
        }

        public async Task<int> Delete(int id)
        {
            return await _marks.Delete(id);
        }

        public async Task<IEnumerable<MarkDTO>> GetAll()
        {
            var marks = await _marks.GetAll();
            if (!marks.Any()) return null;
            return _mapper.Map<IEnumerable<MarkDTO>>(marks);
        }

        public async Task<MarkDTO> GetById(int id)
        {
            var mark = await _marks.GetById(id);
            return _mapper.Map<MarkDTO>(mark);
        }

        public async Task<MarkDTO> Update(MarkDTO dto)
        {
            var model = _mapper.Map<Mark>(dto);
            var mark = await _marks.Update(model);
            return _mapper.Map<MarkDTO>(mark);
        }
    }
}
