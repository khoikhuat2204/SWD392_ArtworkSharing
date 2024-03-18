using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using DataAccessLayer.Models;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.DTOs.ResponseDTO;
using Microsoft.AspNetCore.Authorization;

namespace SWD392.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IMapper _mapper;
        private readonly IArtworkService _artworkService;
        private readonly IUserService _userService;

        public ReportController(IReportService reportService, IMapper mapper,
            IArtworkService artworkService, IUserService userService)
        {
            _reportService = reportService;
            _mapper = mapper;
            _artworkService = artworkService;
            _userService = userService;
        }

        [HttpGet("get-all-reports")]
        [Authorize(Roles = "Moderator")]
        public IActionResult GetAllReports()
        {
            var reports = _reportService.GetAll();
            if (reports == null)
            {
                return Ok("No report found");
            }
            var result = _mapper.Map<List<ReportResponseDTO>>(reports);
            return Ok(result);
        }

        [HttpPost("create-new-report")]
        [Authorize(Roles = "Audience,Creator")]
        public IActionResult AddReport([FromBody] ReportRequestDTO report)
        {
            if(report == null)
            {
                return BadRequest("Report is null");
            }
            
            if(_userService.GetById(report.UserId) == null)
            {
                return BadRequest("User not found");
            }
            if(_artworkService.GetAll().FirstOrDefault(a => a.Id == report.ArtworkId) == null)
            {
                return BadRequest("Artwork not found");
            }
            
            var mappedReport = _mapper.Map<Report>(report);
            _reportService.Add(mappedReport);
            return Ok();
        }

        // [HttpPut("update-report/{id}")]
        // public IActionResult UpdateReport(int id, [FromBody] ReportResponseDTO report)
        // {
        //     if(_reportService.GetAll().Any(r => r.Id == id) == false)
        //     {
        //         return BadRequest("Report not found");
        //     }
        //     if(report == null)
        //     {
        //         return BadRequest("Report is null");
        //     }
        //     
        //     if(_userService.GetById(report.UserId) == null)
        //     {
        //         return BadRequest("User not found");
        //     }
        //     if(_artworkService.GetAll().FirstOrDefault(a => a.Id == report.ArtworkId) == null)
        //     {
        //         return BadRequest("Artwork not found");
        //     }
        //     
        //     _reportService.Update(_mapper.Map<Report>(report));
        //     return Ok();
        // }

        // [HttpDelete("delete-report/{id}")]
        // public IActionResult RemoveReport([FromBody] Report report)
        // {
        //     _reportService.Remove(report);
        //     return Ok();
        // }
    }
}
