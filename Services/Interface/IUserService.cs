﻿using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.Models;

namespace Services.Interface
{
    public interface IUserService
    {
        ResponseDTO<string> Register(RegisterDTO dto);

        ResponseDTO<string> Login(LoginDTO dto);
        
        public User GetById(int id);

        List<User> GetAllCreator();
        List<User> GetAllUsers();
    }
}
