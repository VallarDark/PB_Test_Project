using Contracts;
using Domain.Aggregates.ProductAggregate;

namespace PB_Blazor.Pages
{
    public partial class ProductsComponent
    {
        private string? error;
        private PaginatedCollectionBase<Product>? products;
        private int pageNumber;

        public ProductsComponent()
        {
            pageNumber = 1;
            products = null;
        }

        private async Task GetProducts()
        {
            try
            {
                error = null;

                products = await _productsManager.GetProducts(pageNumber);
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
