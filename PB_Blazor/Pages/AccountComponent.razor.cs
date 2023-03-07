using Contracts;
using PresentationModels.Models;

namespace PB_Blazor.Pages
{
    public partial class AccountComponent
    {
        private UserLoginModel userLogin;
        private UserRegistrationModel userRegistration;

        private RepositoryType repositoryType;
        private bool isRegistrationForm;
        private string? error;

        public AccountComponent()
        {
            isRegistrationForm = false;
            userLogin = new UserLoginModel();
            userRegistration = new UserRegistrationModel();
        }

        private async Task Login()
        {
            try
            {
                error = null;

                await _userManager.LogIn(userLogin);
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

        private async Task Registration()
        {
            try
            {
                error = null;

                await _userManager.Registration(userRegistration);
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
            error = null;

            isRegistrationForm = !isRegistrationForm;
            StateHasChanged();
        }

        private async Task LogOut()
        {
            error = null;
            _userManager.LogOut();
        }

        private async Task SaveChanges()
        {
            try
            {
                error = null;

                await _userManager.ChangeRepositoryType(repositoryType);
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

        protected override void OnInitialized()
        {
            _userManager.OnUserInfoChanged += StateHasChanged;
        }

        public void Dispose()
        {
            _userManager.OnUserInfoChanged -= StateHasChanged;
        }
    }
}
