using AutoMapper;
using Contracts;
using Domain.Agregates.UserAgregate;
using Persistence.Dapper.Context;
using Persistence.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Persistence.Dapper
{
    public class DapperUserRoleRepository : DapperRepositoryBase, IUserRoleRepository
    {
        public DapperUserRoleRepository(DapperContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public Task<UserRole?> Get(Expression<Func<UserRole, bool>>? predicate = null, bool addInnerItems = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserRole>> GetAll(Expression<Func<UserRole, bool>>? predicate = null, bool addInnerItems = false)
        {
            throw new NotImplementedException();
        }

        public Task<UserRole?> GetById(string id, bool addInnerItems = false)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedCollectionBase<UserRole>> GetPage(int pageNumber, int itemsPerPage, Expression<Func<UserRole, bool>>? predicate = null, bool addInnerItems = false)
        {
            throw new NotImplementedException();
        }
    }
}
