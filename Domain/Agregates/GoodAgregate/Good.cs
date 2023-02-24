using Contracts;
using Domain.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Agregates.GoodAgregate
{
    public class Good : EntityBase
    {
        private const int MIN_LENGTH = 2;
        private const int MAX_LENGTH = 25;
        private const int MAX_DESCRIPTION_LENGTH = 500;
        private const int MAX_URL_LENGTH = 50;

        private ICollection<GoodCategory> categories;

        public string Title { get; private set; }

        public ICollection<GoodCategory> Categories => categories.ToList();

        public string Description { get; private set; }

        public string ImgUrl { get; private set; }

        public double Price { get; private set; }

        public Good(
            string title,
            ICollection<GoodCategory> categories,
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

    }
}
