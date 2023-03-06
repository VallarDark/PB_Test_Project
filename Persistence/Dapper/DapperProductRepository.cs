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
    public class DapperProductRepository : DapperRepositoryBase, IProductRepository
    {
        public DapperProductRepository(DapperContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public Task<Unit> Create(Product item)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Product?> Get(Expression<Func<Product, bool>>? predicate = null, bool addInnerItems = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAll(Expression<Func<Product, bool>>? predicate = null, bool addInnerItems = false)
        {
            throw new NotImplementedException();
        }

        public Task<Product?> GetById(string id, bool addInnerItems = false)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedCollectionBase<Product>> GetPage(int pageNumber, int itemsPerPage, Expression<Func<Product, bool>>? predicate = null, bool addInnerItems = false)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Update(Product item)
        {
            throw new NotImplementedException();
        }
    }
}
