using AutoMapper;
using Contracts;
using Domain.Aggregates.UserAggregate;
using Domain.Exceptions;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.FSharp.Core;
using Persistence.Entities;
using Persistence.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Persistence.EntityFramework
{
    public class EntityUserRepository : EntityRepositoryBase, IUserRepository
    {
        public EntityUserRepository(PbDbContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public async Task<Unit> Create(User item)
        {
            var entity = new UserEntity(item);

            var dbRole = await _Db.Roles.FirstAsync(r => r.Id == item.Role.Id);

            entity.Role = dbRole;

            _Db.Users.Add(entity);

            await _Db.SaveChangesAsync();

            return default;
        }

        public async Task<Unit> Delete(string id)
        {
            var item = await _Db.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (item == null)
            {
                throw new ItemNotExistsException(
                    string.Format(ITEM_NOT_EXISTS_EXCEPTION, nameof(User)));
            }

            _Db.Users.Remove(item);

            await _Db.SaveChangesAsync();

            return default;
        }

        public async Task<User?> Get(
            Expression<Func<User, bool>>? predicate = null,
            bool addInnerItems = false)
        {
            UserEntity? result;

            IQueryable<UserEntity> items = _Db.Users;

            if (addInnerItems)
            {
                items = items.Include(r => r.Role);
            }

            if (predicate == null)
            {
                result = await items.FirstOrDefaultAsync();
            }
            else
            {
                var mappedPredicate =
                    _Mapper.Map<Expression<Func<UserEntity, bool>>>(predicate);

                result = await items.FirstOrDefaultAsync(mappedPredicate);
            }

            if (result == null)
            {
                return null;
            }

            return new User(_Mapper.Map<UserDto>(result));
        }

        public async Task<PaginatedCollectionBase<User>> GetPage(
            int pageNumber,
            int itemsPerPage,
            Expression<Func<User, bool>>? predicate = null,
            bool addInnerItems = false)
        {
            EnsuredUtils.EnsureNumberIsMoreOrEqualValue(pageNumber, 1);
            EnsuredUtils.EnsureNumberIsMoreOrEqualValue(itemsPerPage, 1);

            IQueryable<UserEntity> result = _Db.Users;

            if (addInnerItems)
            {
                result = result.Include(r => r.Role);
            }

            if (predicate != null)
            {
                var mappedPredicate =
                    _Mapper.Map<Expression<Func<UserEntity, bool>>>(predicate);

                result = result
                    .Where(mappedPredicate);
            }

            var skipItems = (pageNumber - 1) * itemsPerPage;

            result = result.Skip(skipItems).Take(itemsPerPage);

            var collection = await result
                .Select(u => new User(_Mapper.Map<UserDto>(u)))
                .ToListAsync();

            return new PaginatedCollection<User>(
                collection,
                collection.Count == itemsPerPage);
        }

        public async Task<IEnumerable<User>> GetAll(
            Expression<Func<User, bool>>? predicate = null,
            bool addInnerItems = false)
        {
            IQueryable<UserEntity> result = _Db.Users;

            if (addInnerItems)
            {
                result = result.Include(r => r.Role);
            }

            if (predicate != null)
            {
                var mappedPredicate =
                    _Mapper.Map<Expression<Func<UserEntity, bool>>>(predicate);

                result = result
                    .Where(mappedPredicate);
            }

            result = result.Take(ITEMS_LIMIT);

            return await result
                .Select(u => new User(_Mapper.Map<UserDto>(u)))
                .ToListAsync();
        }

        public async Task<User?> GetById(
            string id,
            bool addInnerItems = false)
        {
            UserEntity? result;

            IQueryable<UserEntity> items = _Db.Users;

            if (addInnerItems)
            {
                items = items.Include(r => r.Role);
            }

            result = await items.FirstOrDefaultAsync(u => u.Id == id);

            if (result == null)
            {
                return null;
            }

            return new User(_Mapper.Map<UserDto>(result));
        }

        public async Task<Unit> Update(User item)
        {
            var existingItem = await _Db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == item.Id);

            if (existingItem == null)
            {
                throw new ItemNotExistsException(
                    string.Format(ITEM_NOT_EXISTS_EXCEPTION, nameof(User)));
            }

            if (existingItem.Role.RoleType != item.Role.RoleType)
            {
                var dbRole = await _Db.Roles
                    .FirstOrDefaultAsync(r => r.Id == item.Role.Id);

                if (dbRole == null)
                {
                    throw new ItemNotExistsException(
                        string.Format(ITEM_NOT_EXISTS_EXCEPTION, nameof(UserRole)));
                }

                existingItem.Role = dbRole;
            }

            existingItem.Update(item);

            _Db.Users.Update(existingItem);

            await _Db.SaveChangesAsync();

            return default;
        }
    }
}
