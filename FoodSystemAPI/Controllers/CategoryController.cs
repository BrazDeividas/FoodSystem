using AutoMapper;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Repositories;
using FoodSystemAPI.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace FoodSystemAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IRepository<Category> _repository;
    private readonly IMapper _mapper;

    public CategoryController(IRepository<Category> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<Category>>>> GetAll()
    {
        var entities = await _repository.GetAll();
        return Ok(new Response<IEnumerable<Category>>(entities));
    }

    [HttpGet("id")]
    public async Task<ActionResult<Response<Category>>> GetById(int id)
    {
        var entity = await _repository.GetById(id);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(new Response<Category>(entity));
    }

    [HttpPost]
    public async Task<ActionResult<Response<Category>>> Add(PostCategoryDto request)
    {
        var newCategory = _mapper.Map<Category>(request);
        var newEntity = await _repository.Add(newCategory);
        _repository.Save();
        return CreatedAtAction(nameof(GetById), new { id = newEntity.CategoryId }, new Response<Category>(newEntity));
    }

    [HttpPut]
    public ActionResult<Response<Category>> Update(Category entity)
    {
        var updatedEntity = _repository.Update(entity);
        _repository.Save();
        return Ok(new Response<Category>(updatedEntity));
    }

    [HttpDelete("id")]
    public async Task<ActionResult<Response<Category>>> Delete(int id)
    {
        var entity = await _repository.Delete(id);
        if (entity == null)
        {
            return NotFound();
        }
        _repository.Save();
        return Ok(new Response<Category>(entity));
    }
}