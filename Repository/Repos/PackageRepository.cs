using DataAccessLayer.Models;
using Repository.BaseRepository;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repos
{
    public class PackageRepository : BaseRepository<Package>, IPackageRepository
    {
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
