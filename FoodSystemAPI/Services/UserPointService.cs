using FoodSystemAPI.Entities;
using FoodSystemAPI.Repositories;

namespace FoodSystemAPI.Services;

public class UserPointService : IUserPointService
{
    private readonly IRepository<UserPoints> _userPointsRepository;

    public UserPointService(IRepository<UserPoints> userPointsRepository)
    {
        _userPointsRepository = userPointsRepository;
    }

    public async Task<UserPoints?> AddUserPoints(int userId, int points)
    {
        var userPointsEntity = (await _userPointsRepository.GetAll(x => x.UserId == userId)).FirstOrDefault();
        
        if(userPointsEntity == null)
        {
            return null;
        }

        userPointsEntity.Points += points;

        _userPointsRepository.Update(userPointsEntity);
        _userPointsRepository.Save();

        return userPointsEntity;
    }

    public async Task<UserPoints?> GetUserPoints(int userId)
    {
        var userPointsEntity = (await _userPointsRepository.GetAll(x => x.UserId == userId)).FirstOrDefault();

        if(userPointsEntity == null)
        {
            return null;
        }

        return userPointsEntity;
    }

    public async Task<UserPoints?> SubtractUserPoints(int userId, int points)
    {
        var userPointsEntity = (await _userPointsRepository.GetAll(x => x.UserId == userId)).FirstOrDefault();

        if(userPointsEntity == null)
        {
            return null;
        }

        if (userPointsEntity.Points < points)
        {
            return null;
        }

        userPointsEntity.Points -= points;

        _userPointsRepository.Update(userPointsEntity);
        _userPointsRepository.Save();

        return userPointsEntity;
    }
}