using Contracts;
using Domain.Agregates.UserAgregate;
using Microsoft.FSharp.Core;
using System;
using System.Linq.Expressions;

namespace Persistence.EntityFramework
{
    public class EntityUserRepository : IUserRepository
    {
        public Unit Create(User item)
        {
            throw new NotImplementedException();
        }

        public Unit Delete(string id)
        {
            throw new NotImplementedException();
        }

        public User Get(Expression<Func<User, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public PaginatedCollectionBase<User> GetAll(int pageNumber, int itemsPerPage, Func<bool, User> predicate = null)
        {
            throw new NotImplementedException();
        }

        public User GetById(string id, Expression<Func<User, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Unit Update(User item)
        {
            throw new NotImplementedException();
        }
    }
}
