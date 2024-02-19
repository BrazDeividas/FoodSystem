using InternalAPI.Models;
using InternalAPI.Repositories;

namespace InternalAPI.Services;

public interface IUnitOfWork
{
    IRepository<Recipe> Recipes { get; }
    Task CompleteAsync();
}