using PresentationModels.Models;

namespace PB_Blazor.Services
{
    public class UserManager
    {
        public event Action? OnChanged;

        private readonly RequestSender _sender;
        private readonly RoutingManager _routingManager;

        public UserInfoDto? UserInfo { get; private set; }

        public bool IsAuthorized => UserInfo != null
            && UserInfo.TokenDto != null
            && !string.IsNullOrWhiteSpace(UserInfo.TokenDto.RefreshToken)
            && !string.IsNullOrWhiteSpace(UserInfo.TokenDto.Token);

        public UserManager(RequestSender sender, RoutingManager routingManager)
        {
            _sender = sender;
            _routingManager = routingManager;
        }

        public async Task<UserInfoDto?> LogIn(UserLoginModel? userLogin)
        {
            var requestUrl =
                _routingManager.GetRoute(RoutingManager.Endpoint.Login);

            UserInfo = await _sender.Send<UserInfoDto>(
                requestUrl,
                null,
                RequestSender.RequestType.HttpPost,
                userLogin);

            OnChanged?.Invoke();

            return UserInfo;
        }

        public async Task<UserInfoDto?> Registration(UserRegistrationModel? userRegistration)
        {
            var requestUrl =
                _routingManager.GetRoute(RoutingManager.Endpoint.Registration);

            UserInfo = await _sender.Send<UserInfoDto>(
                requestUrl,
                null,
                RequestSender.RequestType.HttpPost,
                userRegistration);

            OnChanged?.Invoke();

            return UserInfo;
        }

        public void LogIn(UserInfoDto? userInfoDto)
        {
            if (userInfoDto == null)
            {
                return;
            }

            UserInfo = userInfoDto;
            OnChanged?.Invoke();
        }

        public void LogOut()
        {
            UserInfo = null;
            OnChanged?.Invoke();
        }
    }
}
