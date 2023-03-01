using AutoMapper;
using Contracts;
using Persistence.EntityFramework.Context;

namespace Persistence.EntityFramework
{
    public abstract class EntityRepositoryBase
    {
        protected const int ITEMS_LIMIT = 100;
        protected const string ITEM_NOT_EXISTS_EXCEPTION = "Current {0} does not exists";

        protected readonly PbDbContext _Db;
        protected readonly IMapper _Mapper;

        public EntityRepositoryBase(PbDbContext db, IMapper mapper)
        {
            _Db = db;
            _Mapper = mapper;
        }

        public string ServiceType => RepositoryType.EntityFramework.ToString();
    }
}
