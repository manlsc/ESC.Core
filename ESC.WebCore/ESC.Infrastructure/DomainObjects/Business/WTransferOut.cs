using System;
using System.ComponentModel;
using Dapper.Extensions;
using System.Collections.Generic;

namespace ESC.Infrastructure.DomainObjects
{
    public class WTransferOut : PocoObject
    {

        public WTransferOut()
        {
            CreateDate = new DateTime(1900, 1, 1);
            UpdateDate = new DateTime(1900, 1, 1);
            Lines = new List<WTransferOutLine>();
        }

        /// <summary>
        /// TransferOutCode
        /// </summary>
        [Column("TransferOutCode", "���ⵥ��", true, true, true, 160, "")]
        public string TransferOutCode { get; set; }

        /// <summary>
        /// FWarehouseID
        /// </summary>
        [Column("FWarehouseID", "�����ֿ�", true, true, false, 160, "FWarehouseName")]
        public int FWarehouseID { get; set; }

        /// <summary>
        /// FWarehouseCode
        /// </summary>
        [Column("FWarehouseCode", "�����ֿ����", true, true, true, 160, "")]
        public string FWarehouseCode { get; set; }

        /// <summary>
        /// TWarehouseID
        /// </summary>
        [Column("TWarehouseID", "����ֿ�", true, true, false, 160, "TWarehouseName")]
        public int TWarehouseID { get; set; }

        /// <summary>
        /// TWarehouseCode
        /// </summary>
        [Column("TWarehouseCode", "����ֿ����", true, true, true, 160, "")]
        public string TWarehouseCode { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Column("StockOutType", "��������", false, true, false, 160, "StockOutTypeName")]
        public int StockOutType { get; set; }

        /// <summary>
        /// ҵ����
        /// </summary>
        [Column("BusinessPartnerID", "ҵ����", true, true, false, 160, "BusinessPartnerName")]
        public int BusinessPartnerID { get; set; }

        /// <summary>
        /// ����״̬
        /// </summary>
        [Column("StockStatus", "����״̬", true, true, true, 160, "StockStatusName")]
        public int StockStatus { set; get; }

        /// <summary>
        /// <summary>
        /// ����ʱ��
        /// </summary>
        [Column("CreateDate", "����ʱ��", true, true, true, 160, "")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [Column("CreateBy", "������", true, true, true, 160, "CreateByUserName")]
        public int CreateBy { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        [Column("UpdateDate", "����ʱ��", true, true, true, 160, "")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [Column("UpdateBy", "������", true, true, true, 160, "UpdateByUserName")]
        public int UpdateBy { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [ResultColumn]
        public string CreateByUserName { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [ResultColumn]
        public string UpdateByUserName { get; set; }

        /// <summary>
        /// ҵ����
        /// </summary>
        [ResultColumn]
        public string BusinessPartnerName { set; get; }

        /// <summary>
        /// �������
        /// </summary>
        [ResultColumn]
        public string StockOutTypeName { set; get; }

        /// <summary>
        /// ״̬
        /// </summary>
        [ResultColumn]
        public string StockStatusName { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [ResultColumn]
        public string FWarehouseName { set; get; }

        /// <summary>
        /// ״̬
        /// </summary>
        [ResultColumn]
        public string TWarehouseName { set; get; }

        [Ignore]
        public List<WTransferOutLine> Lines { set; get; }

    }
}
