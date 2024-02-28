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
            return (User)GetAll().Where(user => user.Email.Equals(email));
        }

        public User GetById(int id)
        {
            return (User)GetAll().Where(user => user.Id.Equals(id));
        }
    }
}
