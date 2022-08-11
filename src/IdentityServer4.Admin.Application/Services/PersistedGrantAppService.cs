using AutoMapper;
using ByLearning.Admin.Application.AutoMapper;
using ByLearning.Admin.Application.Interfaces;
using ByLearning.Admin.Application.ViewModels;
using ByLearning.Admin.Domain.Commands.PersistedGrant;
using ByLearning.Admin.Domain.Interfaces;
using ByLearning.DDD.Domain.Core.ViewModels;
using ByLearning.Domain.Core.Bus.Abstract;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ByLearning.Admin.Application.Services
{
    public class PersistedGrantAppService : IPersistedGrantAppService
    {
        private IMapper _mapper;
        private readonly IPersistedGrantRepository _persistedGrantRepository;
        public IEventBus Bus { get; set; }

        public PersistedGrantAppService(
            IEventBus bus,
            IPersistedGrantRepository persistedGrantRepository)
        {
            _mapper = AdminPersistedGrantMapper.Mapper;
            Bus = bus;
            _persistedGrantRepository = persistedGrantRepository;
        }

        public async Task<ListOf<PersistedGrantViewModel>> GetPersistedGrants(IPersistedGrantCustomSearch search)
        {
            var resultado = await _persistedGrantRepository.Search(search);
            var total = await _persistedGrantRepository.Count(search);

            var grants = resultado.Select(s => new PersistedGrantViewModel(s.Key, s.Type, s.SubjectId, s.ClientId, s.CreationTime, s.Expiration, s.Data));
            return new ListOf<PersistedGrantViewModel>(grants, total);
        }

        public Task Remove(RemovePersistedGrantViewModel model)
        {
            // kiss
            var command = _mapper.Map<RemovePersistedGrantCommand>(model);
            return Bus.SendCommand(command);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}