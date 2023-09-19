using PlatformWebService.Models;

namespace PlatformWebService.Data
{
    public interface IPlatformRepo
    {
        bool SaveChanges();

        IEnumerable<Platform> GetAllPlatForm();
        Platform GetPlatformById(int id);
        void CreatePlatform(Platform platform);

    }
}
