using AutoMapper;
using Contracts;
using Persistence.Dapper.Context;

namespace Persistence.EntityFramework
{
    public abstract class DapperRepositoryBase : RepositoryBase
    {
        protected readonly DapperContext _Db;
        protected readonly IMapper _Mapper;

        public DapperRepositoryBase(DapperContext db, IMapper mapper)
        {
            _Db = db;
            _Mapper = mapper;
        }

        public string ServiceType => RepositoryType.Dapper.ToString();
    }
}
