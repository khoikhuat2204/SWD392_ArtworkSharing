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
        public User? Login(string email, string password)
        {
            return GetAll().FirstOrDefault(user => user.Email == email && user.Password == password);
        }

        public User? GetByEmail(string email)
        {
            return GetAll().ToList().Find(user => user.Email == email);
        }

        public User? GetById(int id)
        {
            return GetAll().ToList().Find(user => user.Id == id);
        }
    }
}
