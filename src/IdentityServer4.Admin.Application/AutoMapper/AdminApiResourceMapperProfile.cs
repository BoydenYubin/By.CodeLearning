using AutoMapper;
using ByLearning.Admin.Application.ViewModels.ApiResouceViewModels;
using ByLearning.Admin.Domain.Commands;
using ByLearning.Admin.Domain.Commands.ApiResource;
using ByLearning.DDD.Domain.Core.ViewModels;
using IdentityServer4.Models;
using System.Security.Claims;

namespace ByLearning.Admin.Application.AutoMapper
{
    public class AdminApiResourceMapperProfile : Profile
    {
        public AdminApiResourceMapperProfile()
        {
            CreateMap<ApiResource, ApiResourceListViewModel>();
            CreateMap<Claim, ClaimViewModel>().ConstructUsing(a => new ClaimViewModel(a.Type, a.Value));
            /*
              * Api Resource commands
              */
            CreateMap<ApiResource, RegisterApiResourceCommand>().ConstructUsing(c => new RegisterApiResourceCommand(c));
            CreateMap<ApiResource, ApiResourceListViewModel>();

            CreateMap<UpdateApiResourceViewModel, UpdateApiResourceCommand>().ConstructUsing(c => new UpdateApiResourceCommand(c, c.OldApiResourceId));
            CreateMap<RemoveApiResourceViewModel, RemoveApiResourceCommand>().ConstructUsing(c => new RemoveApiResourceCommand(c.Name));
            CreateMap<SaveApiSecretViewModel, SaveApiSecretCommand>().ConstructUsing(c => new SaveApiSecretCommand(c.ResourceName, c.Description, c.Value, c.Type, c.Expiration, (int)c.Hash.GetValueOrDefault(HashType.Sha256)));
            CreateMap<RemoveApiSecretViewModel, RemoveApiSecretCommand>().ConstructUsing(c => new RemoveApiSecretCommand(c.Type, c.Value, c.ResourceName));
            CreateMap<RemoveApiScopeViewModel, RemoveApiScopeCommand>().ConstructUsing(c => new RemoveApiScopeCommand(c.Name, c.ResourceName));
            CreateMap<SaveApiScopeViewModel, SaveApiScopeCommand>().ConstructUsing(c => new SaveApiScopeCommand(c.ResourceName, c.Name, c.Description, c.DisplayName, c.Emphasize, c.ShowInDiscoveryDocument, c.UserClaims));
        }
    }
}