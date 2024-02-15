using FoodSystemAPI.Entities;
using FoodSystemAPI.Repositories;
using FoodSystemAPI.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace FoodSystemAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private IRepository<Category> _repository;

    public CategoryController(IRepository<Category> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll()
    {
        var entities = await _repository.GetAll();
        return Ok(new Response<IEnumerable<Category>>(entities));
    }

    [HttpGet("id")]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        var entity = await _repository.GetById(id);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(new Response<Category>(entity));
    }

    [HttpPost]
    public async Task<ActionResult<Category>> Add(Category entity)
    {
        var newEntity = await _repository.Add(entity);
        return CreatedAtAction(nameof(GetById), new { id = newEntity.CategoryId }, new Response<Category>(newEntity));
    }

    [HttpPut]
    public ActionResult<Category> Update(Category entity)
    {
        var updatedEntity = _repository.Update(entity);
        return Ok(new Response<Category>(updatedEntity));
    }

    [HttpDelete("id")]
    public async Task<ActionResult<Category>> Delete(int id)
    {
        var entity = await _repository.Delete(id);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(new Response<Category>(entity));
    }
}