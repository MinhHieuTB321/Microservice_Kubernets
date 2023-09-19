using AutoMapper;
using CommandWebService.DTOs;
using CommandWebService.Models;
using PlatformWebService;

namespace CommandWebService.Profiles
{
    public class CommandsProfile:Profile
    {
        public CommandsProfile()
        {
            //Source -> Target
            CreateMap<Platform,PlatformReadDto>().ReverseMap();
            CreateMap<Command,CommandReadDto>().ReverseMap();
            CreateMap<CommandCreateDto,Command>();

            CreateMap<PlatformPublishedDto,Platform>()
                .ForMember(dest=> dest.ExternalId,opt=>opt.MapFrom(src=>src.Id))
                .ReverseMap();
            
            CreateMap<GrpcPlatformModel,Platform>()
                .ForMember(dest=>dest.ExternalId,opt=>opt.MapFrom(src=>src.PlatformId))
                .ForMember(dest=>dest.Name,opt=>opt.MapFrom(src=>src.Name))
                .ForMember(dest=>dest.Commands,opt=>opt.Ignore());


        }
    }
}