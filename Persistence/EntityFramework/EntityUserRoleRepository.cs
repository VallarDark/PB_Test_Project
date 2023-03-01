using AutoMapper;
using Contracts;
using Domain.Agregates.UserAgregate;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using Persistence.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Persistence.EntityFramework
{
    public class EntityUserRoleRepository : EntityRepositoryBase, IUserRoleRepository
    {
        public EntityUserRoleRepository(PbDbContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public async Task<UserRole?> Get(Expression<Func<UserRole, bool>>? predicate = null)
        {
            UserRoleEntity? result;

            if (predicate == null)
            {
                result = await _Db.Roles.FirstOrDefaultAsync();
            }
            else
            {
                var mappedPredicate = _Mapper.Map<Expression<Func<UserRoleEntity, bool>>>(predicate);

                result = await _Db.Roles.FirstOrDefaultAsync(mappedPredicate);
            }

            if (result == null)
            {
                return null;
            }

            return new UserRole(_Mapper.Map<UserRoleDto>(result));
        }

        public async Task<PaginatedCollectionBase<UserRole>> GetAll(
            int pageNumber,
            int itemsPerPage,
            Expression<Func<UserRole, bool>>? predicate = null)
        {
            EnsuredUtils.EnsureNumberIsMoreOrEqualValue(pageNumber, 1);
            EnsuredUtils.EnsureNumberIsMoreOrEqualValue(itemsPerPage, 1);

            IQueryable<UserRoleEntity> result;

            var skipItems = (pageNumber - 1) * itemsPerPage;

            if (predicate == null)
            {
                result = _Db.Roles
                    .Skip(skipItems)
                    .Take(itemsPerPage);
            }
            else
            {
                var mappedPredicate = _Mapper.Map<Expression<Func<UserRoleEntity, bool>>>(predicate);

                result = _Db.Roles
                        .Where(mappedPredicate)
                        .Skip(skipItems)
                        .Take(itemsPerPage);
            }

            var collection = await result.Select(u => new UserRole(_Mapper.Map<UserRoleDto>(u)))
                .ToListAsync();

            return new PaginatedCollection<UserRole>(collection, collection.Count == itemsPerPage);
        }

        public async Task<IEnumerable<UserRole>> GetAll(Expression<Func<UserRole, bool>>? predicate = null)
        {
            IQueryable<UserRoleEntity> result;

            if (predicate == null)
            {
                result = _Db.Roles.Take(ITEMS_LIMIT); ;
            }
            else
            {
                var mappedPredicate = _Mapper.Map<Expression<Func<UserRoleEntity, bool>>>(predicate);

                result = _Db.Roles.Where(mappedPredicate).Take(ITEMS_LIMIT);
            }
            return await result.Select(u => new UserRole(_Mapper.Map<UserRoleDto>(u)))
                .ToListAsync();
        }

        public async Task<UserRole?> GetById(string id)
        {
            var result = await _Db.Roles.FirstOrDefaultAsync(ur => ur.Id == id);

            if (result == null)
            {
                return null;
            }

            return new UserRole(_Mapper.Map<UserRoleDto>(result));
        }
    }
}
