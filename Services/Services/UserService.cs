using DataAccessLayer.Models;
using Repository.Interface;
using Services.Interface;
using Services.RequestDTO;
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
        public bool Register(LoginRegisterDTO dto)
        {
            try
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
                return true;
            }catch(Exception ex)
            {
                throw new Exception(ex.InnerException?.ToString());
            }
        }
    }
}
