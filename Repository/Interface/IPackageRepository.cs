using DataAccessLayer.Models;
using Repository.BaseRepository;

namespace Repository.Interface
{
    public interface IPackageRepository : IBaseRepository<Package>
    {
        public Package? GetById(int id);

        public int GetPackageIdByName(string name);

        public Package? GetPackageByName(string name);
    }
}
