using AutoMapper;
using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.DTOs.ResponseDTO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Interface;

namespace SWD392.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : Controller
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly IMapper _mapper;
    
    public SubscriptionController(ISubscriptionService subscriptionService, IMapper mapper)
    {
        _subscriptionService = subscriptionService;
        _mapper = mapper;
    }
    
    [HttpGet("get-all-subscriptions")]
    public async Task<IActionResult> GetAllSubscriptions()
    {
        var subscriptions = _subscriptionService.GetAllActiveSubscriptions()
            .Include(a => a.Package)
            .Include(a => a.User).ToList();
        if (!subscriptions.Any())
            return NotFound();
        // need remapping for user and package
        var mappedSubscriptions = _mapper.Map<List<ActiveSubscriptionDTO>>(subscriptions);
        // map user name and package name
        foreach (var subscription in mappedSubscriptions)
        {
            subscription.UserName = subscriptions.FirstOrDefault(a => a.Id == subscription.Id)?.User?.FullName;
            subscription.PackageName = subscriptions.FirstOrDefault(a => a.Id == subscription.Id)?.Package?.Name;
        }
        return Ok(mappedSubscriptions);
    }
    
    [HttpPost("add-subscription")]
    public async Task<IActionResult> AddSubscription([FromBody] CreateSubscriptionDTO createSubscriptionDTO)
    {
        var createdSubscription = new ActiveSubscription()
        {
            UserId = createSubscriptionDTO.UserId,
            PackageId = createSubscriptionDTO.PackageId,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(1)
        };
        _subscriptionService.AddSubscription(createdSubscription);
        return Ok();
    }
    
    [HttpDelete("delete-subscription/{id}")]
    public async Task<IActionResult> DeleteSubscription(int id)
    {
        var subscription = _subscriptionService.GetAllActiveSubscriptions().FirstOrDefault(a => a.Id == id);
        if (subscription == null)
            return NotFound();
        _subscriptionService.RemoveSubscription(subscription);
        return NoContent();
    }
}