using FoodSystemAPI.Entities;
using FoodSystemAPI.Filters;
using FoodSystemAPI.Helpers;
using FoodSystemAPI.Repositories;
using FoodSystemAPI.Services;
using FoodSystemAPI.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace FoodSystemAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IngredientController : ControllerBase
{
    private readonly IRepository<Ingredient> _repository;
    private readonly IUriService _uriService;

    public IngredientController(IRepository<Ingredient> repository, IUriService uriService)
    {
        _repository = repository;
        _uriService = uriService;
    }

    [HttpGet]
    public async Task<ActionResult<Response<Ingredient>>> GetAll([FromQuery] PaginationFilter paginationFilter)
    {
        var route = Request.Path.Value;
        var validFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
        var entities = await _repository.GetAll(validFilter);
        var totalRecords = await _repository.CountAsync();
        var pagedReponse = PaginationHelper.CreatePagedResponse<Ingredient>(entities, validFilter, totalRecords, _uriService, route);
        return Ok(pagedReponse);
    }

    [HttpGet("id")]
    public async Task<ActionResult<Ingredient>> GetById(int id)
    {
        var entity = await _repository.GetById(id);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(new Response<Ingredient>(entity));
    }

    [HttpPost]
    public async Task<ActionResult<Ingredient>> Add(Ingredient entity)
    {
        var newEntity = await _repository.Add(entity);
        return CreatedAtAction(nameof(GetById), new { id = newEntity.IngredientId }, new Response<Ingredient>(newEntity));
    }

    [HttpPut]
    public ActionResult<Ingredient> Update(Ingredient entity)
    {
        var updatedEntity = _repository.Update(entity);
        return Ok(new Response<Ingredient>(updatedEntity));
    }

    [HttpDelete("id")]
    public async Task<ActionResult<Ingredient>> Delete(int id)
    {
        var entity = await _repository.Delete(id);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(new Response<Ingredient>(entity));
    }
}