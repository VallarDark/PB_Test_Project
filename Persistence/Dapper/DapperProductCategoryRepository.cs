using AutoMapper;
using Contracts;
using Domain.Aggregates.ProductAggregate;
using Microsoft.FSharp.Core;
using Persistence.Dapper.Context;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Persistence.Dapper
{
    public class DapperProductCategoryRepository : DapperRepositoryBase, IProductCategoryRepository
    {
        public DapperProductCategoryRepository(DapperContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public Task<Unit> Create(ProductCategory item)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductCategory?> Get(Expression<Func<ProductCategory, bool>>? predicate = null, bool addInnerItems = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductCategory>> GetAll(Expression<Func<ProductCategory, bool>>? predicate = null, bool addInnerItems = false)
        {
            throw new NotImplementedException();
        }

        public Task<ProductCategory?> GetById(string id, bool addInnerItems = false)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedCollectionBase<ProductCategory>> GetPage(int pageNumber, int itemsPerPage, Expression<Func<ProductCategory, bool>>? predicate = null, bool addInnerItems = false)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Update(ProductCategory item)
        {
            throw new NotImplementedException();
        }
    }
}
