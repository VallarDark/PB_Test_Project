using AutoMapper;
using Contracts;
using Domain.Agregates.ProductAgregate;
using Domain.Exceptions;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.FSharp.Core;
using Persistence.EntityFramework.Context;
using Persistence.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Persistence.EntityFramework
{
    public class EntityProductCategoryRepository : EntityRepositoryBase, IProductCategoryRepository
    {
        private const string ITEM_NOT_EXISTS_EXCEPTION = "Current {0} does not exists";

        public EntityProductCategoryRepository(
            PbDbContext db,
            IMapper mapper) : base(db, mapper)
        {
        }

        public async Task<Unit> Create(ProductCategory item)
        {
            var entity = new ProductCategoryEntity(item);

            _Db.Categories.Add(entity);

            await _Db.SaveChangesAsync();

            return default;
        }

        public async Task<Unit> Delete(string id)
        {
            var item = await _Db.Categories
                .FirstOrDefaultAsync(u => u.Id == id);

            if (item == null)
            {
                throw new ItemNotExistsException(
                    string.Format(ITEM_NOT_EXISTS_EXCEPTION, nameof(ProductCategory)));
            }

            _Db.Categories.Remove(item);

            await _Db.SaveChangesAsync();

            return default;
        }

        public async Task<ProductCategory?> Get(
            Expression<Func<ProductCategory, bool>>? predicate = null,
            bool addInnerItems = false)
        {
            ProductCategoryEntity? result;

            IQueryable<ProductCategoryEntity> items = _Db.Categories;

            if (addInnerItems)
            {
                items.Include(r => r.Products);
            }

            if (predicate == null)
            {
                result = await items.FirstOrDefaultAsync();
            }
            else
            {
                var mappedPredicate =
                    _Mapper.Map<Expression<Func<ProductCategoryEntity, bool>>>(predicate);

                result = await items.FirstOrDefaultAsync(mappedPredicate);
            }

            if (result == null)
            {
                return null;
            }

            return new ProductCategory(_Mapper.Map<ProductCategoryDto>(result));
        }

        public async Task<PaginatedCollectionBase<ProductCategory>> GetPage(
            int pageNumber,
            int itemsPerPage,
            Expression<Func<ProductCategory, bool>>? predicate = null,
            bool addInnerItems = false)
        {
            EnsuredUtils.EnsureNumberIsMoreOrEqualValue(pageNumber, 1);
            EnsuredUtils.EnsureNumberIsMoreOrEqualValue(itemsPerPage, 1);

            IQueryable<ProductCategoryEntity> result = _Db.Categories; ;

            if (addInnerItems)
            {
                result = result.Include(r => r.Products);
            }

            if (predicate != null)
            {
                var mappedPredicate =
                    _Mapper.Map<Expression<Func<ProductCategoryEntity, bool>>>(predicate);

                result = result
                    .Where(mappedPredicate);
            }

            var skipItems = (pageNumber - 1) * itemsPerPage;

            result = result.Skip(skipItems).Take(itemsPerPage);

            var collection = await result
                .Select(u => new ProductCategory(_Mapper.Map<ProductCategoryDto>(u)))
                .ToListAsync();

            return new PaginatedCollection<ProductCategory>(
                collection,
                collection.Count == itemsPerPage);
        }

        public async Task<IEnumerable<ProductCategory>> GetAll(
            Expression<Func<ProductCategory, bool>>? predicate = null,
            bool addInnerItems = false)
        {
            IQueryable<ProductCategoryEntity> result = _Db.Categories; ;

            if (addInnerItems)
            {
                result = result.Include(r => r.Products);
            }

            if (predicate != null)
            {
                var mappedPredicate =
                    _Mapper.Map<Expression<Func<ProductCategoryEntity, bool>>>(predicate);

                result = result
                    .Where(mappedPredicate);
            }

            result = result.Take(ITEMS_LIMIT);

            return await result
                .Select(u => new ProductCategory(_Mapper.Map<ProductCategoryDto>(u)))
                .ToListAsync();
        }

        public async Task<ProductCategory?> GetById(
            string id,
            bool addInnerItems = false)
        {
            ProductCategoryEntity? result;

            IQueryable<ProductCategoryEntity> items = _Db.Categories;

            if (addInnerItems)
            {
                items.Include(r => r.Products);
            }

            result = await items.FirstOrDefaultAsync(i => i.Id == id);

            if (result == null)
            {
                return null;
            }

            return new ProductCategory(_Mapper.Map<ProductCategoryDto>(result));
        }

        public async Task<Unit> Update(ProductCategory item)
        {
            var existingItem = await _Db.Categories
                .Include(u => u.Products)
                .FirstOrDefaultAsync(u => u.Id == item.Id);

            if (existingItem == null)
            {
                throw new ItemNotExistsException(
                    string.Format(ITEM_NOT_EXISTS_EXCEPTION, nameof(ProductCategory)));
            }

            existingItem.Update(item);

            var newItemProducts = item.Products
                .Select(p => new ProductEntity(p))
                .ToList();

            if (!CollectionUtils.AreCollectionsSame(
                existingItem.Products,
                newItemProducts,
                out var needToRemove,
                out var needToAdd,
                out var neeedToUpdate))
            {
                foreach (var itemToRemove in needToRemove)
                {
                    existingItem.Products.Remove(itemToRemove);
                }

                foreach (var itemToUpdate in neeedToUpdate)
                {
                    var existingItemMember = existingItem.Products
                        .FirstOrDefault(m => m.Id == itemToUpdate.Id);

                    if (existingItemMember != null)
                    {
                        existingItemMember.Update(itemToUpdate);
                    }
                }

                foreach (var itemToAdd in needToAdd)
                {
                    var notExistingItemMember = _Db.Products
                        .FirstOrDefault(m => m.Id == itemToAdd.Id);

                    if (notExistingItemMember != null)
                    {
                        existingItem.Products.Add(notExistingItemMember);
                    }
                }
            }

            _Db.Categories.Update(existingItem);

            await _Db.SaveChangesAsync();

            return default;
        }
    }
}
