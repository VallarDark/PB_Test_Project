using Contracts;
using Domain.Agregates.UserAgregate;
using Microsoft.FSharp.Core;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Persistence.Dapper
{
    public class DapperUserRepository : IUserRepository
    {
        public string ServiceType => RepositoryType.Dapper.ToString();

        public Task<Unit> Create(User item)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<User> Get(Expression<Func<User, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedCollectionBase<User>> GetAll(int pageNumber, int itemsPerPage, Func<bool, User> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedCollectionBase<User>> GetAll(Func<bool, User> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetById(string id, Expression<Func<User, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Update(User item)
        {
            throw new NotImplementedException();
        }
    }
}
