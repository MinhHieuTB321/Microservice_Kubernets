using PlatformWebService.DTOs;

namespace PlatformWebService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
    }   
}