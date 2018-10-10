using ESC.Core;
using ESC.Core.Helper;
using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ESC.Service
{
    public class BMaterialGroupService
    {
        protected BMaterialGroupRepository mgRepository;
        protected BMaterialGroupLocationRelRepository mglrRepository;
        protected BLocationRepository lRepository;

        public BMaterialGroupService()
        {
            mgRepository = new BMaterialGroupRepository();
            mglrRepository = new BMaterialGroupLocationRelRepository();
            lRepository = new BLocationRepository();
        }

        /// <summary>
        /// 分组是否存在
        /// </summary>
        /// <param name="group"></param>
        /// <param name="isAdd"></param>
        public bool IsNotExits(BMaterialGroup group, bool isAdd)
        {
            return mgRepository.IsNotExits(group, isAdd);
        }

        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public int AddMaterialGroup(BMaterialGroup group)
        {

            object groupId = mgRepository.Insert(group);
            return Convert.ToInt32(groupId);
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool UpdateMaterialGroup(BMaterialGroup group)
        {
            return mgRepository.Update(group);
        }

        /// <summary>
        /// 根据ID删除记录
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public bool RemoveMaterialGroup(int groupId)
        {
            return mgRepository.Delete(groupId);
        }

        /// <summary>
        /// 根据ID获取详情
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public BMaterialGroup GetMaterialGroupById(int groupId)
        {
            return mgRepository.GetMaterialGroupById(groupId);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool RemoveMaterialGroup(BMaterialGroup group)
        {
            return mgRepository.Delete(group);
        }

        /// <summary>
        /// 查询所有子记录（包括本身）
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="materialGroups"></param>
        public void GetMaterialGroupWithChildren(BMaterialGroup parent, List<BMaterialGroup> materialGroups)
        {
            if (parent == null)
                return;
            materialGroups.Add(parent);

            List<BMaterialGroup> children = GetChildMaterialGroups(parent.ID);
            foreach (BMaterialGroup child in children)
            {
                GetMaterialGroupWithChildren(child, materialGroups);
            }
        }

        /// <summary>
        /// 获取子对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<BMaterialGroup> GetChildMaterialGroups(int parentId)
        {
            return mgRepository.GetChildMaterialGroups(parentId);
        }

        /// <summary>
        /// 获取树
        /// </summary>
        /// <returns></returns>
        public List<BMaterialGroup> GetMaterialGroupTree()
        {
            List<BMaterialGroup> MaterialGroups = mgRepository.QueryAll();
            List<BMaterialGroup> parents = MaterialGroups.Where(o => o.ParentID == 0).ToList();
            foreach (BMaterialGroup parent in parents)
            {
                GetMaterialGroupChild(parent, MaterialGroups);
            }
            return parents;
        }

        /// <summary>
        /// 递归
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="orgs"></param>
        /// <returns></returns>
        private void GetMaterialGroupChild(BMaterialGroup parent, List<BMaterialGroup> MaterialGroups)
        {
            List<BMaterialGroup> children = MaterialGroups.Where(o => o.ParentID == parent.ID).ToList();
            if (children.Count > 0)
            {
                parent.Children.AddRange(children);
                foreach (BMaterialGroup child in children)
                {
                    GetMaterialGroupChild(child, MaterialGroups);
                }
            }
        }

        /// <summary>
        /// 判断父节点否等于节点本身
        /// </summary>
        /// <param name="MaterialGroup"></param>
        /// <returns></returns>
        public bool IsParentEqSelf(BMaterialGroup MaterialGroup)
        {
            if (MaterialGroup.ParentID == 0)
                return false;
            return MaterialGroup.ParentID == MaterialGroup.ID;
        }

        /// <summary>
        /// 根据物料组查询存储单元
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<BLocation> GetLoctionsByGroup(int groupId)
        {
            return lRepository.GetLoctionsByGroup(groupId);
        }

        /// <summary>
        /// 非空验证
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public string CheckMaterialGroup(BMaterialGroup group)
        {
            if (string.IsNullOrWhiteSpace(group.GroupCode))
            {
                return "物料组编码不能为空.";
            }
            if (string.IsNullOrWhiteSpace(group.GroupName))
            {
                return "物料组名称码不能为空.";
            }
            return string.Empty;
        }

        /// <summary>
        /// 添加物料组和仓库关系
        /// </summary>
        /// <param name="locationIds"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public int AddMaterialGroupLocation(List<int> locationIds, int groupId)
        {
            List<BMaterialGroupLocationRel> groups = mglrRepository.GetBMaterialGroupLocationByMaterialGroupId(groupId);
            foreach (int locationId in locationIds)
            {
                if (!groups.Exists(ur => ur.LocationID == locationId))
                {
                    mglrRepository.Insert(new BMaterialGroupLocationRel() { LocationID = locationId, MaterialGroupID = groupId });
                }
            }
            return groups.Count;
        }

        /// <summary>
        /// 删除物料组和仓库关系
        /// </summary>
        /// <param name="locationIds"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public int RemoveMaterialGroupLocation(List<int> locationIds, int groupId)
        {
            List<BMaterialGroupLocationRel> groups = mglrRepository.GetBMaterialGroupLocationByMaterialGroupId(groupId);
            foreach (int locationId in locationIds)
            {
                BMaterialGroupLocationRel rel = groups.Where(ur => ur.LocationID == locationId).FirstOrDefault();
                if (rel != null)
                {
                    mglrRepository.Delete(rel.ID);
                }
            }
            return groups.Count;
        }

        /// <summary>
        /// 根据编码或成名查询
        /// </summary>
        /// <param name="codeName"></param>
        /// <returns></returns>
        public List<BMaterialGroup> GetGroupCodeName(string codeName)
        {
            return mgRepository.GetGroupCodeName(codeName);
        }
    }
}
