using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Repository;
using Dapper.Extensions;
using System;
using System.Collections.Generic;
using ESC.Core;

namespace ESC.Service
{
    /// <summary>
    /// 业务伙伴 +
    /// </summary>
    public class BBusinessPartnerService
    {
        protected BBusinessPartnerRepository bpRepository;

        public BBusinessPartnerService()
        {
            bpRepository = new BBusinessPartnerRepository();
        }

        /// <summary>
        /// 根据主键查询业务伙伴
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public BBusinessPartner GetBusinessPartnerById(int bpId)
        {
            return bpRepository.GetBusinessPartnerById(bpId);
        }


        /// <summary>
        /// 检查非空验证
        /// </summary>
        /// <param name="BusinessPartner"></param>
        public string CheckBusinessPartner(BBusinessPartner bPartner)
        {
            string msg = string.Empty;
            if (string.IsNullOrWhiteSpace(bPartner.BusinessPartnerCode))
            {
                msg = "业务伙伴编码不能为空.";
                return msg;
            }
            if (string.IsNullOrWhiteSpace(bPartner.BusinessPartnerName))
            {
                msg = "业务伙伴名称不能为空.";
                return msg;
            }
            if (!string.IsNullOrWhiteSpace(bPartner.Mail))
            {
                if (!ESCRegex.IsEmail(bPartner.Mail))
                {
                    msg = "邮箱格式错误.";
                    return msg;
                }
            }
            if (!string.IsNullOrWhiteSpace(bPartner.Fax))
            {
                if (!ESCRegex.IsFax(bPartner.Fax))
                {
                    msg = "传真格式错误.";
                    return msg;
                }
            }
            if (!string.IsNullOrWhiteSpace(bPartner.Mobile))
            {
                if (!ESCRegex.IsMobile(bPartner.Mobile))
                {
                    msg = "电话格式错误.";
                    return msg;
                }
            }

            return msg;
        }

        /// <summary>
        /// 删除业务伙伴
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveBusinessPartnerById(int ID)
        {
            bpRepository.Delete(ID);
        }

        /// <summary>
        /// 业务伙伴是否存在
        /// </summary>
        /// <param name="bPartner"></param>
        /// <param name="isAdd"></param>
        public bool IsNotExits(BBusinessPartner bPartner, bool isAdd)
        {
            return bpRepository.IsNotExits(bPartner, isAdd);
        }


        /// <summary>
        /// 插入新业务伙伴
        /// </summary>
        /// <param name="bPartner"></param>
        /// <returns></returns>
        public int AddBusinessPartner(BBusinessPartner bPartner)
        {
            object result = bpRepository.Insert(bPartner);
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// 删除业务伙伴
        /// </summary>
        /// <param name="BusinessPartner"></param>
        /// <returns></returns>
        public int RemoveBusinessPartners(List<BBusinessPartner> BusinessPartner)
        {
            DatabaseContext db = bpRepository.DbCondext;
            try
            {
                db.BeginTransaction();
                foreach (BBusinessPartner bp in BusinessPartner)
                {
                    RemoveBusinessPartner(bp);
                }
                db.CompleteTransaction();
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw ex;
            }
            return BusinessPartner.Count;

        }

        /// <summary>
        /// 删除业务伙伴
        /// </summary>
        /// <param name="bPartner"></param>
        /// <returns></returns>
        public bool RemoveBusinessPartner(BBusinessPartner bPartner)
        {
            //删除业务伙伴
            return bpRepository.Delete(bPartner);
        }

        /// <summary>
        /// 根据ID删除业务伙伴
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool RemoveBusinessPartner(int ID)
        {
            return bpRepository.Delete(ID);
        }

        /// <summary>
        /// 更新业务伙伴
        /// </summary>
        /// <param name="bPartner"></param>
        /// <returns></returns>
        public bool UpdateBusinessPartner(BBusinessPartner bPartner)
        {
            return bpRepository.Update(bPartner);
        }

        /// <summary>
        /// 根据编码或名称查询
        /// </summary>
        /// <param name="codeOrName"></param>
        /// <returns></returns>
        public List<BBusinessPartner> GetBusinessPartnerByCodeName(string codeOrName)
        {
            return bpRepository.GetBusinessPartnerByCodeName(codeOrName);
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<BBusinessPartner> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = bpRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<BBusinessPartner> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return bpRepository.Pages(pageIndex, pageSize, sql, args);
        }

    }
}
