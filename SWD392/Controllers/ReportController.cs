using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using DataAccessLayer.Models;
using System.Threading.Tasks;

namespace SWD392.Controllers
{
    [ApiController]
    [Route("/")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public IActionResult GetAllReports()
        {
            var reports = _reportService.GetAll();
            return Ok(reports);
        }

        [HttpPost]
        public IActionResult AddReport([FromBody] Report report)
        {
            _reportService.Add(report);
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateReport([FromBody] Report report)
        {
            _reportService.Update(report);
            return Ok();
        }

        [HttpDelete]
        public IActionResult RemoveReport([FromBody] Report report)
        {
            _reportService.Remove(report);
            return Ok();
        }
    }
}
