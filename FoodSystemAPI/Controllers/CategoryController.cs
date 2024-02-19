using AutoMapper;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Services;
using FoodSystemAPI.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace FoodSystemAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<Category>>>> GetAll()
    {
        var entities = await _service.GetAll();
        return Ok(new Response<IEnumerable<Category>>(entities));
    }

    [HttpGet("id")]
    public async Task<ActionResult<Response<Category>>> GetById(int id)
    {
        var entity = await _service.GetById(id);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(new Response<Category>(entity));
    }

    [HttpPost]
    public async Task<ActionResult<Response<Category>>> Add(PostCategoryDto request)
    {
        var newEntity = await _service.Add(request);
        return CreatedAtAction(nameof(GetById), new { id = newEntity.CategoryId }, new Response<Category>(newEntity));
    }

    [HttpPut]
    public ActionResult<Response<Category>> Update(Category entity)
    {
        var updatedEntity = _service.Update(entity);
        return Ok(new Response<Category>(updatedEntity));
    }

    [HttpDelete("id")]
    public async Task<ActionResult<Response<Category>>> Delete(int id)
    {
        var entity = await _service.Delete(id);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(new Response<Category>(entity));
    }
}