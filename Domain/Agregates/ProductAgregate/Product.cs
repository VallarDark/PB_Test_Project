using Contracts;
using Domain.Utils;
using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Agregates.ProductAgregate
{
    public class Product : ValueObject, IEntity
    {
        private const int MIN_LENGTH = 2;
        private const int MAX_LENGTH = 25;
        private const int MAX_DESCRIPTION_LENGTH = 500;
        private const int MAX_URL_LENGTH = 50;

        private ICollection<ProductCategory> categories = new List<ProductCategory>();

        private string id = Guid.NewGuid().ToString();

        public string Id => id;

        public string Title { get; private set; }

        public ICollection<ProductCategory> Categories => categories.ToList();

        public string Description { get; private set; }

        public string ImgUrl { get; private set; }

        public double Price { get; private set; }

        public Product(
            string title,
            ICollection<ProductCategory> categories,
            string description,
            string imgUrl,
            double price)
        {
            Title = EnsuredUtils.EnsureStringLengthIsCorrect(title, MIN_LENGTH, MAX_LENGTH);
            this.categories = EnsuredUtils.EnsureNotNull(categories);
            Description = EnsuredUtils.EnsureStringLengthIsCorrect(description, MIN_LENGTH, MAX_DESCRIPTION_LENGTH);
            ImgUrl = EnsuredUtils.EnsureStringLengthIsCorrect(imgUrl, MIN_LENGTH, MAX_URL_LENGTH);
            Price = EnsuredUtils.EnsureNumberIsMoreOrEqualValue(price, 0);
        }

        public Product(ProductDto productDto)
        {
            id = EnsuredUtils.EnsureStringIsNotEmpty(productDto.Id);
            Title = EnsuredUtils.EnsureStringLengthIsCorrect(productDto.Title, MIN_LENGTH, MAX_LENGTH);
            categories = EnsuredUtils.EnsureNotNull(productDto.Categories.Select(c => new ProductCategory(c)).ToList());
            Description = EnsuredUtils.EnsureStringLengthIsCorrect(productDto.Description, MIN_LENGTH, MAX_DESCRIPTION_LENGTH);
            ImgUrl = EnsuredUtils.EnsureStringLengthIsCorrect(productDto.ImgUrl, MIN_LENGTH, MAX_URL_LENGTH);
            Price = EnsuredUtils.EnsureNumberIsMoreOrEqualValue(productDto.Price, 0);
        }

        public Unit ChangeDescription(string description)
        {
            EnsuredUtils.EnsureNewValueIsNotSame(Description, description);

            Description = EnsuredUtils.EnsureStringLengthIsCorrect(description, MIN_LENGTH, MAX_DESCRIPTION_LENGTH);

            return default;
        }

        public Unit ChangePrice(double price)
        {
            EnsuredUtils.EnsureNewValueIsNotSame(Price, price);

            Price = EnsuredUtils.EnsureNumberIsMoreOrEqualValue(price, 0);

            return default;
        }

        public Unit ChangeImage(string imgUrl)
        {
            EnsuredUtils.EnsureNewValueIsNotSame(ImgUrl, imgUrl);

            ImgUrl = EnsuredUtils.EnsureStringLengthIsCorrect(imgUrl, MIN_LENGTH, MAX_URL_LENGTH);

            return default;
        }

        public Unit ChangeTitle(string title)
        {
            EnsuredUtils.EnsureNewValueIsNotSame(Title, title);

            Title = EnsuredUtils.EnsureStringLengthIsCorrect(title, MIN_LENGTH, MAX_LENGTH);

            return default;
        }

        public Unit AddCategory(ProductCategory category)
        {
            EnsuredUtils.EnsureNotNull(category);

            EnsuredUtils.EnsureCollectionNotContainsItem(categories, category);

            categories.Add(category);

            return default;
        }

        public Unit RemoveCategory(ProductCategory category)
        {
            EnsuredUtils.EnsureNotNull(category);

            EnsuredUtils.EnsureCollectionContainsItem(categories, category);

            categories.Remove(category);

            return default;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Title;
            yield return Price;
            yield return Description;
            yield return ImgUrl;
        }
    }
}
