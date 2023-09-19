using CommandWebService.Models;

namespace CommandWebService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _dbContext;

        public CommandRepo(AppDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        public void CreateCommand(int platformId, Command command)
        {
             if(command==null){
                throw new ArgumentNullException(nameof(command));
            }
            command.PlatformId=platformId;
            _dbContext.Commands.Add(command);
        }

        public void CreatePlatform(Platform plat)
        {
            if(plat==null){
                throw new ArgumentNullException(nameof(plat));
            }
            _dbContext.Platforms.Add(plat);
        }

        public bool ExternalPlatformExist(int exteralPlatformId)
        {
             return _dbContext.Platforms.Any(p=>p.ExternalId==exteralPlatformId);
        }

        public IEnumerable<Command> GetAllCommand(int platformId)
        {
            return _dbContext.Commands
            .Where(c=>c.PlatformId==platformId)
            .OrderBy(c=>c.Platform.Name);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _dbContext.Platforms.ToList();
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _dbContext.Commands
            .FirstOrDefault(c=>c.PlatformId==platformId && c.Id==commandId)!;
        }

        public bool PlatformExists(int platformId)
        {
            return _dbContext.Platforms.Any(p=>p.Id==platformId);
        }

        public bool SaveChanges()
        {
            return _dbContext.SaveChanges()>0;
        }
    }
}