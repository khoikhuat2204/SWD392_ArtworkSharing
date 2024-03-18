using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;

namespace Services.Interface
{
    public interface IPackageService
    {
        public List<Package> GetAll();

        public void Add(Package package);

        public bool Update(Package package);

        public void Remove(Package package);
    }
}
