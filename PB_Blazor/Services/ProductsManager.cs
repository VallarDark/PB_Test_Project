using Contracts;
using Domain.Aggregates.ProductAggregate;
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
            EnsuredUtils.EnsureNotNull(_userManager?.UserInfo);
            EnsuredUtils.EnsureNumberIsMoreOrEqualValue(pageNumber, 1);

            var requestUrl =
                _routingManager.GetRoute(RoutingManager.Endpoint.GetProducts, pageNumber.ToString());

            var request = new RequestSender.Request
            {
                Url = requestUrl,
                AuthToken = _userManager?.UserInfo.TokenDto?.Token,
                Type = RequestSender.RequestType.HttpGet,
                Data = null
            };

            PaginatedCollectionBase<Product>? result = null;

            result = await _sender.Send<PaginatedCollectionBase<Product>>(request);

            return result;
        }

    }
}
