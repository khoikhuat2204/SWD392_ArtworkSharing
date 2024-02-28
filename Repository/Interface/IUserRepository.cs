using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using Repository.BaseRepository;

namespace Repository.Interface
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User? GetById(int id);
        User? GetByEmail(string email);
    }
}
