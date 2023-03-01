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

        public async Task<UserRole?> Get(
            Expression<Func<UserRole, bool>>? predicate = null,
            bool addInnerItems = false)
        {
            UserRoleEntity? result;

            IQueryable<UserRoleEntity> items = _Db.Roles;

            if (addInnerItems)
            {
                items.Include(r => r.Users);
            }

            if (predicate == null)
            {
                result = await items.FirstOrDefaultAsync();
            }
            else
            {
                var mappedPredicate =
                    _Mapper.Map<Expression<Func<UserRoleEntity, bool>>>(predicate);

                result = await items.FirstOrDefaultAsync(mappedPredicate);
            }

            if (result == null)
            {
                return null;
            }

            return new UserRole(_Mapper.Map<UserRoleDto>(result));
        }

        public async Task<PaginatedCollectionBase<UserRole>> GetPage(
            int pageNumber,
            int itemsPerPage,
            Expression<Func<UserRole, bool>>? predicate = null,
            bool addInnerItems = false)
        {
            EnsuredUtils.EnsureNumberIsMoreOrEqualValue(pageNumber, 1);
            EnsuredUtils.EnsureNumberIsMoreOrEqualValue(itemsPerPage, 1);

            IQueryable<UserRoleEntity> result = _Db.Roles;

            if (addInnerItems)
            {
                result = result.Include(r => r.Users);
            }

            if (predicate != null)
            {
                var mappedPredicate =
                    _Mapper.Map<Expression<Func<UserRoleEntity, bool>>>(predicate);

                result = result
                    .Where(mappedPredicate);
            }

            var skipItems = (pageNumber - 1) * itemsPerPage;

            result = result.Skip(skipItems).Take(itemsPerPage);

            var collection = await result
                .Select(u => new UserRole(_Mapper.Map<UserRoleDto>(u)))
                .ToListAsync();

            return new PaginatedCollection<UserRole>(
                collection,
                collection.Count == itemsPerPage);
        }

        public async Task<IEnumerable<UserRole>> GetAll(
            Expression<Func<UserRole, bool>>? predicate = null,
            bool addInnerItems = false)
        {
            IQueryable<UserRoleEntity> result = _Db.Roles;

            if (addInnerItems)
            {
                result = result.Include(r => r.Users);
            }

            if (predicate != null)
            {
                var mappedPredicate =
                    _Mapper.Map<Expression<Func<UserRoleEntity, bool>>>(predicate);

                result = result.Where(mappedPredicate);
            }

            result = result.Take(ITEMS_LIMIT);

            return await result
                .Select(u => new UserRole(_Mapper.Map<UserRoleDto>(u)))
                .ToListAsync();
        }

        public async Task<UserRole?> GetById(
            string id,
            bool addInnerItems = false)
        {
            UserRoleEntity? result;

            IQueryable<UserRoleEntity> items = _Db.Roles;

            if (addInnerItems)
            {
                items.Include(r => r.Users);
            }

            result = await items.FirstOrDefaultAsync(ur => ur.Id == id);

            if (result == null)
            {
                return null;
            }

            return new UserRole(_Mapper.Map<UserRoleDto>(result));
        }
    }
}
