using AutoMapper;
using DataAccessLayer.DTOs.ResponseDTO;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace SWD392.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;
    private readonly IMapper _mapper;

    public TagController(ITagService tagService, IMapper mapper)
    {
        _tagService = tagService;
        _mapper = mapper;
    }
    
    [HttpGet("get-all-tags")]
    public async Task<IActionResult> GetAllTags()
    {
        var tags = _tagService.GetAllTags();
        if (!tags.Any())
            return Ok("No tags found");
        var mappedTags = _mapper.Map<List<TagResponseDTO>>(tags);
        return Ok(mappedTags);
    }
}