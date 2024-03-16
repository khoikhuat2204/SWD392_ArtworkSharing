using DataAccessLayer.Models;

namespace Services.Interface;

public interface ITokenService
{
    public string CreateTokenForAccount(User user);
}