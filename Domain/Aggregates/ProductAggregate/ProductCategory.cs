using Contracts;
using Domain.Utils;
using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Aggregates.ProductAggregate
{
    public class ProductCategory : ValueObject, IEntity
    {
        public const int MIN_LENGTH = 2;
        public const int MAX_LENGTH = 25;
        public const int MAX_DESCRIPTION_LENGTH = 500;

        private ICollection<Product> products = new List<Product>();

        private string id = Guid.NewGuid().ToString();

        public string Id => id;

        public string Name { get; private set; }

        public string Description { get; private set; }

        public ICollection<Product> Products => products.ToList();

        public ProductCategory(string name, string description)
        {
            Name = EnsuredUtils.EnsureStringLengthIsCorrect(
                name,
                MIN_LENGTH,
                MAX_LENGTH);

            Description = EnsuredUtils.EnsureStringLengthIsCorrect(
                description,
                MIN_LENGTH,
                MAX_DESCRIPTION_LENGTH);

            products = new List<Product>();
        }

        public ProductCategory(ProductCategoryDto categoryDto)
        {
            id = EnsuredUtils.EnsureStringIsNotEmpty(categoryDto.Id);

            Name = EnsuredUtils.EnsureStringLengthIsCorrect(
                categoryDto.Name,
                MIN_LENGTH,
                MAX_LENGTH);

            if (categoryDto.CyclicDepth < 1)
            {
                products = new List<Product>();
            }
            else
            {
                EnsuredUtils.EnsureNotNull(categoryDto.Products);

                foreach (var item in categoryDto.Products)
                {
                    item.CyclicDepth--;
                }

                products = categoryDto.Products.Select(c => new Product(c)).ToList();
            }

            Description = EnsuredUtils.EnsureStringLengthIsCorrect(
                categoryDto.Description,
                MIN_LENGTH,
                MAX_DESCRIPTION_LENGTH);
        }

        public Unit ChangeName(string name)
        {
            Name = EnsuredUtils.EnsureStringLengthIsCorrect(
                name,
                MIN_LENGTH,
                MAX_LENGTH);

            return default;
        }

        public Unit ChangeDescription(string description)
        {
            Description = EnsuredUtils.EnsureStringLengthIsCorrect(
                description,
                MIN_LENGTH,
                MAX_DESCRIPTION_LENGTH);

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
