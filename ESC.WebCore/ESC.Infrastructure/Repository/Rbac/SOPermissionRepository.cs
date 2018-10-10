using ESC.Infrastructure.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Infrastructure.Repository
{
    public class SOPermissionRepository:BaseRepository<SOPermission>
    {
        #region 构造

        public SOPermissionRepository() : base() { }

        public SOPermissionRepository(string connectionString) : base(connectionString) { }

        public SOPermissionRepository(DatabaseContext dbContext) : base(dbContext) { }

        #endregion
    }
}
