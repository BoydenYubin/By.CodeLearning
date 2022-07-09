using AutoMapper;

namespace ByLearningAutoMapper.IOC
{
    public class ProfileClass : Profile
    {
        public ProfileClass()
        {
            CreateMap<SourceClass, Destination>()
                .ForMember(s => s.DName, opt => opt.MapFrom(s => s.Name))
                .ForMember(s => s.DAge, opt => opt.MapFrom(s => s.Age))
                .ReverseMap();
        }
    }
}
