using Contracts;
using Domain.Agregates.UserAgregate;
using Domain.Exceptions;
using Domain.Utils;
using Microsoft.FSharp.Core;


namespace Services.UserAgregate
{
    public class UserService : IUserService
    {
        private const int ITEMS_PER_PAGE = 15;
        private const string REPOSITORY_DOES_NOT_EXISTS = "Current repository doesn't exists";

        private readonly IRepositoryResolver _repositoryResolver;

        private RepositoryType repositoryType = RepositoryType.EntityFramework;

        private IUserRepository? userRepository =>
            _repositoryResolver?.GetRepositoryByType<User>(repositoryType) as IUserRepository;

        public UserService(IRepositoryResolver repositoryResolver)
        {
            _repositoryResolver = repositoryResolver;
        }

        public Unit Create(User item)
        {
            CheckIfCurrentRepositoryExists();

            EnsuredUtils.EnsureItemNotExists(userRepository, u => u.PersonalData.Equals(item.PersonalData));

            return userRepository.Create(item);
        }

        public User? Get(string id)
        {
            CheckIfCurrentRepositoryExists();

            return EnsuredUtils.EnsureItemExists(userRepository, u => u.Id == id);
        }

        public PaginatedCollectionBase<User> GetAll(int pageNumber)
        {
            CheckIfCurrentRepositoryExists();

            return userRepository.GetAll(pageNumber, ITEMS_PER_PAGE);
        }

        public Unit ChangeRepositoryType(RepositoryType type)
        {
            return EnsuredUtils.EnsureNewValueIsNotSame(repositoryType, type);
        }

        private Unit CheckIfCurrentRepositoryExists()
        {
            if (userRepository == null)
            {
                throw new NullValueException(REPOSITORY_DOES_NOT_EXISTS);
            }

            return default;
        }
    }
}
