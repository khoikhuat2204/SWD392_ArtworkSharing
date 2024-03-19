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
using AutoMapper;
using DataAccessLayer.DTOs.ResponseDTO;
using Microsoft.AspNetCore.Http;

namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IAzureService _azureService;

        public UserService(IUserRepository userRepository, IMapper mapper, ITokenService tokenService,
            IAzureService azureService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
            _azureService = azureService;
        }

        public ResponseDTO<string> Login(LoginDTO dto)
        {
            User? existedUser = _userRepository.Login(dto.Email ?? string.Empty, dto.Password ?? string.Empty);
            if (existedUser == null)
            {
                return new ResponseDTO<string>
                {
                    statusCode = 400,
                    message = "Wrong username or password"
                };
            }
            var jwtToken = _tokenService.CreateTokenForAccount(existedUser);
            return new ResponseDTO<string>
            {
                statusCode = 200,
                message = jwtToken
            };
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public int GetIdByEmail(string email)
        {
            return _userRepository.GetIdByEmail(email);
        }

        public ResponseDTO<string> Register(RegisterDTO dto)
        {
            // check email exists
            User? existedUser = _userRepository.GetByEmail(dto.Email ?? string.Empty);
            if (existedUser != null)
            {
                return new ResponseDTO<string>
                {
                    statusCode = 400,
                    message = "A user with this email already exists!"
                };
            }
            User newUser = _mapper.Map<User>(dto);
            newUser.IsDeleted = false;
            if (_userRepository.Add(newUser))
            {
                return new ResponseDTO<string>
                {
                    statusCode = 200,
                    message = "New user created"
                };
            }
            return new ResponseDTO<string>
            {
                statusCode = 400,
                message = "Register failed! Please try again or contact us for more details."
            };
        }

        public string GetNameByEmail(string email)
        {
            return _userRepository.GetNameByEmail(email);
        }

        public List<User> GetAllCreator()
        {
            return _userRepository.GetAllCreator().ToList();
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers().ToList();
        }

        public bool UpdateProfile(User user)
        {
            try
            {
                _userRepository.Update(user);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
