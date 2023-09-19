using PlatformWebService.DTOs;

namespace PlatformWebService.SyncDataServices.Http
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDtos platform);
    }
}
