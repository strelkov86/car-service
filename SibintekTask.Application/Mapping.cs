using AutoMapper;
using SibintekTask.Application.DTO;
using SibintekTask.Core.Models;

namespace SibintekTask.Application
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Mark, MarkDTO>().ReverseMap();
            CreateMap<Mark, MarkDetails>().ReverseMap();

            CreateMap<Repair, RepairDTO>().ReverseMap();

            CreateMap<RepairType, RepairTypeDTO>().ReverseMap();
            CreateMap<RepairType, RepairTypeDetails>().ReverseMap();

            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Role, RoleDetails>().ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserDetails>().ReverseMap();

            CreateMap<Vehicle, VehicleDTO>().ReverseMap();
            CreateMap<Vehicle, VehicleDetails>().ReverseMap();
        }
    }
}
