using AutoMapper;
using IdentityServer4.Models;
using ByLearning.Admin.Application.AutoMapper;
using ByLearning.Admin.Application.Interfaces;
using ByLearning.Admin.Application.ViewModels.ApiResouceViewModels;
using ByLearning.Admin.Domain.Commands.ApiResource;
using ByLearning.Admin.Domain.Interfaces;
using ByLearning.Domain.Core.Bus.Abstract;
using ByLearning.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByLearning.Admin.Application.Services
{
    public class ApiResourceAppService : IApiResourceAppService
    {
        private IMapper _mapper;
        private IEventStoreRepository _eventStoreRepository;
        private readonly IApiResourceRepository _apiResourceRepository;
        public IEventBus Bus { get; set; }

        public ApiResourceAppService(
            IEventBus bus,
            IEventStoreRepository eventStoreRepository,
            IApiResourceRepository apiResourceRepository)
        {
            _mapper = AdminApiResourceMapper.Mapper;
            Bus = bus;
            _eventStoreRepository = eventStoreRepository;
            _apiResourceRepository = apiResourceRepository;
        }

        public async Task<IEnumerable<ApiResourceListViewModel>> GetApiResources()
        {
            var resultado = await _apiResourceRepository.GetResources();
            return resultado.Select(s => _mapper.Map<ApiResourceListViewModel>(s)).ToList();
        }

        public Task<ApiResource> GetDetails(string name)
        {
            return _apiResourceRepository.GetResource(name);
        }

        public Task<bool> Save(ApiResource model)
        {
            var command = _mapper.Map<RegisterApiResourceCommand>(model);
            return Bus.SendCommand(command);
        }

        public Task<bool> Update(string id, ApiResource model)
        {
            var command = new UpdateApiResourceCommand(model, id);
            return Bus.SendCommand(command);

        }

        public Task<bool> Remove(RemoveApiResourceViewModel model)
        {
            var command = _mapper.Map<RemoveApiResourceCommand>(model);
            return Bus.SendCommand(command);
        }

        public Task<IEnumerable<Secret>> GetSecrets(string name)
        {
            return _apiResourceRepository.GetSecretsByApiName(name);
        }

        public Task<bool> RemoveSecret(RemoveApiSecretViewModel model)
        {
            var registerCommand = _mapper.Map<RemoveApiSecretCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task<bool> SaveSecret(SaveApiSecretViewModel model)
        {
            var registerCommand = _mapper.Map<SaveApiSecretCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task<IEnumerable<Scope>> GetScopes(string name)
        {
            return _apiResourceRepository.GetScopesByResource(name);
        }

        public Task<bool> RemoveScope(RemoveApiScopeViewModel model)
        {
            var registerCommand = _mapper.Map<RemoveApiScopeCommand>(model);
            return Bus.SendCommand(registerCommand);
        }


        public Task<bool> SaveScope(SaveApiScopeViewModel model)
        {
            var registerCommand = _mapper.Map<SaveApiScopeCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}