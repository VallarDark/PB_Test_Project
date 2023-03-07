using Contracts;
using Domain.Aggregates.UserAggregate;
using Domain.Exceptions;
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

        private bool isRefreshTokenFailed;

        public bool IsRefreshTokenFailed => isRefreshTokenFailed;

        public UserInfoDto? UserInfo { get; private set; }

        public bool IsAuthorized => UserInfo != null
            && UserInfo.TokenDto != null
            && !string.IsNullOrWhiteSpace(UserInfo.TokenDto.RefreshToken)
            && !string.IsNullOrWhiteSpace(UserInfo.TokenDto.Token);

        public UserManager(RequestSender sender, RoutingManager routingManager)
        {
            _sender = sender;
            _routingManager = routingManager;
            isRefreshTokenFailed = false;
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

        public async Task<UserInfoDto?> Registration(UserRegistrationModel? userRegistration)
        {
            var requestUrl =
                _routingManager.GetRoute(RoutingManager.Endpoint.Registration);

            UserInfo = await _sender.Send<UserInfoDto>(
                requestUrl,
                null,
                RequestSender.RequestType.HttpPost,
                userRegistration);

            OnUserInfoChanged?.Invoke();

            return UserInfo;
        }

        public async Task<UserInfoDto?> ChangeRepositoryType(RepositoryType repositoryType)
        {
            EnsuredUtils.EnsureNotNull(UserInfo);

            var requestUrl =
                _routingManager.GetRoute(RoutingManager.Endpoint.ChangeRepository);

            RepositoryType? result = null;

            try
            {
                result = await _sender.Send<RepositoryType>(
                    requestUrl,
                    UserInfo.TokenDto?.Token,
                    RequestSender.RequestType.HttpPost,
                    repositoryType);

                UserInfo.RepositoryType = result;

                OnUserInfoChanged?.Invoke();
            }
            catch (TokenExpiredException)
            {
                await RefreshToken();

                if (!isRefreshTokenFailed)
                {
                    await ChangeRepositoryType(repositoryType);
                }
            }

            return UserInfo;
        }

        public async Task<UserInfoDto?> RefreshToken(string? returnUrl = null)
        {
            EnsuredUtils.EnsureNotNull(UserInfo);

            var requestUrl =
                _routingManager.GetRoute(RoutingManager.Endpoint.RefreshToken);

            var refreshTokenData = new RefreshTokenModel
            {
                RefreshToken = UserInfo.TokenDto.RefreshToken,
                Token = UserInfo.TokenDto.Token
            };

            TokenDto? result = null;

            try
            {
                isRefreshTokenFailed = false;

                result = await _sender.Send<TokenDto>(
                    requestUrl,
                    null,
                    RequestSender.RequestType.HttpPost,
                    refreshTokenData);

                UserInfo.TokenDto = result;

                OnUserInfoChanged?.Invoke();
            }
            catch (TokenExpiredException)
            {
                OnRefreshTokenExpired?.Invoke(returnUrl);
                throw;
            }
            catch
            {
                isRefreshTokenFailed = true;
                LogOut();
            }

            return UserInfo;
        }

        public void LogOut()
        {
            UserInfo = null;
            OnUserInfoChanged?.Invoke();
        }
    }
}
