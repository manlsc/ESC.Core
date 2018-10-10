using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Repository;
using Dapper.Extensions;
using System;
using System.Collections.Generic;

namespace ESC.Service
{
    public class BMaterialService
    {
        protected BMaterialRepository mRepository;

        public BMaterialService()
        {
            mRepository = new BMaterialRepository();
        }

        /// <summary>
        /// 根据主键查询物料
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public BMaterial GetMaterialById(int bpId)
        {
            return mRepository.GetMaterialById(bpId);
        }


        /// <summary>
        /// 检查非空验证
        /// </summary>
        /// <param name="Material"></param>
        public string CheckMaterial(BMaterial material)
        {
            string msg = string.Empty;
            if (string.IsNullOrWhiteSpace(material.MaterialCode))
            {
                msg = "物料编码不能为空.";
                return msg;
            }
            if (string.IsNullOrWhiteSpace(material.MaterialName))
            {
                msg = "物料名称不能为空.";
                return msg;
            }
            return msg;
        }

        /// <summary>
        /// 删除物料
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveMaterialById(int ID)
        {
            mRepository.Delete(ID);
        }


        /// <summary>
        /// 根据编码或名称查询
        /// </summary>
        /// <param name="codeName"></param>
        /// <returns></returns>
        public List<BMaterial> GetMaterialCodeName(string codeName)
        {
            return mRepository.GetMaterialCodeName(codeName);
        }

        /// <summary>
        /// 物料是否存在
        /// </summary>
        /// <param name="material"></param>
        /// <param name="isAdd"></param>
        public bool IsNotExits(BMaterial material, bool isAdd)
        {
            return mRepository.IsNotExits(material, isAdd);
        }


        /// <summary>
        /// 插入新物料
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public int AddMaterial(BMaterial material)
        {
            object result = mRepository.Insert(material);
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// 删除物料
        /// </summary>
        /// <param name="Material"></param>
        /// <returns></returns>
        public int RemoveMaterials(List<BMaterial> Material)
        {
            DatabaseContext db = mRepository.DbCondext;
            try
            {
                db.BeginTransaction();
                foreach (BMaterial bp in Material)
                {
                    RemoveMaterial(bp);
                }
                db.CompleteTransaction();
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw ex;
            }
            return Material.Count;

        }

        /// <summary>
        /// 删除物料
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public bool RemoveMaterial(BMaterial material)
        {
            //删除物料
            return mRepository.Delete(material);
        }

        /// <summary>
        /// 根据ID删除物料
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool RemoveMaterial(int ID)
        {
            return mRepository.Delete(ID);
        }

        /// <summary>
        /// 更新物料
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public bool UpdateMaterial(BMaterial material)
        {
            return mRepository.Update(material);
        }

        /// <summary>
        /// 根据物料编码查询物料
        /// </summary>
        /// <param name="matCode"></param>
        /// <returns></returns>
        public BMaterial GetMaterialByCode(string matCode)
        {
            return mRepository.GetMaterialByCode(matCode);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<BMaterial> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = mRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<BMaterial> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return mRepository.Pages(pageIndex, pageSize, sql, args);
        }

    }
}
