using PresentationModels.Models;

namespace PB_Blazor.Pages
{
    public partial class AccountComponent
    {
        private UserLoginModel userLogin;
        private UserRegistrationModel userRegistration;

        private bool isRegistrationForm;
        private string? error;
        private string? returnUrl;

        public AccountComponent()
        {
            isRegistrationForm = false;
            userLogin = new UserLoginModel();
            userRegistration = new UserRegistrationModel();
        }

        private async Task OnLoginAsync()
        {
            try
            {
                var result = await _userManager.LogIn(userLogin);

                if (result != null)
                {
                    await _storage.SetAsync(nameof(UserInfoDto), result);
                }

                returnUrl = (await _storage.GetAsync<string>(nameof(returnUrl))).Value;

                if (returnUrl != null)
                {
                    await _storage.SetAsync(nameof(returnUrl), null);

                    _navigationManager.NavigateTo(returnUrl, true);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                StateHasChanged();
            }
        }

        private async Task OnRegistrationAsync()
        {
            try
            {
                var result = await _userManager.Registration(userRegistration);

                if (result != null)
                {
                    await _storage.SetAsync(nameof(UserInfoDto), result);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                StateHasChanged();
            }
        }

        private void ChangeForm()
        {
            isRegistrationForm = !isRegistrationForm;
            StateHasChanged();
        }

        private async Task LogOut()
        {
            await _storage.SetAsync(nameof(UserInfoDto), null);
            _userManager.LogOut();
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
