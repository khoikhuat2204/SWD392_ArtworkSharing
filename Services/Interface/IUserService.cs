using DataAccessLayer.DTOs.RequestDTO;

namespace Services.Interface
{
    public interface IUserService
    {
        ResponseDTO<string> Register(RegisterDTO dto);

        ResponseDTO<string> Login(LoginDTO dto);
    }
}
