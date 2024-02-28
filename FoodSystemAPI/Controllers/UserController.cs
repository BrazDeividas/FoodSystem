using System.Diagnostics;
using System.Security.Claims;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Services;
using FoodSystemAPI.Wrappers;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodSystemAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
     public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
    {
        if(string.IsNullOrEmpty(loginDto.username))
        {
            return BadRequest();
        }

        var user = await _userService.GetUserByUsername(loginDto.username);

        if(user == null)
        {
            return NotFound();
        }

        var claimsPrincipal = new ClaimsPrincipal(
            new ClaimsIdentity(
                [new Claim(ClaimTypes.Name, loginDto.username)], 
                BearerTokenDefaults.AuthenticationScheme));

        return SignIn(claimsPrincipal);
    }

    [HttpGet("metrics")]
    public async Task<ActionResult<Response<UserMetrics>>> GetMetrics()
    {
        ClaimsPrincipal claimsPrincipal = HttpContext.User;
        var username = claimsPrincipal.FindFirst(ClaimTypes.Name);

        if(username == null)
        {
            return NotFound("User not found");
        }

        var user = await _userService.GetUserByUsername(username.Value);
        var userMetrics = await _userService.GetUserMetricsByUserIdAsync(user.UserId);
        if (userMetrics == null)
        {
            return NotFound("User metrics not found");
        }
        return Ok(new Response<UserMetrics>(userMetrics));
    }

    [HttpPost("metrics")]
    public async Task<ActionResult<Response<UserMetrics>>> PostMetrics(PostUserMetricsDto metrics)
    {
        ClaimsPrincipal claimsPrincipal = HttpContext.User;
        var username = claimsPrincipal.FindFirst(ClaimTypes.Name);

        if(username == null)
        {
            return NotFound();
        }

        var user = await _userService.GetUserByUsername(username.Value);

        var userMetrics = await _userService.AddUserMetricsAsync(metrics, user.UserId);
        if (userMetrics == null)
        {
            return NotFound("User not found");
        }
        return Ok(new Response<UserMetrics>(userMetrics));
    }

    [HttpPut("metrics")]
    public async Task<ActionResult<Response<UserMetrics>>> PutMetrics(PostUserMetricsDto metrics)
    {
        ClaimsPrincipal claimsPrincipal = HttpContext.User;
        var username = claimsPrincipal.FindFirst(ClaimTypes.Name);

        if(username == null)
        {
            return NotFound();
        }

        var user = await _userService.GetUserByUsername(username.Value);
        var userMetrics = await _userService.UpdateUserMetricsAsync(metrics, user.UserId);
        if (userMetrics == null)
        {
            return NotFound(new Response<UserMetrics>("User not found"));
        }
        return Ok(new Response<UserMetrics>(userMetrics));
    }
}