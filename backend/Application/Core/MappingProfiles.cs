using Application.Titles;
using AutoMapper;
using Domain;

namespace Application.Core;

//Create mapping mechanism to return the correct information
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Title, Title>();
        CreateMap<Author, Author>();
        CreateMap<Title, TitleDto>()
            .ForMember(d => d.OwnerName, o => o.MapFrom(
                s => s.Owner.FirstOrDefault(x => x.Owned).User.UserName));
        CreateMap<BookOwner, Profiles.Profile>()
            .ForMember(d => d.Username, o => o.MapFrom(s => s.User.UserName))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.User.Name));
    }
    
}