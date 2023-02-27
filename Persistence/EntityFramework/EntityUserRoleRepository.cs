using Contracts;
using Domain.Agregates.UserAgregate;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Persistence.Dapper
{
    public class EntityUserRoleRepository : IUserRoleRepository
    {
        public string ServiceType => RepositoryType.EntityFramework.ToString();

        public Task<UserRole> Get(Expression<Func<UserRole, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedCollectionBase<UserRole>> GetAll(int pageNumber, int itemsPerPage, Func<bool, UserRole> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedCollectionBase<UserRole>> GetAll(Func<bool, UserRole> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<UserRole> GetById(string id, Expression<Func<UserRole, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }
    }
}
