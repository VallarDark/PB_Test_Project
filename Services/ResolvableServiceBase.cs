using Contracts;

namespace Services
{
    public abstract class ResolvableServiceBase
    {
        protected readonly IRepositoryResolver _RepositoryResolver;

        public RepositoryType RepositoryType { get; set; }

        public ResolvableServiceBase(IRepositoryResolver repositoryResolver)
        {
            _RepositoryResolver = repositoryResolver;

            RepositoryType = RepositoryType.EntityFramework;
        }
    }
}
