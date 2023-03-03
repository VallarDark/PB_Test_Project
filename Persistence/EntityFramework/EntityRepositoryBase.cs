using AutoMapper;
using Contracts;
using Persistence.EntityFramework.Context;

namespace Persistence.EntityFramework
{
    public abstract class EntityRepositoryBase : RepositoryBase
    {
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
