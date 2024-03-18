using DataAccessLayer.Models;
using Repository.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.DTOs.RequestDTO;
using Services.Extensions;

namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly TokenService _tokenService;

        public UserService(IUserRepository userRepository, TokenService tokenService)
        {
            this.userRepository = userRepository;
            _tokenService = tokenService;
        }

        public ResponseDTO<string> Login(LoginDTO dto)
        {
            User? existedUser = userRepository.GetByEmail(dto.Email);
            if (existedUser == null)
            {
                return new ResponseDTO<string>
                {
                    statusCode = 400,
                    message = "Wrong username or password"
                };
            }
            if (existedUser.Password == dto.Password)
            {
                var jwtToken = _tokenService.CreateTokenForAccount(existedUser);
                return new ResponseDTO<string>
                {
                    statusCode = 200,
                    message = jwtToken
                };
            }
            return new ResponseDTO<string>
            {
                statusCode = 500,
                message = "Server Error"
            };
        }

        public User GetById(int id)
        {
            return userRepository.GetById(id);
        }

        public int GetIdByEmail(string email)
        {
            return userRepository.GetIdByEmail(email);
        }

        public ResponseDTO<string> Register(RegisterDTO dto)
        {
            User user = new User
            {
                Email = dto.Email,
                Password = dto.Password,
                FullName = dto.FullName,
                Phone = dto.Phone,
                Address = dto.Address,
                Role = dto.Role,
                IsDeleted = false
            };
            userRepository.Add(user);
            return new ResponseDTO<string>
            {
                statusCode = 200,
                message = "New user created"
            };
        }

        public string GetNameByEmail(string email)
        {
            return userRepository.GetNameByEmail(email);
        }
    }
}
