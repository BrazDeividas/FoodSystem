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

    public async Task<UserMetrics> AddUserMetricsAsync(PostUserMetricsDto userMetrics)
    {
        var user = await _userRepository.GetById(userMetrics.UserId);
        if (user == null)
        {
            return null;
        }

        var userMetric = await _userMetricsRepository.GetAll(x => x.UserId == userMetrics.UserId);
        if (userMetric.Any())
        {
            return null;
        }
        var userMetricsEntity = _mapper.Map<PostUserMetricsDto, UserMetrics>(userMetrics);
        await _userMetricsRepository.Add(userMetricsEntity);
        _userMetricsRepository.Save();

        return userMetricsEntity;
    }

    public async Task<UserMetrics> UpdateUserMetricsAsync(PostUserMetricsDto userMetrics)
    {
        var entity = await _userMetricsRepository.GetById(userMetrics.UserId);
        if (entity == null)
        {
            var user = await _userRepository.GetById(userMetrics.UserId);
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
}