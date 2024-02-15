using AutoMapper;
using FoodSystemAPI.DTOs;
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
    private readonly IMapper _mapper;

    public IngredientController(IRepository<Ingredient> repository, IUriService uriService, IMapper mapper)
    {
        _repository = repository;
        _uriService = uriService;
        _mapper = mapper;
    }

    [HttpGet] //fix links after setting custom category
    public async Task<ActionResult<Response<IEnumerable<Ingredient>>>> GetAll([FromQuery] PaginationFilter paginationFilter, [FromQuery] int categoryId = 0)
    {
        var route = Request.Path.Value;
        var validFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
        IEnumerable<Ingredient> entities;
        int totalRecords;
        PagedResponse<IEnumerable<Ingredient>> pagedResponse;

        if(categoryId != 0)
        {
            entities = await _repository.GetAll(validFilter, x => x.CategoryId == categoryId);
            totalRecords = await _repository.CountAsync(x => x.CategoryId == categoryId);
        }
        else
        {
            entities = await _repository.GetAll(validFilter);
            totalRecords = await _repository.CountAsync();
        }

        pagedResponse = PaginationHelper.CreatePagedResponse(entities, validFilter, totalRecords, _uriService, route);
        return Ok(pagedResponse);
    }

    [HttpGet("id")]
    public async Task<ActionResult<Response<Ingredient>>> GetById(int id)
    {
        var entity = await _repository.GetById(id);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(new Response<Ingredient>(entity));
    }

    [HttpPost]
    public async Task<ActionResult<Response<Ingredient>>> Add(PostIngredientDto request)
    {
        var model = _mapper.Map<Ingredient>(request);
        var newEntity = await _repository.Add(model);
        _repository.Save();
        return CreatedAtAction(nameof(GetById), new { id = newEntity.IngredientId }, new Response<Ingredient>(newEntity));
    }

    [HttpPut]
    public ActionResult<Response<Ingredient>> Update(Ingredient entity)
    {
        var updatedEntity = _repository.Update(entity);
        _repository.Save();
        return Ok(new Response<Ingredient>(updatedEntity));
    }

    [HttpDelete("id")]
    public async Task<ActionResult<Response<Ingredient>>> Delete(int id)
    {
        var entity = await _repository.Delete(id);
        if (entity == null)
        {
            return NotFound();
        }
        _repository.Save();
        return Ok(new Response<Ingredient>(entity));
    }
}