using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using Services.RequestDTO;
using Services.ResponseDTO;

namespace SWD392.Controllers
{
    [ApiController]
    [Route("/")]
    public class UserController : Controller
    {
        readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        public IActionResult login([FromBody] LoginRegisterDTO dto)
        {
            ResponseDTO<string> res = userService.Login(dto);
            if (res.statusCode == 200)
            {
                return Ok(res.message);
            }
            return StatusCode(res.statusCode, res.message);

        }

        [HttpPost("register")]
        public IActionResult register([FromBody] LoginRegisterDTO dto)
        {
            ResponseDTO<string> res = userService.Register(dto);
            if (res.statusCode == 200)
            {
                return Ok(res.message);
            }

            return StatusCode(res.statusCode, res.message);
        }
    }
}
