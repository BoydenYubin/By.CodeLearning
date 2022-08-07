using AutoMapper;
using ByLearning.SSO.Application.ViewModels.RoleViewModels;
using ByLearning.SSO.Domain.Commands.Role;
using ByLearning.SSO.Domain.Models;

namespace ByLearning.SSO.Application.AutoMapper
{
    public class RoleMapperProfile : Profile
    {
        public RoleMapperProfile()
        {
            /*
              * Role commands
              */
            CreateMap<RemoveRoleViewModel, RemoveRoleCommand>().ConstructUsing(c => new RemoveRoleCommand(c.Name));
            CreateMap<SaveRoleViewModel, SaveRoleCommand>().ConstructUsing(c => new SaveRoleCommand(c.Name));
            CreateMap<RemoveUserFromRoleViewModel, RemoveUserFromRoleCommand>().ConstructUsing(c => new RemoveUserFromRoleCommand(c.Role, c.Username));

            /*
             * Domain to view model
             */
            CreateMap<Role, RoleViewModel>(MemberList.Destination);
        }
    }
}
