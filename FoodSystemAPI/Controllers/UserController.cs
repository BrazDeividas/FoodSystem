using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Services;
using FoodSystemAPI.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace FoodSystemAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("metrics")]
    public async Task<ActionResult<Response<UserMetrics>>> PostMetrics(PostUserMetricsDto metrics)
    {
        var userMetrics = await _userService.AddUserMetricsAsync(metrics);
        if (userMetrics == null)
        {
            return NotFound(new Response<UserMetrics>("User not found or metrics already exist"));
        }
        return Ok(new Response<UserMetrics>(userMetrics));
    }

    [HttpPut("metrics")]
    public async Task<ActionResult<Response<UserMetrics>>> PutMetrics(PostUserMetricsDto metrics)
    {
        var userMetrics = await _userService.UpdateUserMetricsAsync(metrics);
        if (userMetrics == null)
        {
            return NotFound(new Response<UserMetrics>("User not found"));
        }
        return Ok(new Response<UserMetrics>(userMetrics));
    }
}