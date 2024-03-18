using AutoMapper;
using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.DTOs.ResponseDTO;
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
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this._mapper = mapper;

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

        [HttpGet("get-creators")]
        public IActionResult GetAllCreators()
        {
            var users = userService.GetAllCreator();
            if (!users.Any())
                return Ok("No creators found");
            var mappedUsers = users.Select(p => _mapper.Map<UserDTO>(p)).ToList();
            return Ok(mappedUsers);
        }

        [HttpGet("get-all-users")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUsers()
        {
            var users = userService.GetAllUsers();
            if (!users.Any())
                return Ok("No users found");
            var mappedUsers = users.Select(p => _mapper.Map<UserDTO>(p)).ToList();
            return Ok(mappedUsers);

        }

        [HttpGet("view-account-detail/{id}")]
        public IActionResult ViewAccountDetail(int id)
        {
            var user = userService.GetById(id);
            if (user == null)
                return BadRequest("No user found");
            var mappedUser = _mapper.Map<ViewAccountDTO>(user);
            return Ok(mappedUser);
        }
    }
}
