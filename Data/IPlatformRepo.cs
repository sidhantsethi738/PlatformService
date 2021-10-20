using PlatformService.Models;
using System.Collections.Generic;

namespace PlatformService.Data
{
    public interface IPlatformRepo
    {
        bool SaveChanges();
        IEnumerable<Platform> GetAllPlatforms();
        Platform GePlatformById(int id);
        void CreatePlatform(Platform platform);
        bool NameExists(string Name);


    }
}
