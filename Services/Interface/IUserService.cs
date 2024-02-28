using DataAccessLayer.Models;
using Services.RequestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IUserService
    {
        bool Register(LoginRegisterDTO dto);
    }
}
