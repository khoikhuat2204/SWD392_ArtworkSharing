using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using DataAccessLayer.Models;
using System.Threading.Tasks;

namespace SWD392.Controllers;


[ApiController]
[Route("/")]

    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet]
        public IActionResult GetAllPackages()
        {
            var packages = _packageService.GetAll();
            return Ok(packages);
        }

        [HttpPost]
        public IActionResult AddPackage([FromBody] Package package)
        {
            _packageService.Add(package);
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdatePackage([FromBody] Package package)
        {
            _packageService.Update(package);
            return Ok();
        }

        [HttpDelete]
        public IActionResult RemovePackage([FromBody] Package package)
        {
            _packageService.Remove(package);
            return Ok();
        }
    }

