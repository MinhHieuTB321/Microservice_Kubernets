using PlatformWebService.Models;

namespace PlatformWebService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _appDbContext;
        public PlatformRepo(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }
            _appDbContext.Platforms.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatForm()
        {
            return _appDbContext.Platforms.ToList();
        }

        public Platform GetPlatformById(int id)
        {
            var result= _appDbContext.Platforms.FirstOrDefault(x => x.Id == id);
            if (result == null) throw new NullReferenceException();
            return result;
        }

        public bool SaveChanges()
        {
            return _appDbContext.SaveChanges()>0;
        }
    }
}
