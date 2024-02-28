using DataAccessLayer.Models;
using Services.RequestDTO;
using Services.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IUserService
    {
        ResponseDTO<string> Register(LoginRegisterDTO dto);

        ResponseDTO<string> Login(LoginRegisterDTO dto);
    }
}
