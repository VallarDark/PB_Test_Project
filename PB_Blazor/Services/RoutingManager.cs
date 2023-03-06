using Domain.Utils;

namespace PB_Blazor.Services
{
    public class RoutingManager
    {
        private readonly IConfiguration _configuration;

        private Dictionary<string, string> _routs;

        public enum Endpoint
        {
            Registration,
            Login,
            RefreshToken,
            LogOut,
            ChangeRepository,

            GetCategories,
            CreateCategory,
            UpdateCategory,
            DeleteCategory,
            AddProductToCategory,
            RemoveProductFromCategory,

            GetProducts,
            CreateProduct,
            UpdateProduct,
            DeleteProduct
        }

        public string GetRoute(
            Endpoint endpoint,
            string? routeParameter = null)
        {
            var result = _routs[endpoint.ToString()];

            var mainApiUrl = _configuration["MainApiUrl"];

            EnsuredUtils.EnsureStringIsNotEmpty(mainApiUrl);

            return string.Format(result, mainApiUrl, routeParameter);
        }

        public RoutingManager(IConfiguration configuration)
        {
            _routs = new Dictionary<string, string>
            {
                {
                    Endpoint.Registration.ToString(),
                    "{0}/api/Account/registration"
                },
                {
                    Endpoint.Login.ToString(),
                    "{0}/api/Account/login"
                },
                {
                    Endpoint.RefreshToken.ToString(),
                    "{0}/api/Account/refreshToken"
                },
                {
                    Endpoint.LogOut.ToString(),
                    "{0}/api/Account/logout"
                },
                {
                    Endpoint.GetCategories.ToString(),
                    "{0}/api/Categories/page/{1}"
                },
                {
                    Endpoint.CreateCategory.ToString(),
                    "{0}/api/Categories"
                },
                {
                    Endpoint.UpdateCategory.ToString(),
                    "{0}/api/Categories/{1}"
                },
                {
                    Endpoint.DeleteCategory.ToString(),
                    "{0}/api/Categories/{1}"
                },
                {
                    Endpoint.AddProductToCategory.ToString(),
                    "{0}/api/Categories/{1}/addProduct"
                },
                {
                    Endpoint.RemoveProductFromCategory.ToString(),
                    "{0}/api/Categories/{1}/removeProduct"
                },
                {
                    Endpoint.GetProducts.ToString(),
                    "{0}/api/Products/page/{1}"
                },
                {
                    Endpoint.CreateProduct.ToString(),
                    "{0}/api/Products"
                },
                {
                    Endpoint.UpdateProduct.ToString(),
                    "{0}/api/Products/{1}"
                },
                {
                    Endpoint.DeleteProduct.ToString(),
                    "{0}/api/Products/{1}"
                }
            };

            _configuration = configuration;
        }
    }
}
