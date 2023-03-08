using PresentationModels.Models;

namespace PB_Blazor.Components
{
    public partial class SignInComponent
    {
        private bool isAuthorized;
        private string? returnUrl;

        public SignInComponent()
        {
            isAuthorized = false;
        }

        private async void LogOut()
        {
            await _userManager.LogOut();
            GoToAccount();
        }

        protected override async Task OnAfterRenderAsync(bool isFirst)
        {
            if (isFirst)
            {
                var userInfoDto = (await _storage.GetAsync<UserInfoDto>(nameof(UserInfoDto))).Value;

                if (userInfoDto != null)
                {
                    _userManager.SetUserInfoFromLocale(userInfoDto);

                    await _userManager.RefreshToken(userInfoDto.TokenDto);
                }
            }
        }

        private void GoToAccount()
        {
            _navigationManager.NavigateTo($"/account");
        }

        private async void UpdateLocaleUserInfo()
        {
            await _storage.SetAsync(nameof(UserInfoDto), _userManager.UserInfo);
            StateHasChanged();
        }

        private async void NavigateToReturnUrl()
        {
            returnUrl = (await _storage.GetAsync<string>(nameof(returnUrl))).Value;

            if (returnUrl != null)
            {
                await _storage.SetAsync(nameof(returnUrl), null);

                _navigationManager.NavigateTo(returnUrl, true);
            }
        }

        private async void SaveReturnUrl(string? returnUrl)
        {
            await _storage.SetAsync(nameof(returnUrl), returnUrl);
        }

        protected override void OnInitialized()
        {
            _userManager.OnUserInfoChanged += UpdateLocaleUserInfo;
            _userManager.OnLogin += NavigateToReturnUrl;
            _userManager.OnRefreshTokenExpired += SaveReturnUrl;
        }

        public void Dispose()
        {
            _userManager.OnUserInfoChanged -= UpdateLocaleUserInfo;
            _userManager.OnLogin -= NavigateToReturnUrl;
            _userManager.OnRefreshTokenExpired -= SaveReturnUrl;
        }
    }
}
