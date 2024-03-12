using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Web;
using AutoMapper;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Filters;
using FoodSystemAPI.Helpers;
using FoodSystemAPI.Services;
using FoodSystemAPI.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodSystemAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class IngredientController : ControllerBase
{
    private readonly IIngredientService _service;
    private readonly IUriService _uriService;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public IngredientController(IIngredientService service, IUriService uriService, IMapper mapper, IUserService userService)
    {
        _service = service;
        _uriService = uriService;
        _mapper = mapper;
        _userService = userService;
    }

    [HttpGet] 
    public async Task<ActionResult<Response<IEnumerable<Ingredient>>>> GetAll([FromQuery] PaginationFilter paginationFilter, [FromQuery] int categoryId = 0, [FromQuery] string search = "")
    {
        var route = Request.Path.Value;
        var validFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
        IEnumerable<Ingredient> entities;
        int totalRecords;
        PagedResponse<IEnumerable<Ingredient>> pagedResponse;

        if(categoryId != 0)
        {
            Expression<Func<Ingredient, bool>> expression = !string.IsNullOrEmpty(search) 
            ? x => x.CategoryId == categoryId && x.Description.Contains(search)
            : x => x.CategoryId == categoryId;
            entities = await _service.GetAll(validFilter, expression);
            totalRecords = await _service.CountAsync(expression);
        }
        else
        {
            Expression<Func<Ingredient, bool>> expression = !string.IsNullOrEmpty(search) 
            ? x => x.Description.Contains(search)
            : null;
            entities = expression != null
            ? await _service.GetAll(validFilter, expression)
            : await _service.GetAll(validFilter);
            totalRecords = expression != null
            ? await _service.CountAsync(expression)
            : await _service.CountAsync();
        }
        
        var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
        parameters  = PaginationHelper.TrimPaginationParameters(parameters);
        var parametersEnumerable = NameValueCollectionExtensions.AsEnumerable(parameters);
        pagedResponse = PaginationHelper.CreatePagedResponse(entities, validFilter, totalRecords, _uriService, route, parametersEnumerable);
        return Ok(pagedResponse);
    }

    [HttpGet("id")]
    public async Task<ActionResult<Response<Ingredient>>> GetById(int id)
    {
        var entity = await _service.GetById(id);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(new Response<Ingredient>(entity));
    }

    [HttpGet("byUser")]
    public async Task<ActionResult<Response<IEnumerable<Ingredient>>>> GetByUser()
    {
        ClaimsPrincipal claimsPrincipal = HttpContext.User;
        var username = claimsPrincipal.FindFirst(ClaimTypes.Name);
        
        if(username == null)
        {
            return NotFound();
        }

        var user = await _userService.GetUserByUsername(username.Value);
        var userIngredients = await _service.GetAll(x => x.Users.Contains(user));
        return Ok(new Response<IEnumerable<Ingredient>>(userIngredients));
    }

    [HttpPost]
    public async Task<ActionResult<Response<Ingredient>>> Add(PostIngredientDto request)
    {
        
        var newEntity = await _service.Add(request);
        return CreatedAtAction(nameof(GetById), new { id = newEntity.IngredientId }, new Response<Ingredient>(newEntity));
    }

    [HttpPost("AddToUser")]
    public async Task<ActionResult> AddToUser(IEnumerable<int> ingredientIds)
    {
        ClaimsPrincipal claimsPrincipal = HttpContext.User;
        var username = claimsPrincipal.FindFirst(ClaimTypes.Name);
        
        if(username == null)
        {
            return NotFound();
        }

        var user = await _userService.GetUserByUsername(username.Value);
        var ingredients = await _service.GetAll(x => ingredientIds.Contains(x.IngredientId));
        await _userService.AddIngredientsToUserAsync(ingredients, user.UserId);
        return Ok();
    }
    [HttpDelete("RemoveFromUser")]
    public async Task<ActionResult> RemoveFromUser(IEnumerable<int> ingredientIds)
    {
        ClaimsPrincipal claimsPrincipal = HttpContext.User;
        var username = claimsPrincipal.FindFirst(ClaimTypes.Name);
        
        if(username == null)
        {
            return NotFound();
        }

        var user = await _userService.GetUserByUsername(username.Value);
        var ingredients = await _service.GetAll(x => ingredientIds.Contains(x.IngredientId));
        await _userService.RemoveIngredientsFromUserAsync(ingredients, user.UserId);
        return Ok();
    }

    [HttpPut]
    public ActionResult<Response<Ingredient>> Update(Ingredient entity)
    {
        var updatedEntity = _service.Update(entity);
        return Ok(new Response<Ingredient>(updatedEntity));
    }

    [HttpDelete("id")]
    public async Task<ActionResult<Response<Ingredient>>> Delete(int id)
    {
        var entity = await _service.Delete(id);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(new Response<Ingredient>(entity));
    }
}