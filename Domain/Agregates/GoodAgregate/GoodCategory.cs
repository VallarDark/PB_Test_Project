using Contracts;
using Domain.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Agregates.GoodAgregate
{
    public class GoodCategory : EntityBase
    {
        private const int MIN_LENGTH = 2;
        private const int MAX_LENGTH = 25;
        private const int MAX_DESCRIPTION_LENGTH = 500;

        private ICollection<Good> goods;

        public string Name { get; private set; }

        public string Description { get; private set; }

        public ICollection<Good> Goods => goods.ToList();

        public GoodCategory(string name, string description, ICollection<Good> goods)
        {
            Name = EnsuredUtils.EnsureStringLengthIsCorrect(name, MIN_LENGTH, MAX_LENGTH);
            this.goods = EnsuredUtils.EnsureNotNull(goods);
            Description = EnsuredUtils.EnsureStringLengthIsCorrect(description, MIN_LENGTH, MAX_DESCRIPTION_LENGTH);
        }
    }
}
