using AutoMapper;

namespace ByLearning.SSO.Application.AutoMapper
{
    public static class GlobalConfigurationMapping
    {
        internal static IMapper Mapper { get; }
        static GlobalConfigurationMapping()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<GlobalConfigurationMapperProfile>()).CreateMapper();
        }
    }
}