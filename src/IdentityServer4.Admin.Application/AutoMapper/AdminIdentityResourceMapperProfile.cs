using AutoMapper;
using ByLearning.Admin.Application.ViewModels.IdentityResourceViewModels;
using ByLearning.Admin.Domain.Commands.IdentityResource;
using ByLearning.DDD.Domain.Core.ViewModels;
using IdentityServer4.Models;
using System.Security.Claims;

namespace ByLearning.Admin.Application.AutoMapper
{
    public class AdminIdentityResourceMapperProfile : Profile
    {
        public AdminIdentityResourceMapperProfile()
        {
            CreateMap<IdentityResource, IdentityResourceListView>(MemberList.Destination);
            CreateMap<Claim, ClaimViewModel>().ConstructUsing(a => new ClaimViewModel(a.Type, a.Value));

            /*
             * Identity Resource commands
             */
            CreateMap<IdentityResource, RegisterIdentityResourceCommand>().ConstructUsing(c => new RegisterIdentityResourceCommand(c));
            CreateMap<RemoveIdentityResourceViewModel, RemoveIdentityResourceCommand>().ConstructUsing(c => new RemoveIdentityResourceCommand(c.Name));

        }
    }
}