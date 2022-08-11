using AutoMapper;
using ByLearning.Admin.Application.ViewModels;
using ByLearning.Admin.Application.ViewModels.IdentityResourceViewModels;
using ByLearning.Admin.Domain.Commands.PersistedGrant;
using ByLearning.DDD.Domain.Core.ViewModels;
using IdentityServer4.Models;
using System.Security.Claims;

namespace ByLearning.Admin.Application.AutoMapper
{
    public class AdminPersistedGrantMapperProfile : Profile
    {
        public AdminPersistedGrantMapperProfile()
        {
            CreateMap<IdentityResource, IdentityResourceListView>(MemberList.Destination);
            CreateMap<Claim, ClaimViewModel>().ConstructUsing(a => new ClaimViewModel(a.Type, a.Value));
            /*
            * Persisted grant
            */
            CreateMap<RemovePersistedGrantViewModel, RemovePersistedGrantCommand>().ConstructUsing(c => new RemovePersistedGrantCommand(c.Key));



        }
    }
}