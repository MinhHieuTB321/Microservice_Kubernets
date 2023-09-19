using AutoMapper;
using CommandWebService.Data;
using CommandWebService.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandWebService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _commandRepo;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepo commandRepo,IMapper mapper)
        {
            _commandRepo=commandRepo;
            _mapper=mapper;
        }

        [HttpGet]
        public IActionResult GetAllPlatform()
        {
            Console.WriteLine("--> Getting Platforms from CommandService");
            var platforms= _commandRepo.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpPost]
        public IActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound Post # Command Service");
            return Ok("Inbound test of from Platforms Controller");
        }
    }
}
