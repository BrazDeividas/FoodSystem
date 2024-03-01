using System.Security.Claims;

namespace FoodSystemClient.Authentication;

public class AuthenticationService
{
    public event Action<ClaimsPrincipal>? UserChanged;
    private ClaimsPrincipal? _currentUser;
    public ClaimsPrincipal CurrentUser
    {
        get { return _currentUser ?? new(); }
        set
        {
            _currentUser = value;

            if(UserChanged != null)
            {
                UserChanged(_currentUser);
            }
        }
    }
}