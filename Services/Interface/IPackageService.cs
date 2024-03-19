using DataAccessLayer.Models;

namespace Services.Interface
{
    public interface IPackageService
    {
        public List<Package> GetAll();

        public Package? GetById(int id);

        public void Add(Package package);

        public bool Update(Package package);

        public void Remove(Package package);

        public int GetPackageIdByName(string name);

        public Package? GetPackageByName(string name);
    }
}
