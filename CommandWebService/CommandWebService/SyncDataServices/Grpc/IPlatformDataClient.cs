using CommandWebService.Models;

namespace CommandWebService.SyncDataClient.Grpc
{
    public interface  IPlatformDataClient
    {
        IEnumerable<Platform> ReturnAllPlatforms(); 
    }
}