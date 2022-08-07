using AutoMapper;
using ByLearning.SSO.Application.ViewModels;
using ByLearning.SSO.Domain.Commands.GlobalConfiguration;
using ByLearning.SSO.Domain.Models;

namespace ByLearning.SSO.Application.AutoMapper
{
    public class GlobalConfigurationMapperProfile : Profile
    {
        public GlobalConfigurationMapperProfile()
        {

            /*
             * Global configuration commands
             */
            CreateMap<ConfigurationViewModel, ManageConfigurationCommand>().ConstructUsing(c => new ManageConfigurationCommand(c.Key, c.Value, c.IsSensitive, c.IsPublic));

            CreateMap<GlobalConfigurationSettings, ConfigurationViewModel>(MemberList.Destination).ForMember(m => m.IsSensitive, opt => opt.MapFrom(src => src.Sensitive));

        }
    }
}