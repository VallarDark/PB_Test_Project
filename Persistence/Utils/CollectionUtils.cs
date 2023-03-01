using Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Persistence.Utils
{
    public static class CollectionUtils
    {
        public static bool AreCollectionsSame<T>(
            ICollection<T> oldCollection,
            ICollection<T> newCollection,
            out ICollection<T> needToRemove,
            out ICollection<T> needToAdd,
            out ICollection<T> needToUpdate


            ) where T : ValueObject, IEntity
        {
            needToRemove = new List<T>();
            needToAdd = new List<T>();
            needToUpdate = new List<T>();

            foreach (var item in oldCollection)
            {
                var sameIdItem = newCollection.FirstOrDefault(item => item.Id == item.Id);

                if (sameIdItem == default)
                {
                    needToRemove.Add(item);
                }
                else
                {
                    if (!sameIdItem.Equals(item))
                    {
                        needToUpdate.Add(item);
                    }
                }
            }

            foreach (var item in newCollection)
            {
                var sameIdItem = oldCollection.FirstOrDefault(item => item.Id == item.Id);

                if (sameIdItem == default)
                {
                    needToAdd.Add(item);
                }
            }

            return needToRemove.Count == 0
                && needToAdd.Count == 0
                && needToUpdate.Count == 0;
        }
    }
}
