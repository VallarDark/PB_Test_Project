using Contracts;
using Domain.Agregates.UserAgregate;
using Domain.Exceptions;
using Domain.Utils;
using Microsoft.FSharp.Core;
using System.Threading.Tasks;

namespace Services.UserAgregate
{
    public class UserService : IUserService
    {
        private const int ITEMS_PER_PAGE = 15;
        private const string REPOSITORY_DOES_NOT_EXISTS = "{0} repository doesn't exists";
        const string DEFAULT_USER_SHOULD_EXISTS_ERROR = "Current {0} does not exist";
        const string DEFAULT_INVALID_LOGIN_DATA_ERROR = "Invalid login data";
        const string DEFAULT_USER_SHOULD_NOT_EXISTS_ERROR = "{0} with same data already exists";

        private readonly IRepositoryResolver _repositoryResolver;
        private readonly IUserTokenProvider _userTokenProvider;

        private User? currentUser;

        private IUserRepository? userRepository =>
            _repositoryResolver?.GetRepository<IUserRepository, User>(RepositoryType) as IUserRepository;

        private IUserRoleRepository? userRoleRepository =>
            _repositoryResolver?.GetReadeableRepository<IUserRoleRepository, UserRole>(RepositoryType) as IUserRoleRepository;

        public RepositoryType RepositoryType { get; set; } = RepositoryType.EntityFramework;

        public User? CurrentUser => currentUser;

        public UserService(IRepositoryResolver repositoryResolver, IUserTokenProvider userTokenProvider)
        {
            _repositoryResolver = repositoryResolver;
            _userTokenProvider = userTokenProvider;
        }

        public async Task<string> RegisterCasualUser(UserDto item)
        {
            return await RegisterUser(item, UserRoleType.User);
        }

        public async Task<string> RegisterAdminUser(UserDto item)
        {
            return await RegisterUser(item, UserRoleType.Admin);
        }

        public async Task<string> LoginUser(UserDto item)
        {
            EnsuredUtils.EnsureNotNull(
                userRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(userRepository)));

            var existedUser = await userRepository.Get(u => u.PersonalData.Email == item.Email);

            if (existedUser == null || !existedUser.VerifyPassword(item.Password))
            {
                throw new InvalidLoginDataException(DEFAULT_INVALID_LOGIN_DATA_ERROR);
            }

            currentUser = existedUser;

            return _userTokenProvider.GenerateToken(existedUser);
        }

        public async Task<User?> VerifyUserByPersonalData(PersonalData personalData)
        {
            EnsuredUtils.EnsureNotNull(
                userRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(userRepository)));

            currentUser = await userRepository.Get(u =>
            u.PersonalData.Email == personalData.Email
            && u.PersonalData.Name == personalData.Name
            && u.PersonalData.LastName == personalData.LastName);

            return currentUser;
        }

        public async Task<PaginatedCollectionBase<User>> GetAllUsers(int pageNumber)
        {
            EnsuredUtils.EnsureNotNull(currentUser);

            PermissionCheckUtils.DoesUserHavePermission(currentUser, UserRoleType.Admin);

            EnsuredUtils.EnsureNotNull(
                userRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(userRepository)));

            return await userRepository.GetAll(pageNumber, ITEMS_PER_PAGE);
        }

        public Unit ChangeRepositoryType(RepositoryType type)
        {
            return EnsuredUtils.EnsureNewValueIsNotSame(RepositoryType, type);
        }

        private async Task<string> RegisterUser(UserDto item, UserRoleType roleType)
        {
            EnsuredUtils.EnsureNotNull(
                userRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(userRepository)));

            EnsuredUtils.EnsureNotNull(
                userRoleRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(userRoleRepository)));

            var personalData = new PersonalData(
                item.Email,
                item.Name,
                item.LastName);

            var existedUser = await VerifyUserByPersonalData(personalData);

            if (existedUser != null)
            {
                throw new ItemAlreadyExistsException(string.Format(DEFAULT_USER_SHOULD_NOT_EXISTS_ERROR, nameof(User)));
            }

            var role = await userRoleRepository.Get(r => r.RoleType == roleType);

            if (role == null)
            {
                throw new ItemNotExistsException(string.Format(DEFAULT_USER_SHOULD_EXISTS_ERROR, nameof(UserRole)));
            }

            var newUser = new User(personalData, item.NickName, role, item.Password);

            currentUser = newUser;

            await userRepository.Create(newUser);

            return _userTokenProvider.GenerateToken(newUser);
        }
    }
}
