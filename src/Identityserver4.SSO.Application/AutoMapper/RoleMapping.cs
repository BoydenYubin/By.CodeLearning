using AutoMapper;

namespace ByLearning.SSO.Application.AutoMapper
{
    public static class RoleMapping
    {
        internal static IMapper Mapper { get; }
        static RoleMapping()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<RoleMapperProfile>()).CreateMapper();
        }
    }
}