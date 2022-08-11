using AutoMapper;
using IdentityServer4.Models;
using ByLearning.Admin.Application.AutoMapper;
using ByLearning.Admin.Application.Interfaces;
using ByLearning.Admin.Application.ViewModels.IdentityResourceViewModels;
using ByLearning.Admin.Domain.Commands.IdentityResource;
using ByLearning.Admin.Domain.Interfaces;
using ByLearning.Domain.Core.Bus.Abstract;
using ByLearning.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ByLearning.Admin.Application.Services
{
    public class IdentityResourceAppService : IIdentityResourceAppService
    {
        private IMapper _mapper;
        private IEventStoreRepository _eventStoreRepository;
        private readonly IIdentityResourceRepository _identityResourceRepository;
        public IEventBus Bus { get; set; }

        public IdentityResourceAppService(
            IEventBus bus,
            IEventStoreRepository eventStoreRepository,
            IIdentityResourceRepository identityResourceRepository)
        {
            _mapper = AdminIdentityResourceMapper.Mapper;
            Bus = bus;
            _eventStoreRepository = eventStoreRepository;
            _identityResourceRepository = identityResourceRepository;
        }


        public async Task<IEnumerable<IdentityResourceListView>> GetIdentityResources()
        {
            var resultado = await _identityResourceRepository.All();
            return _mapper.Map<IEnumerable<IdentityResourceListView>>(resultado);
        }

        public Task<IdentityResource> GetDetails(string name)
        {
            return _identityResourceRepository.GetDetails(name);
        }

        public Task Save(IdentityResource model)
        {
            var command = _mapper.Map<RegisterIdentityResourceCommand>(model);
            return Bus.SendCommand(command);
        }

        public Task Update(string resource, IdentityResource model)
        {
            var command = new UpdateIdentityResourceCommand(model, resource);
            return Bus.SendCommand(command);
        }

        public Task Remove(RemoveIdentityResourceViewModel model)
        {
            var command = _mapper.Map<RemoveIdentityResourceCommand>(model);
            return Bus.SendCommand(command);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}