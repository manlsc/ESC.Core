using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Infrastructure.Repository
{
    public class SRolePermissionRepository : BaseRepository<SRolePermission>
    {
        #region 构造

        public SRolePermissionRepository()
            : base()
        {
        }

        public SRolePermissionRepository(string connectionString)
            : base(connectionString)
        {
        }

        public SRolePermissionRepository(DatabaseContext dbContext)
            : base(dbContext)
        {

        }

        #endregion
    }
}
