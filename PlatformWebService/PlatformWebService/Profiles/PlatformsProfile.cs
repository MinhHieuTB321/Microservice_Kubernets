using AutoMapper;
using PlatformWebService.DTOs;
using PlatformWebService.Models;

namespace PlatformWebService.Profiles
{
    public class PlatformsProfile:Profile
    {
        public PlatformsProfile()
        {
            //Source - Target
            CreateMap<Platform,PlatformReadDtos>().ReverseMap();
            CreateMap<PlatformCreateDto,Platform>().ReverseMap();
            CreateMap<PlatformReadDtos,PlatformPublishedDto>().ReverseMap();
            CreateMap<Platform,GrpcPlatformModel>()
                .ForMember(dest=>dest.PlatformId,opt=>opt.MapFrom(src=>src.Id))
                .ForMember(dest=>dest.Name,opt=>opt.MapFrom(src=>src.Name))
                .ForMember(dest=>dest.Publisher,opt=>opt.MapFrom(src=>src.Publisher));
        }
    }
}
