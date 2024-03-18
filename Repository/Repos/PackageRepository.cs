using DataAccessLayer.Models;
using Repository.BaseRepository;
using Repository.Interface;

namespace Repository.Repos
{
    public class PackageRepository : BaseRepository<Package>, IPackageRepository
    {
        public Package? GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public int GetPackageIdByName(string name)
        {
            var package = GetAll().ToList().Find(p => p.Name == name);
            return package?.Id ?? 0;
        }

        // GetPackageByName
        public Package? GetPackageByName(string name)
        {
            return GetAll().ToList().Find(p => p.Name == name);
        }
    }
}
