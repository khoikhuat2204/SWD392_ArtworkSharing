using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Extensions;
using Services.Interface;

namespace SWD392.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO dto)
        {
            ResponseDTO<string> res = userService.Login(dto);
            if (res.statusCode == 200)
            {
                return Ok(res.message);
            }
            return StatusCode(res.statusCode, res.message);

        }
        
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDTO dto)
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
