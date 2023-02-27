using Contracts;
using Domain.Utils;
using Microsoft.FSharp.Core;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Agregates.ProductAgregate
{
    public class ProductCategory : ValueObject
    {
        private const int MIN_LENGTH = 2;
        private const int MAX_LENGTH = 25;
        private const int MAX_DESCRIPTION_LENGTH = 500;

        private ICollection<Product> products;

        public string Name { get; private set; }

        public string Description { get; private set; }

        public ICollection<Product> Products => products.ToList();

        public ProductCategory(string name, string description, ICollection<Product> products)
        {
            Name = EnsuredUtils.EnsureStringLengthIsCorrect(name, MIN_LENGTH, MAX_LENGTH);
            this.products = EnsuredUtils.EnsureNotNull(products);
            Description = EnsuredUtils.EnsureStringLengthIsCorrect(description, MIN_LENGTH, MAX_DESCRIPTION_LENGTH);
        }

        public Unit ChangeName(string name)
        {
            EnsuredUtils.EnsureNewValueIsNotSame(Name, name);

            Name = EnsuredUtils.EnsureStringLengthIsCorrect(name, MIN_LENGTH, MAX_LENGTH);

            return default;
        }

        public Unit ChangeDescription(string description)
        {
            EnsuredUtils.EnsureNewValueIsNotSame(Description, description);

            Description = EnsuredUtils.EnsureStringLengthIsCorrect(description, MIN_LENGTH, MAX_DESCRIPTION_LENGTH);

            return default;
        }

        public Unit AddProduct(Product product)
        {
            EnsuredUtils.EnsureNotNull(product);

            EnsuredUtils.EnsureCollectionNotContainsItem(products, product);

            products.Add(product);

            return default;
        }

        public Unit RemoveProduct(Product product)
        {
            EnsuredUtils.EnsureNotNull(product);

            EnsuredUtils.EnsureCollectionContainsItem(products, product);

            products.Remove(product);

            return default;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Description;
        }
    }
}
