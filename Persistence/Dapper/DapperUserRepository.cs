using AutoMapper;
using Contracts;
using Domain.Agregates.UserAgregate;
using Microsoft.FSharp.Core;
using Persistence.Dapper.Context;
using Persistence.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Persistence.Dapper
{
    public class DapperUserRepository : DapperRepositoryBase, IUserRepository
    {
        public DapperUserRepository(DapperContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public Task<Unit> Create(User item)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> Get(
            Expression<Func<User, bool>>? predicate = null,
            bool addInnerItems = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAll(Expression<Func<User, bool>>? predicate = null, bool addInnerItems = false)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetById(string id, bool addInnerItems = false)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedCollectionBase<User>> GetPage(int pageNumber, int itemsPerPage, Expression<Func<User, bool>>? predicate = null, bool addInnerItems = false)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Update(User item)
        {
            throw new NotImplementedException();
        }
    }
}
