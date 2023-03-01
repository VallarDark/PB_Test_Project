using AutoMapper;
using Contracts;
using Domain.Agregates.UserAgregate;
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

            _Db.Add(entity);

            await _Db.SaveChangesAsync();

            return default;
        }

        public async Task<Unit> Delete(string id)
        {
            var item = await GetById(id);

            if (item == null)
            {
                throw new ItemNotExistsException(string.Format(ITEM_NOT_EXISTS_EXCEPTION, nameof(User)));
            }

            _Db.Remove(item);

            await _Db.SaveChangesAsync();

            return default;
        }

        public async Task<User?> Get(Expression<Func<User, bool>>? predicate = null)
        {
            UserEntity? result;

            if (predicate == null)
            {
                result = await _Db.Users.Include(u => u.Role).FirstOrDefaultAsync();
            }
            else
            {
                var mappedPredicate = _Mapper.Map<Expression<Func<UserEntity, bool>>>(predicate);

                result = await _Db.Users.Include(u => u.Role).FirstOrDefaultAsync(mappedPredicate);
            }

            if (result == null)
            {
                return null;
            }

            return new User(_Mapper.Map<UserDto>(result));
        }

        public async Task<PaginatedCollectionBase<User>> GetAll(int pageNumber, int itemsPerPage, Expression<Func<User, bool>>? predicate = null)
        {
            EnsuredUtils.EnsureNumberIsMoreOrEqualValue(pageNumber, 1);
            EnsuredUtils.EnsureNumberIsMoreOrEqualValue(itemsPerPage, 1);

            IQueryable<UserEntity> result;

            var skipItems = (pageNumber - 1) * itemsPerPage;

            if (predicate == null)
            {
                result = _Db.Users.Include(u => u.Role)
                    .Skip(skipItems)
                    .Take(itemsPerPage);
            }
            else
            {
                var mappedPredicate = _Mapper.Map<Expression<Func<UserEntity, bool>>>(predicate);

                result = _Db.Users.Include(u => u.Role)
                        .Where(mappedPredicate)
                        .Skip(skipItems)
                        .Take(itemsPerPage);
            }

            var collection = await result.Select(u => new User(_Mapper.Map<UserDto>(u))).ToListAsync();

            return new PaginatedCollection<User>(collection, collection.Count == itemsPerPage);
        }

        public async Task<IEnumerable<User>> GetAll(Expression<Func<User, bool>> predicate = null)
        {
            IQueryable<UserEntity> result;

            if (predicate == null)
            {
                result = _Db.Users.Include(u => u.Role).Take(ITEMS_LIMIT);
            }
            else
            {
                var mappedPredicate = _Mapper.Map<Expression<Func<UserEntity, bool>>>(predicate);

                result = _Db.Users.Include(u => u.Role).Where(mappedPredicate).Take(ITEMS_LIMIT);
            }

            return await result.Select(u => new User(_Mapper.Map<UserDto>(u))).ToListAsync();
        }

        public async Task<User?> GetById(string id)
        {
            var result = await _Db.Users.Include(u => u.Role).FirstOrDefaultAsync();

            if (result == null)
            {
                return null;
            }

            return new User(_Mapper.Map<UserDto>(result));
        }

        public async Task<Unit> Update(User item)
        {
            var existingItem = await _Db.Users.Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == item.Id);

            if (existingItem == null)
            {
                throw new ItemNotExistsException(string.Format(ITEM_NOT_EXISTS_EXCEPTION, nameof(User)));
            }

            if (existingItem.Role.RoleType != item.Role.RoleType)
            {
                var dbRole = await _Db.Roles.FirstAsync(r => r.Id == item.Role.Id);

                if (dbRole == null)
                {
                    throw new ItemNotExistsException(string.Format(ITEM_NOT_EXISTS_EXCEPTION, nameof(UserRole)));
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
