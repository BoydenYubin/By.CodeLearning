using AutoMapper;
using ByLearning.Domain.Core.Bus.Abstract;
using ByLearning.Domain.Core.Interfaces;
using ByLearning.SSO.Application.AutoMapper;
using ByLearning.SSO.Application.Interfaces;
using ByLearning.SSO.Application.ViewModels.RoleViewModels;
using ByLearning.SSO.Domain.Commands.Role;
using ByLearning.SSO.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ByLearning.SSO.Application.Services
{
    public class RoleManagerAppService : IRoleManagerAppService
    {
        private IEventStoreRepository _eventStoreRepository;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public IEventBus Bus { get; set; }
        public RoleManagerAppService(
            IRoleService roleService,
            IEventBus bus,
            IEventStoreRepository eventStoreRepository
        )
        {
            _mapper = RoleMapping.Mapper;
            _roleService = roleService;
            Bus = bus;
            _eventStoreRepository = eventStoreRepository;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<RoleViewModel>> GetAllRoles()
        {
            return _mapper.Map<IEnumerable<RoleViewModel>>(await _roleService.GetAllRoles());
        }

        public Task Remove(RemoveRoleViewModel model)
        {
            var command = _mapper.Map<RemoveRoleCommand>(model);
            return Bus.SendCommand(command);
        }

        public async Task<RoleViewModel> GetDetails(string name)
        {
            return _mapper.Map<RoleViewModel>(await _roleService.Details(name));
        }

        public Task Save(SaveRoleViewModel model)
        {
            var command = _mapper.Map<SaveRoleCommand>(model);
            return Bus.SendCommand(command);
        }

        public Task Update(string id, UpdateRoleViewModel model)
        {
            var command = new UpdateRoleCommand(model.Name, id);
            return Bus.SendCommand(command);
        }

        public Task RemoveUserFromRole(RemoveUserFromRoleViewModel model)
        {
            var command = _mapper.Map<RemoveUserFromRoleCommand>(model);
            return Bus.SendCommand(command);
        }
    }
}