﻿using DataAccessLayer.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Extensions;
using Services.Interface;
using Services.RequestDTO;
using Services.ResponseDTO;

namespace SWD392.Controllers
{
    [ApiController]
    [Route("/")]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly TokenService _tokenService;

        public UserController(IUserService userService, TokenService tokenService)
        {
            this.userService = userService;
            _tokenService = tokenService;
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
