using AutoMapper;
using Contracts;
using Domain.Agregates.ProductAgregate;
using Microsoft.FSharp.Core;
using Persistence.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Persistence.EntityFramework
{
    public class EntityProductRepository : EntityRepositoryBase, IProductRepository
    {
        public EntityProductRepository(PbDbContext db, IMapper mapper) : base(db, mapper)
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

        public Task<Product?> Get(Expression<Func<Product, bool>>? predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedCollectionBase<Product>> GetAll(int pageNumber, int itemsPerPage, Expression<Func<Product, bool>>? predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAll(Expression<Func<Product, bool>>? predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<Product?> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Update(Product item)
        {
            throw new NotImplementedException();
        }
    }
}
