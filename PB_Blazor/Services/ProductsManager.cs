using Contracts;
using Domain.Aggregates.ProductAggregate;
using Domain.Exceptions;
using Domain.Utils;

namespace PB_Blazor.Services
{
    public class ProductsManager
    {
        private readonly RequestSender _sender;
        private readonly RoutingManager _routingManager;
        private readonly UserManager _userManager;

        public ProductsManager(
            RequestSender sender,
            RoutingManager routingManager,
            UserManager userManager)
        {
            _sender = sender;
            _routingManager = routingManager;
            _userManager = userManager;
        }

        public async Task<PaginatedCollectionBase<Product>?> GetProducts(int pageNumber)
        {
            EnsuredUtils.EnsureNotNull(_userManager);
            EnsuredUtils.EnsureNotNull(_userManager.UserInfo);
            EnsuredUtils.EnsureNumberIsMoreOrEqualValue(pageNumber, 1);

            var requestUrl =
                _routingManager.GetRoute(RoutingManager.Endpoint.GetProducts, pageNumber.ToString());

            PaginatedCollectionBase<Product>? result = null;

            try
            {
                result = await _sender.Send<PaginatedCollectionBase<Product>>(
                    requestUrl,
                    _userManager?.UserInfo?.TokenDto?.Token,
                    RequestSender.RequestType.HttpGet,
                    null);
            }
            catch (TokenExpiredException)
            {
                await _userManager.RefreshToken("products");

                if (!_userManager.IsRefreshTokenFailed)
                {
                    await GetProducts(pageNumber);
                }
            }

            return result;
        }

    }
}
