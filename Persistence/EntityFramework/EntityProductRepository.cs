using AutoMapper;
using Contracts;
using Domain.Agregates.ProductAgregate;
using Domain.Exceptions;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.FSharp.Core;
using Persistence.Entities;
using Persistence.EntityFramework.Context;
using Persistence.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Persistence.EntityFramework
{
    public class EntityProductRepository : EntityRepositoryBase, IProductRepository
    {
        public EntityProductRepository(PbDbContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public async Task<Unit> Create(Product item)
        {
            var entity = new ProductEntity(item);

            _Db.Products.Add(entity);

            await _Db.SaveChangesAsync();

            return default;
        }

        public async Task<Unit> Delete(string id)
        {
            var item = await _Db.Products
                .FirstOrDefaultAsync(u => u.Id == id);

            if (item == null)
            {
                throw new ItemNotExistsException(
                    string.Format(ITEM_NOT_EXISTS_EXCEPTION, nameof(Product)));
            }

            _Db.Products.Remove(item);

            await _Db.SaveChangesAsync();

            return default;
        }

        public async Task<Product?> Get(
            Expression<Func<Product, bool>>? predicate = null,
            bool addInnerItems = false)
        {
            ProductEntity? result;

            IQueryable<ProductEntity> items = _Db.Products;

            if (addInnerItems)
            {
                items = items.Include(r => r.Categories);
            }

            if (predicate == null)
            {
                result = await items.FirstOrDefaultAsync();
            }
            else
            {
                var mappedPredicate =
                    _Mapper.Map<Expression<Func<ProductEntity, bool>>>(predicate);

                result = await items.FirstOrDefaultAsync(mappedPredicate);
            }

            if (result == null)
            {
                return null;
            }

            return new Product(_Mapper.Map<ProductDto>(result));
        }

        public async Task<PaginatedCollectionBase<Product>> GetPage(
            int pageNumber,
            int itemsPerPage,
            Expression<Func<Product, bool>>? predicate = null,
            bool addInnerItems = false)
        {
            EnsuredUtils.EnsureNumberIsMoreOrEqualValue(pageNumber, 1);
            EnsuredUtils.EnsureNumberIsMoreOrEqualValue(itemsPerPage, 1);

            IQueryable<ProductEntity> result = _Db.Products;

            if (addInnerItems)
            {
                result = result.Include(r => r.Categories);
            }

            if (predicate != null)
            {
                var mappedPredicate =
                    _Mapper.Map<Expression<Func<ProductEntity, bool>>>(predicate);

                result = result
                    .Where(mappedPredicate);
            }

            var skipItems = (pageNumber - 1) * itemsPerPage;

            result = result.Skip(skipItems).Take(itemsPerPage);

            var collection = await result
                .Select(u => new Product(_Mapper.Map<ProductDto>(u)))
                .ToListAsync();

            return new PaginatedCollection<Product>(
                collection,
                collection.Count == itemsPerPage);
        }

        public async Task<IEnumerable<Product>> GetAll(
            Expression<Func<Product, bool>>? predicate = null,
            bool addInnerItems = false)
        {
            IQueryable<ProductEntity> result = _Db.Products;

            if (addInnerItems)
            {
                result = result.Include(r => r.Categories);
            }

            if (predicate != null)
            {
                var mappedPredicate =
                    _Mapper.Map<Expression<Func<ProductEntity, bool>>>(predicate);

                result = result
                    .Where(mappedPredicate);
            }

            result = result.Take(ITEMS_LIMIT);

            return await result
                .Select(u => new Product(_Mapper.Map<ProductDto>(u)))
                .ToListAsync();
        }

        public async Task<Product?> GetById(
            string id,
            bool addInnerItems = false)
        {
            ProductEntity? result;

            IQueryable<ProductEntity> items = _Db.Products;

            if (addInnerItems)
            {
                items = items.Include(r => r.Categories);
            }

            result = await items.FirstOrDefaultAsync(i => i.Id == id);

            if (result == null)
            {
                return null;
            }

            return new Product(_Mapper.Map<ProductDto>(result));
        }

        public async Task<Unit> Update(Product item)
        {
            var existingItem = await _Db.Products
                .Include(u => u.Categories)
                .FirstOrDefaultAsync(u => u.Id == item.Id);

            if (existingItem == null)
            {
                throw new ItemNotExistsException(string.Format(
                    ITEM_NOT_EXISTS_EXCEPTION,
                    nameof(Product)));
            }

            existingItem.Update(item);

            var newItemCategories = item.Categories
                .Select(p => new ProductCategoryEntity(p))
                .ToList();

            if (!CollectionUtils.AreCollectionsSame(
                existingItem.Categories,
                newItemCategories,
                out var needToRemove,
                out var needToAdd,
                out var neeedToUpdate))
            {
                foreach (var itemToRemove in needToRemove)
                {
                    existingItem.Categories.Remove(itemToRemove);
                }

                foreach (var itemToUpdate in neeedToUpdate)
                {
                    var existingItemMember = existingItem.Categories
                        .FirstOrDefault(m => m.Id == itemToUpdate.Id);

                    if (existingItemMember != null)
                    {
                        existingItemMember.Update(itemToUpdate);
                    }
                }

                foreach (var itemToAdd in needToAdd)
                {
                    var notExistingItemMember = _Db.Categories
                        .FirstOrDefault(m => m.Id == itemToAdd.Id);

                    if (notExistingItemMember != null)
                    {
                        existingItem.Categories.Add(notExistingItemMember);
                    }
                }
            }

            _Db.Products.Update(existingItem);

            await _Db.SaveChangesAsync();

            return default;
        }
    }
}
