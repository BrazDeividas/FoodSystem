using AutoMapper;
using FoodSystemAPI.DTOs;
using FoodSystemAPI.Entities;
using FoodSystemAPI.Repositories;

namespace FoodSystemAPI.Services;

public class UserService : IUserService
{
    private readonly IRepository<UserMetrics> _userMetricsRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public UserService(IRepository<UserMetrics> userMetricsRepository, IRepository<User> userRepository, IMapper mapper)
    {
        _userMetricsRepository = userMetricsRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserMetrics> AddUserMetricsAsync(PostUserMetricsDto userMetrics, int userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null)
        {
            return null;
        }

        var userMetric = await _userMetricsRepository.GetAll(x => x.UserId == userId);
        if (userMetric.Any())
        {
            return null;
        }
        var userMetricsEntity = _mapper.Map<PostUserMetricsDto, UserMetrics>(userMetrics);
        await _userMetricsRepository.Add(userMetricsEntity);
        _userMetricsRepository.Save();

        return userMetricsEntity;
    }

    public async Task<UserMetrics> GetUserMetricsByUserIdAsync(int userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null)
        {
            return null;
        }

        var userMetrics = await _userMetricsRepository.GetAll(x => x.UserId == userId);
        if (!userMetrics.Any())
        {
            return null;
        }

        return userMetrics.First();
    }

    public async Task<UserMetrics> UpdateUserMetricsAsync(PostUserMetricsDto userMetrics, int userId)
    {
        var entity = await _userMetricsRepository.GetById(userId);
        if (entity == null)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                return null;
            }

            var userMetricsEntity = _mapper.Map<PostUserMetricsDto, UserMetrics>(userMetrics);

            await _userMetricsRepository.Add(userMetricsEntity);
            _userMetricsRepository.Save();
            return userMetricsEntity;
        }

        _mapper.Map(userMetrics, entity);
        _userMetricsRepository.Update(entity);
        _userMetricsRepository.Save();
        return entity;
    }

    public async Task<User> GetUserByUsername(string username)
    {
        return (await _userRepository.GetAll(x => x.Username == username)).FirstOrDefault();
    }

    public async Task AddIngredientsToUserAsync(IEnumerable<Ingredient> ingredients, int userId)
    {
        var user = (await _userRepository.GetAllInclude(x => x.UserId == userId, x => x.Ingredients)).FirstOrDefault();
        
        if(user == null)
        {
            return;
        }

        foreach(var ingredient in ingredients)
        {
            user.Ingredients.Add(ingredient);
        }

        _userRepository.Update(user);
        _userRepository.Save();
    }

    public async Task RemoveIngredientsFromUserAsync(IEnumerable<Ingredient> ingredients, int userId)
    {
        var user = (await _userRepository.GetAllInclude(x => x.UserId == userId, x => x.Ingredients)).FirstOrDefault();

        if (user == null)
        {
            return;
        }

        foreach(var ingredient in ingredients)
        {
            user.Ingredients.Remove(ingredient);
        }

        _userRepository.Update(user);
        _userRepository.Save();
    }
}