using AutoMapper;
using Contracts;
using Domain.Agregates.ProductAgregate;
using Microsoft.FSharp.Core;
using Persistence.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Persistence.EntityFramework
{
    public class EntityProductCategoryRepository : EntityRepositoryBase, IProductCategoryRepository
    {
        public EntityProductCategoryRepository(PbDbContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public async Task<Unit> Create(ProductCategory item)
        {
            var entity = new ProductCategoryEntity(item);

            var produ


            var dbRole = _Db.Products.Where(r => r.Id == item.Role.Id);

            entity.Role = dbRole;

            _Db.Add(entity);

            await _Db.SaveChangesAsync();

            return default;
        }

        public Task<Unit> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductCategory?> Get(Expression<Func<ProductCategory, bool>>? predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedCollectionBase<ProductCategory>> GetAll(int pageNumber, int itemsPerPage, Expression<Func<ProductCategory, bool>>? predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductCategory>> GetAll(Expression<Func<ProductCategory, bool>>? predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<ProductCategory?> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Update(ProductCategory item)
        {
            throw new NotImplementedException();
        }
    }
}
