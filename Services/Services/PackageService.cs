using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using Repository.Interface;

namespace Services.Services
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _packageRepository;
        
        public PackageService(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }
        public List<Package> GetAll()
        {
            return _packageRepository.GetAll().ToList();
        }

        public void Add(Package package)
        {
            _packageRepository.Add(package);
        }

        public bool Update(Package package)
        {
            try
            {
                _packageRepository.Update(package);
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public void Remove(Package package)
        {
            _packageRepository.Delete(package);
        }

        public int GetPackageIdByName(string name)
        {
            return _packageRepository.GetPackageIdByName(name);
        }

        public Package? GetPackageByName(string name)
        {
            return _packageRepository.GetPackageByName(name);
        }
    }
}
