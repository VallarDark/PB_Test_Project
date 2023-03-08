using Contracts;
using Domain.Aggregates.UserAggregate;
using Domain.Utils;
using PresentationModels.Models;

namespace PB_Blazor.Services
{
    public class UserManager
    {
        public event Action? OnUserInfoChanged;
        public event Action<string?>? OnRefreshTokenExpired;
        public event Action? OnLogin;

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
            _sender.OnTokenExpired += RefreshToken;
            _sender.OnTokenInvalid += LogOut;
        }

        ~UserManager()
        {
            _sender.OnTokenExpired -= RefreshToken;
            _sender.OnTokenInvalid -= LogOut;
        }

        public async Task<UserInfoDto?> LogIn(UserLoginModel? userLogin)
        {
            var requestUrl =
                _routingManager.GetRoute(RoutingManager.Endpoint.Login);

            var request = new RequestSender.Request
            {
                Url = requestUrl,
                AuthToken = null,
                Type = RequestSender.RequestType.HttpPost,
                Data = userLogin
            };

            UserInfo = await _sender.Send<UserInfoDto>(request);

            OnUserInfoChanged?.Invoke();
            OnLogin?.Invoke();

            return UserInfo;
        }

        public void LogIn(UserInfoDto? userInfoDto)
        {
            if (userInfoDto == null)
            {
                return;
            }

            UserInfo = userInfoDto;
            OnUserInfoChanged?.Invoke();
        }

        public void SetUserInfoFromLocale(UserInfoDto? userInfoDto)
        {
            UserInfo = userInfoDto;
        }

        public async Task<UserInfoDto?> Registration(UserRegistrationModel? userRegistration)
        {
            var requestUrl =
                _routingManager.GetRoute(RoutingManager.Endpoint.Registration);

            var request = new RequestSender.Request
            {
                Url = requestUrl,
                AuthToken = null,
                Type = RequestSender.RequestType.HttpPost,
                Data = userRegistration
            };

            UserInfo = await _sender.Send<UserInfoDto>(request);

            OnUserInfoChanged?.Invoke();

            return UserInfo;
        }

        public async Task<UserInfoDto?> ChangeRepositoryType(RepositoryType repositoryType)
        {
            EnsuredUtils.EnsureNotNull(UserInfo);

            var requestUrl =
                _routingManager.GetRoute(RoutingManager.Endpoint.ChangeRepository);

            var request = new RequestSender.Request
            {
                Url = requestUrl,
                AuthToken = UserInfo.TokenDto?.Token,
                Type = RequestSender.RequestType.HttpPost,
                Data = repositoryType
            };

            var result = await _sender.Send<RepositoryType>(request);

            UserInfo.RepositoryType = result;

            OnUserInfoChanged?.Invoke();

            return UserInfo;
        }

        public async Task<UserInfoDto?> RefreshToken()
        {
            EnsuredUtils.EnsureNotNull(UserInfo);

            return await RefreshToken(UserInfo.TokenDto);
        }

        public async Task<UserInfoDto?> RefreshToken(TokenDto tokenDto)
        {
            EnsuredUtils.EnsureNotNull(tokenDto);

            var requestUrl =
                _routingManager.GetRoute(RoutingManager.Endpoint.RefreshToken);

            var refreshTokenData = new RefreshTokenModel
            {
                RefreshToken = tokenDto.RefreshToken,
                Token = tokenDto.Token
            };

            var request = new RequestSender.Request
            {
                Url = requestUrl,
                AuthToken = null,
                Type = RequestSender.RequestType.HttpPost,
                Data = refreshTokenData
            };

            TokenDto? result = null;

            try
            {
                result = await _sender.Send<TokenDto>(request);

                UserInfo.TokenDto = result;
            }
            catch
            {
                UserInfo = null;
            }
            finally
            {
                OnUserInfoChanged?.Invoke();
            }

            return UserInfo;
        }

        public async Task LogOut()
        {
            var requestUrl =
                _routingManager.GetRoute(RoutingManager.Endpoint.LogOut);

            var request = new RequestSender.Request
            {
                Url = requestUrl,
                AuthToken = UserInfo.TokenDto?.Token,
                Type = RequestSender.RequestType.HttpPost,
                Data = null
            };
            try
            {
                await _sender.Send<string>(request);
            }
            finally
            {
                UserInfo = null;
                OnUserInfoChanged?.Invoke();
            }
        }
    }
}
