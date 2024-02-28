using DataAccessLayer.Models;
using Repository.Interface;
using Services.Interface;
using Services.RequestDTO;
using Services.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UserService : IUserService
    {
        readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public ResponseDTO<string> Login(LoginRegisterDTO dto)
        {
            User? existedUser = userRepository.GetByEmail(dto.Email);
            if (existedUser == null)
            {
                return new ResponseDTO<string>
                {
                    statusCode = 400,
                    message = "wrong username or password"
                };
            }
            if (existedUser.Password == dto.Password)
            {
                return new ResponseDTO<string>
                {
                    statusCode = 200,
                    message = "login success"
                };
            }
            return new ResponseDTO<string>
            {
                statusCode = 500,
                message = "server error :("
            };
        }

        public ResponseDTO<string> Register(LoginRegisterDTO dto)
        {
            User user = new User
            {
                Email = dto.Email,
                Password = dto.Password,
                FullName = dto.FullName,
                Phone = dto.Phone,
                Address = dto.Address,
                Role = DataAccessLayer.Enum.Role.Customer,
                IsDeleted = false
            };
            userRepository.Add(user);
            return new ResponseDTO<string>
            {
                statusCode = 200,
                message = "new user created"
            };
        }
    }
}
