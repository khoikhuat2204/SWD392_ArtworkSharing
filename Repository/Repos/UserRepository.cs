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
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public User GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
