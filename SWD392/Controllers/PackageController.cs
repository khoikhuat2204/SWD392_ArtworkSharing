using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using DataAccessLayer.Models;
using AutoMapper;
using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.DTOs.ResponseDTO;
using Microsoft.AspNetCore.Authorization;

namespace SWD392.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;
        private readonly IMapper _mapper;

        public PackageController(IPackageService packageService, IMapper mapper)
        {
            _packageService = packageService;
            _mapper = mapper;
        }

        [HttpGet("get-all-packages")]
        public IActionResult GetAllPackages()
        {
            var packages = _packageService.GetAll();
            if (!packages.Any())
                return Ok("No package found!");
            var mappedPackages = packages.Select(p => _mapper.Map<PackageDTO>(p)).ToList();
            return Ok(packages);
        }

        [HttpGet("get-package/{id}")]
        public IActionResult GetPackage(int id)
        {
            Package? package = _packageService.GetById(id);
            if (package == null)
            {
                return BadRequest();
            }
            return Ok(package);
        }

        [HttpPost("create-new-package")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddPackage([FromBody] CreatePackageDTO createPackageDto)
        {
            var createdPackage = new Package()
            {
                Name = createPackageDto.Name,
                Description = createPackageDto.Description,
                Price = createPackageDto.Price,
                UploadsPerDay = createPackageDto.UploadsPerDay,
                TotalUploads = createPackageDto.TotalUploads,
                IsDeleted = false
            };
            _packageService.Add(createdPackage);
            return Ok();
        }

        [HttpPut("update-package/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdatePackage(int id, [FromBody] UpdatePackageDTO updatePackageDto)
        {
            var existingPackage = _packageService.GetAll().FirstOrDefault(a => a.Id == id);
            if (id == 0)
            {
                return BadRequest();
            }
            if (existingPackage == null)
                return NotFound();

            existingPackage.Name = updatePackageDto.Name;
            existingPackage.Description = updatePackageDto.Description;
            existingPackage.Price = updatePackageDto.Price;
            existingPackage.UploadsPerDay = updatePackageDto.UploadsPerDay;
            existingPackage.TotalUploads = updatePackageDto.TotalUploads;
            existingPackage.IsDeleted = updatePackageDto.IsDeleted;
            _packageService.Update(existingPackage);
            return Ok();
        }

        [HttpDelete("delete-package/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RemovePackage(int id)
        {
            var package = _packageService.GetAll().FirstOrDefault(a => a.Id == id);
            if (package == null)
            {
                return NotFound();
            }
            _packageService.Remove(package);
            return NoContent();
        }
    }
}
