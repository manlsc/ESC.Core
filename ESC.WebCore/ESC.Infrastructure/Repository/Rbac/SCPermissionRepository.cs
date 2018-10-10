using ESC.Infrastructure.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 列权限 +
    /// </summary>
   public class SCPermissionRepository : BaseRepository<SCPermission>
    {
        #region 构造

        public SCPermissionRepository() : base() { }

        public SCPermissionRepository(string connectionString) : base(connectionString) { }

        public SCPermissionRepository(DatabaseContext dbContext) : base(dbContext) { }

        #endregion

    }
}
