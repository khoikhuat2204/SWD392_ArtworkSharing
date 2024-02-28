using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using Services.RequestDTO;

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
            return StatusCode(418, "ha ha error fix it.");
        }        
        
        [HttpPost("register")]
        public IActionResult register([FromBody] LoginRegisterDTO dto)
        {
            bool newUserCreated = userService.Register(dto);
            if(newUserCreated)
            {
                return Ok($"add user with email {dto.Email}");
            }

            return StatusCode(418, "ha ha error fix it.");
        }
    }
}
