using PresentationModels.Models;

namespace PB_Blazor.Components
{
    public partial class SignInComponent
    {
        private bool isAuthorized;

        public SignInComponent()
        {
            isAuthorized = false;
        }

        private async Task LogOut()
        {
            await _storage.SetAsync(nameof(UserInfoDto), null);
            _userManager.LogOut();
            GoToAccount();
        }

        protected override async Task OnAfterRenderAsync(bool _)
        {
            var userInfoDto = (await _storage.GetAsync<UserInfoDto>(nameof(UserInfoDto))).Value;

            if (userInfoDto != null)
            {
                _userManager.LogIn(userInfoDto);
            }
        }

        private void GoToAccount()
        {
            _navigationManager.NavigateTo($"/account");
        }

        protected override void OnInitialized()
        {
            _userManager.OnChanged += StateHasChanged;
        }

        public void Dispose()
        {
            _userManager.OnChanged -= StateHasChanged;
        }
    }
}
