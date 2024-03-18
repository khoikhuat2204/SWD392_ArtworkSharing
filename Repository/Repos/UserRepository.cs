﻿using DataAccessLayer.Models;
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
        public User? GetByEmail(string email)
        {
            return GetAll().ToList().Find(user => user.Email == email);
        }

        public User? GetById(int id)
        {
            return GetAll().ToList().Find(user => user.Id == id);
        }

        // GetUserIdByEmail is a method that returns the id of a user by their email
        public int GetIdByEmail(string email)
        {
            var user  = GetAll().ToList().Find(user => user.Email == email);
            return user?.Id ?? 0;
        }

        // Get Name by Email is a method that returns the name of a user by their email
        public string GetNameByEmail(string email)
        {
            var user = GetAll().ToList().Find(user => user.Email == email);
            return user?.FullName ?? "";
        }
    }
}
