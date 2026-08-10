using AutoMapper;
using DPK.Mosavabat.Entities.Models;
using DPK.Mosavabat.Shared.DataTransferObjects;

namespace DPK.Mosavabat.Api;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<UserForRegistrationDto, User>();

    }
}