using AutoMapper;
using SibintekTask.Application.Auth;
using SibintekTask.Application.DTO;
using SibintekTask.Application.Interfaces;
using SibintekTask.Core.Interfaces;
using SibintekTask.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SibintekTask.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _users;
        private readonly IRolesRepository _roles;
        private readonly IMapper _mapper;
        public UsersService(IUsersRepository u, IRolesRepository r, IMapper m)
        {
            _users = u;
            _roles = r;
            _mapper = m;
        }

        public async Task<bool> GrantRoleToUser(int userId, int roleId)
        {
            return await _users.AttachRoleToUser(userId, roleId);
        }

        public async Task<UserDTO> CreateCustomer(UserDTO userDto)
        {
            var exists = await _users.GetByITN(userDto.ITN);
            if (exists != null) return null;

            var model = _mapper.Map<User>(userDto);
            var user = await _users.Create(model);

            var customerRole = await _roles.GetByName("Customer");
            var result = await _users.AttachRoleToUser(user.Id, customerRole.Id);
            if (!result) return null;

            user.Roles.Add(customerRole);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<int> Delete(int id)
        {
            return await _users.Delete(id);
        }

        public async Task<IEnumerable<UserDTO>?> GetAll()
        {
            var users = await _users.GetAll();
            if (!users.Any()) return null;
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO> GetById(int id)
        {
            var user = await _users.GetById(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> GetByITN(string itn)
        {
            var user = await _users.GetByITN(itn);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<IEnumerable<UserDTO>?> GetByRole(int roleId)
        {
            var users = await _users.GetByRole(roleId);
            if (users is null) return null;
            if (!users.Any()) return Array.Empty<UserDTO>();
            return _mapper.Map<IEnumerable<UserDTO>?>(users);
        }

        public async Task<bool> RevokeRoleFromUser(int userId, int roleId)
        {
            return await _users.RemoveRoleFromUser(userId, roleId);
        }

        public async Task<UserDTO> Update(UserDTO userDto)
        {
            var model = _mapper.Map<User>(userDto);
            var user = await _users.Update(model);
            return _mapper.Map<UserDTO>(user);
        }
    }
}
