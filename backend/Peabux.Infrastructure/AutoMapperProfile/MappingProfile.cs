using AutoMapper;
using Peabux.Domain.Dtos;
using Peabux.Domain.Entities;

namespace Peabux.Infrastructure.AutoMapperProfile;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegistrationDto, User>()
        //.ForMember(x => x.FirstName, y => { y.MapFrom(p => p.FirstName); })
        //.ForMember(x => x.Surname, y => { y.MapFrom(p => p.Surname); })
        //.ForMember(x => x.NationalIdNumber, y => { y.MapFrom(p => p.NationalIdNumber); })
        .ForMember(x => x.UserName, y => { y.MapFrom(p => p.NationalIdNumber); })
        .ReverseMap();

        CreateMap<RegistrationDto, Teacher>().ReverseMap();
        CreateMap<RegistrationDto, Student>().ReverseMap();
    }
}
