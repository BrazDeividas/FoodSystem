using System.Linq.Expressions;
using System.Web;
using AutoMapper;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Filters;
using FoodSystemAPI.Helpers;
using FoodSystemAPI.Services;
using FoodSystemAPI.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace FoodSystemAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IngredientController : ControllerBase
{
    private readonly IIngredientService _service;
    private readonly IUriService _uriService;
    private readonly IMapper _mapper;

    public IngredientController(IIngredientService service, IUriService uriService, IMapper mapper)
    {
        _service = service;
        _uriService = uriService;
        _mapper = mapper;
    }

    [HttpGet] 
    public async Task<ActionResult<Response<IEnumerable<Ingredient>>>> GetAll([FromQuery] PaginationFilter paginationFilter, [FromQuery] int categoryId = 0, [FromQuery] string searchString = "")
    {
        var route = Request.Path.Value;
        var validFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
        IEnumerable<Ingredient> entities;
        int totalRecords;
        PagedResponse<IEnumerable<Ingredient>> pagedResponse;

        if(categoryId != 0)
        {
            Expression<Func<Ingredient, bool>> expression = !string.IsNullOrEmpty(searchString) 
            ? x => x.CategoryId == categoryId && x.Description.Contains(searchString)
            : x => x.CategoryId == categoryId;
            entities = await _service.GetAll(validFilter, expression);
            totalRecords = await _service.CountAsync(expression);
        }
        else
        {
            Expression<Func<Ingredient, bool>> expression = !string.IsNullOrEmpty(searchString) 
            ? x => x.Description.Contains(searchString)
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

    [HttpPost]
    public async Task<ActionResult<Response<Ingredient>>> Add(PostIngredientDto request)
    {
        var newEntity = await _service.Add(request);
        return CreatedAtAction(nameof(GetById), new { id = newEntity.IngredientId }, new Response<Ingredient>(newEntity));
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