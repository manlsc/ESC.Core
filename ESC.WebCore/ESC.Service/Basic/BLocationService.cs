using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Repository;
using Dapper;
using Dapper.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ESC.Service
{
    public class BLocationService
    {
        protected BLocationRepository lRepository;
        protected BLocationUserRelRepository lurRepository;

        public BLocationService()
        {
            lRepository = new BLocationRepository();
            lurRepository = new BLocationUserRelRepository(lRepository.DbCondext);
        }

        /// <summary>
        /// 获取存储单元树
        /// </summary>
        /// <returns></returns>
        public List<BLocation> GetLocationTree()
        {
            List<BLocation> orgs = lRepository.QueryAll();
            List<BLocation> parents = orgs.Select(t => t).Where(o => o.ParentID == 0).ToList();
            foreach (BLocation parent in parents)
            {
                GetLocationChild(parent, orgs);
            }
            return parents;
        }

        /// <summary>
        /// 递归
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="orgs"></param>
        /// <returns></returns>
        private void GetLocationChild(BLocation parent, List<BLocation> orgs)
        {
            List<BLocation> children = orgs.Select(t => t).Where(o => o.ParentID == parent.ID).ToList();
            if (children.Count > 0)
            {
                parent.Children.AddRange(children);
                foreach (BLocation child in children)
                {
                    GetLocationChild(child, orgs);
                }
            }
        }

        /// <summary>
        /// 添加存储单元
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public int AddLocation(BLocation location)
        {

            //添加存储单元
            object id = lRepository.Insert(location);
            return Convert.ToInt32(id);
        }

        /// <summary>
        /// 根据id删除存储单元
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public bool RmoveLocation(int locationId)
        {
            return lRepository.Delete(locationId);
        }

        /// <summary>
        /// 编辑存储单元
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool UpdateLocation(BLocation location)
        {
            var oldEntity = lRepository.GetLocationById(location.ID);
            int oldOrgId = oldEntity.OrgID;
            DatabaseContext db = lRepository.DbCondext;
            try
            {
                db.BeginTransaction();
                bool result = lRepository.Update(location);
                //如果修改了组织，下属的货区货位的组织也要跟着变
                if (oldOrgId != location.OrgID)
                {
                    db.Connection.Execute(string.Format("Update BLocation set OrgId={0} where ID<>{1} and TopLocationID={1}", location.OrgID, location.ID));
                }
                db.CompleteTransaction();
                return result;
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// 添加库管员
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public int AddLocationUserRels(List<int> userIds, int locationId)
        {
            List<BLocationUserRel> locUseRels = lurRepository.GetLocUserRelsByLocId(locationId);

            foreach (int userId in userIds)
            {
                if (!locUseRels.Exists(ur => ur.UserID == userId))
                {
                    lurRepository.Insert(new BLocationUserRel() { LocationID = locationId, UserID = userId });
                }
            }

            return locUseRels.Count;
        }

        /// <summary>
        /// 删除库管员
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public int RemoveLocationUserRels(List<int> userIds, int locationId)
        {
            List<BLocationUserRel> locUseRels = lurRepository.GetLocUserRelsByLocId(locationId);

            foreach (int userId in userIds)
            {
                BLocationUserRel rel = locUseRels.Where(ur => ur.UserID == userId).FirstOrDefault();
                if (rel != null)
                {
                    lurRepository.Delete(rel.ID);
                }
            }
            return locUseRels.Count;
        }

        /// <summary>
        /// 非空验证
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public string CheckLocation(BLocation location)
        {
            if (string.IsNullOrWhiteSpace(location.LocationCode))
            {
                return "存储单元编码不能为空.";
            }
            //if (location.OrgID < 1)
            //{
            //    return "组织机构不能为空.";
            //}
            if (location.ID > 0)
            {
                if (location.ID == location.ParentID)
                {
                    return "上级存储单元不能选择自身.";
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据ID获取存储单元
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public BLocation GetLocationById(int locationId)
        {
            return lRepository.GetLocationById(locationId);
        }

        /// <summary>
        /// 根据编码获取存储单元
        /// </summary>
        /// <param name="locCode"></param>
        /// <returns></returns>
        public List<BLocation> GetLocationCodeName(string locCode)
        {
            return lRepository.GetLocationCodeName(locCode);
        }

        /// <summary>
        /// 根据编码获取存储单元
        /// </summary>
        /// <param name="codeName">货位编码或名称</param>
        /// <param name="warehouseId">仓库ID</param>
        /// <returns></returns>
        public List<BLocation> GetLocationCodeNameWithWh(string locCode,int warehouseId)
        {
            return lRepository.GetLocationCodeNameWithWh(locCode, warehouseId);
        }
        
        /// <summary>
        /// 获取仓库
        /// </summary>
        /// <param name="locCode"></param>
        /// <returns></returns>
        public List<BLocation> GetLocationCodeNameWh(string locCode)
        {
            return lRepository.GetLocationCodeNameWh(locCode);
        }

        /// <summary>
        /// 获取非仓库存储单元
        /// </summary>
        /// <param name="codeName"></param>
        /// <returns></returns>
        public List<BLocation> GetLocationCodeNameNotWh(string codeName)
        {
            return lRepository.GetLocationCodeNameNotWh(codeName);
        }

        /// <summary>
        /// 获取子类存储单元
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public List<BLocation> GetLocationByParentID(int locationId)
        {
            return lRepository.GetLocationByParentID(locationId);
        }

        /// <summary>
        /// 获取默认货区
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public BLocation GetDefaultLocation(int parentId, int locationClass)
        {
            return lRepository.GetDefaultLocation(parentId, locationClass);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="locList"></param>
        /// <returns></returns>
        public int AddLocations(List<BLocation> locList)
        {
            return lRepository.BatchInsert("",locList);
        }

        /// <summary>
        /// 获取所有仓库
        /// </summary>
        /// <returns></returns>
        public List<BLocation> GetTopLocations()
        {
            return lRepository.GetTopLocations();
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool IsNotExits(BLocation location, bool isAdd)
        {
            return lRepository.IsNotExits(location, isAdd);
        }

        /// <summary>
        /// 根据用户编码获取仓库权限
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public List<BLocation> GetLocationsByUserCode(string userCode)
        {
            List<BLocation> locs = new List<BLocation>();
            DataTable dt = lRepository.GetLocationsUserRel(userCode);
            if (dt == null || dt.Rows.Count < 1)
            {
                dt = lRepository.GetLocationsUser(userCode);
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    locs.Add(new BLocation() { ID = Convert.ToInt32(row["ID"].ToString()), LocationCode = row["LocationCode"].ToString() });
                }
            }
            return locs;
        }

        /// <summary>
        /// 根据物料组获取默认存储单元
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<BLocation> GetLocationsByMaterialGroup(int groupId)
        {
            return lRepository.GetLocationsByMaterialGroup(groupId);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<BLocation> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = lRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<BLocation> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return lRepository.Pages(pageIndex, pageSize, sql, args);
        }

    }
}
