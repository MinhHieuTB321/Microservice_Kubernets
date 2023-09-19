using System.Text.Json;
using AutoMapper;
using CommandWebService.Data;
using CommandWebService.DTOs;
using CommandWebService.Models;

namespace CommandWebService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory=scopeFactory;
            _mapper=mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType=DetermineEvent(message);
            switch(eventType){
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage){
            Console.WriteLine("--> Determining Event");

            var eventType= JsonSerializer.Deserialize<GenericEventDo>(notificationMessage);

            switch(eventType!.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("-->Platform Published Event Detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }
    
        private void AddPlatform(string PlatformPublishedMessage)
        {
            using(var scope= _scopeFactory.CreateScope())
            {
                var repo= scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(PlatformPublishedMessage);

                try
                {   
                    var plat= _mapper.Map<Platform>(platformPublishedDto);
                    if(!repo.ExternalPlatformExist(plat.ExternalId))
                    {
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                        Console.WriteLine($"--> Platform added!");
                    }
                }   
                catch(Exception ex)
                {
                    Console.WriteLine($"--> Could not add Platforms  to Db {ex.Message}");
                }
            }
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}