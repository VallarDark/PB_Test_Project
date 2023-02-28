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
    public class EntityUserRepository : IUserRepository
    {
        private const string ITEM_NOT_EXISTS_EXCEPTION = "Current User does not exists";

        private readonly PbDbContext _db;
        private readonly IMapper _mapper;

        public EntityUserRepository(PbDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public string ServiceType => RepositoryType.EntityFramework.ToString();

        public async Task<Unit> Create(User item)
        {
            var entity = new UserEntity(item);

            var dbRole = await _db.Roles.FirstAsync(r => r.Id == item.Role.Id);

            entity.Role = dbRole;

            _db.Add(entity);

            await _db.SaveChangesAsync();

            return default;
        }

        public async Task<Unit> Delete(string id)
        {
            var item = await GetById(id);

            if (item == null)
            {
                throw new ItemNotExistsException(ITEM_NOT_EXISTS_EXCEPTION);
            }

            _db.Remove(item);

            await _db.SaveChangesAsync();

            return default;
        }

        public async Task<User?> Get(Expression<Func<User, bool>>? predicate = null)
        {
            UserEntity? result;

            if (predicate == null)
            {
                result = await _db.Users.Include(u => u.Role).FirstOrDefaultAsync();
            }
            else
            {
                var mappedPredicate = _mapper.Map<Expression<Func<UserEntity, bool>>>(predicate);

                result = await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(mappedPredicate);
            }

            if (result == null)
            {
                return null;
            }

            return new User(_mapper.Map<UserDto>(result));
        }

        public async Task<PaginatedCollectionBase<User>> GetAll(int pageNumber, int itemsPerPage, Expression<Func<User, bool>>? predicate = null)
        {
            EnsuredUtils.EnsureNumberIsMoreOrEqualValue(pageNumber, 1);
            EnsuredUtils.EnsureNumberIsMoreOrEqualValue(itemsPerPage, 1);

            IQueryable<UserEntity> result;

            var skipItems = (pageNumber - 1) * itemsPerPage;

            if (predicate == null)
            {
                result = _db.Users.Include(u => u.Role)
                    .Skip(skipItems)
                    .Take(itemsPerPage);
            }
            else
            {
                var mappedPredicate = _mapper.Map<Expression<Func<UserEntity, bool>>>(predicate);

                result = _db.Users.Include(u => u.Role)
                        .Where(mappedPredicate)
                        .Skip(skipItems)
                        .Take(itemsPerPage);
            }

            var collection = await result.Select(u => new User(_mapper.Map<UserDto>(u))).ToListAsync();

            return new PaginatedCollection<User>(collection, collection.Count == itemsPerPage);
        }

        public async Task<IEnumerable<User>> GetAll(Expression<Func<User, bool>> predicate = null)
        {
            IQueryable<UserEntity> result;

            if (predicate == null)
            {
                result = _db.Users.Include(u => u.Role);
            }
            else
            {
                var mappedPredicate = _mapper.Map<Expression<Func<UserEntity, bool>>>(predicate);

                result = _db.Users.Include(u => u.Role).Where(mappedPredicate);
            }

            return await result.Select(u => new User(_mapper.Map<UserDto>(u))).ToListAsync();
        }

        public async Task<User?> GetById(string id)
        {
            var result = await _db.Users.Include(u => u.Role).FirstOrDefaultAsync();

            if (result == null)
            {
                return null;
            }

            return new User(_mapper.Map<UserDto>(result));
        }

        public async Task<Unit> Update(User item)
        {
            var entity = new UserEntity(item);

            _db.Users.Update(entity);

            await _db.SaveChangesAsync();

            return default;
        }
    }
}
