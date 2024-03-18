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
        User? Login(string email, string password);
        User? GetById(int id);
        User? GetByEmail(string email);
        int GetIdByEmail(string email);
        string GetNameByEmail(string email);
        IQueryable<User> GetAllCreator();
        IQueryable<User> GetAllUsers();
    }
}
