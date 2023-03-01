using Contracts;
using Domain.Agregates.UserAgregate;
using Domain.Exceptions;
using Domain.Utils;
using Microsoft.FSharp.Core;
using System.Threading.Tasks;

namespace Services.UserAgregate
{
    public class UserService : ResolvableServiceBase, IUserService
    {
        private const string DEFAULT_INVALID_LOGIN_DATA_ERROR = "Invalid login data";

        private readonly IUserTokenProvider _userTokenProvider;

        private User? currentUser;

        private IUserRepository? userRepository =>
            _RepositoryResolver?.GetRepository<IUserRepository, User>(RepositoryType)
            as IUserRepository;

        private IUserRoleRepository? userRoleRepository =>
            _RepositoryResolver?.GetReadeableRepository<IUserRoleRepository, UserRole>(RepositoryType)
            as IUserRoleRepository;

        public User? CurrentUser => currentUser;

        public bool DoesUserHavePermission(UserRoleType permission)
        {
            EnsuredUtils.EnsureNotNull(currentUser, DEFAULT_UNAUTHORISED_ERROR);

            return currentUser.Role.CompareTo(permission) >= 0;
        }

        public UserService(
            IRepositoryResolver repositoryResolver,
            IUserTokenProvider userTokenProvider) : base(repositoryResolver)
        {
            _userTokenProvider = userTokenProvider;
        }

        public async Task<string> RegisterCasualUser(UserRegistrationDto userData)
        {
            return await RegisterUser(userData, UserRoleType.User);
        }

        public async Task<string> RegisterAdminUser(UserRegistrationDto userData)
        {
            return await RegisterUser(userData, UserRoleType.Admin);
        }

        public async Task<string> LoginUser(UserLoginDto userData)
        {
            EnsuredUtils.EnsureNotNull(
                userRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(userRepository)));

            var existedUser = await userRepository.Get(
                    u => u.PersonalData.Email == userData.Email,
                    addInnerItems: true);

            if (existedUser == null
                || !existedUser.VerifyPassword(userData.Password))
            {
                throw new InvalidLoginDataException(DEFAULT_INVALID_LOGIN_DATA_ERROR);
            }

            existedUser.GenerateNewSessionToken();

            await userRepository.Update(existedUser);

            currentUser = existedUser;

            return _userTokenProvider.GenerateToken(existedUser);
        }

        public async Task<User?> VerifyUser(UserValidationDto userValidationData)
        {
            EnsuredUtils.EnsureNotNull(
                userRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(userRepository)));

            currentUser = await userRepository.Get(u =>
                u.PersonalData.Email == userValidationData.Email
                && u.PersonalData.Name == userValidationData.Name
                && u.PersonalData.LastName == userValidationData.LastName
                && u.SessionToken == userValidationData.SessionToken
                && u.Role.RoleType == userValidationData.Role,
                addInnerItems: true);

            if (currentUser == null
                || !currentUser.VerifyPasswordByHash(userValidationData.PasswordHash))
            {
                throw new InvalidLoginDataException(DEFAULT_INVALID_LOGIN_DATA_ERROR);
            }

            return currentUser;
        }

        public async Task<PaginatedCollectionBase<User>> GetAllUsers(int pageNumber)
        {
            if (!DoesUserHavePermission(UserRoleType.Admin))
            {
                throw new LowPrevilegiesLevelException(DEFAULT_LOW_PREVILEGIES_LEVEL_ERROR);
            }

            EnsuredUtils.EnsureNotNull(
                userRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(userRepository)));

            return await userRepository.GetPage(
                pageNumber,
                ITEMS_PER_PAGE,
                addInnerItems: true);
        }

        private async Task<string> RegisterUser(
            UserRegistrationDto userData,
            UserRoleType roleType)
        {
            EnsuredUtils.EnsureNotNull(
                userRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(userRepository)));

            EnsuredUtils.EnsureNotNull(
                userRoleRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(userRoleRepository)));

            var existedUser = await userRepository.Get(u =>
                u.PersonalData.Email == userData.Email
                && u.PersonalData.Name == userData.Name
                && u.PersonalData.LastName == userData.LastName);

            if (existedUser != null)
            {
                throw new ItemAlreadyExistsException(
                    string.Format(DEFAULT_ITEM_SHOULD_NOT_EXISTS_ERROR, nameof(User)));
            }

            var role = await userRoleRepository.Get(r => r.RoleType == roleType);

            if (role == null)
            {
                throw new ItemNotExistsException(
                    string.Format(DEFAULT_ITEM_SHOULD_EXISTS_ERROR, nameof(UserRole)));
            }

            var personalData = new PersonalData(
                userData.Email,
                userData.Name,
                userData.LastName);

            var newUser = new User(
                personalData,
                userData.NickName,
                role,
                userData.Password);

            currentUser = newUser;

            await userRepository.Create(newUser);

            return _userTokenProvider.GenerateToken(newUser);
        }

        public async Task<Unit> UpdateUserRole(string userId, UserRoleType roleType)
        {
            EnsuredUtils.EnsureNotNull(
                userRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(userRepository)));

            EnsuredUtils.EnsureNotNull(
                userRoleRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(userRoleRepository)));

            var existedUser = await userRepository.GetById(userId);

            if (existedUser == null)
            {
                throw new ItemNotExistsException(
                    string.Format(DEFAULT_ITEM_SHOULD_EXISTS_ERROR, nameof(User)));
            }

            var role = await userRoleRepository.Get(r => r.RoleType == roleType);

            if (role == null)
            {
                throw new ItemNotExistsException(
                    string.Format(DEFAULT_ITEM_SHOULD_EXISTS_ERROR, nameof(UserRole)));
            }

            existedUser.UpdateRole(role);

            return await userRepository.Update(existedUser);
        }
    }
}
