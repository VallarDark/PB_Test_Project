using Contracts;

namespace Services
{
    public abstract class ResolvableServiceBase
    {
        protected const int ITEMS_PER_PAGE = 15;
        protected const string REPOSITORY_DOES_NOT_EXISTS =
            "{0} repository doesn't exists";

        protected const string DEFAULT_ITEM_SHOULD_EXISTS_ERROR =
            "Current {0} does not exist";

        protected const string DEFAULT_ITEM_SHOULD_NOT_EXISTS_ERROR =
            "{0} with same data already exists";

        protected const string DEFAULT_ITEM_ALREADY_IN_COLLECTION_ERROR =
            "{0} already exists in this {1}";

        protected const string DEFAULT_ITEM_NOT_IN_COLLECTION_ERROR =
            "{0} not exists in this {1}";

        protected const string DEFAULT_LOW_PREVILEGIES_LEVEL_ERROR =
            "Your access level too low";

        protected const string DEFAULT_UNAUTHORISED_ERROR =
            "You should be an authorized";

        protected readonly IRepositoryResolver _RepositoryResolver;

        public virtual RepositoryType RepositoryType => RepositoryType.EntityFramework;

        public ResolvableServiceBase(IRepositoryResolver repositoryResolver)
        {
            _RepositoryResolver = repositoryResolver;
        }
    }
}
