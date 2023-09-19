using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformWebService.AsyncDataServices;
using PlatformWebService.Data;
using PlatformWebService.DTOs;
using PlatformWebService.Models;
using PlatformWebService.SyncDataServices.Http;

namespace PlatformWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commanDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(IPlatformRepo platformRepo,
            IMapper mapper,
            ICommandDataClient commandData,
            IMessageBusClient messageBus
            )
        {
            _platformRepo=platformRepo;
            _mapper=mapper;
            _commanDataClient=commandData;
            _messageBusClient=messageBus;
        }

        [HttpGet]
        public IActionResult GetAllPlatform()
        {
            Console.WriteLine("--> Getting Platforms ...");
            var plats= _platformRepo.GetAllPlatForm();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDtos>>(plats));
        }

        [HttpGet("{id}")]
        public IActionResult GetPlatformById(int id) 
        {
            Console.WriteLine("--> Getting Platform ...");
            var plat=_platformRepo.GetPlatformById(id);
            return Ok(_mapper.Map<PlatformReadDtos>(plat));
        }

        [HttpPost]
        public async Task<IActionResult> AddPlatform(PlatformCreateDto platformCreateDto)
        {
            Console.WriteLine("--> Creating platform . . .");
            var platform = _mapper.Map<Platform>(platformCreateDto);
            _platformRepo.CreatePlatform(platform);
            _platformRepo.SaveChanges();
            var platformReadDto = _mapper.Map<PlatformReadDtos>(platform);
            // Send Sync Message
            try
            {
                await _commanDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.InnerException}");
            }

            //Send Async message

            try
            {
                var platformPublishedDto= _mapper.Map<PlatformPublishedDto>(platformReadDto);
                platformPublishedDto.Event="Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asyncchronously: {ex.InnerException}");
            }

            return CreatedAtAction(nameof(GetPlatformById), new {id=platformReadDto.Id},platformReadDto);

        }
    }
}
