using System;
using System.ComponentModel;
using Dapper.Extensions;

namespace ESC.Infrastructure.DomainObjects
{
    public class WTransferOutLine : PocoObject
    {

        public WTransferOutLine()
        {
           CreateDate = new DateTime(1900, 1, 1);
           UpdateDate = new DateTime(1900, 1, 1);
           StockInDate = new DateTime(1900, 1, 1);
        }

        [Column("ParentID", "ParentID", false)]
        public int ParentID { set; get; }
    
        /// <summary>
        /// ��λ
        /// </summary>
        [Column("PositionID", "��λ", true, true, false, 160, "PositionName")]
        public int PositionID { get; set; }

        /// <summary>
        /// ��λ����
        /// </summary>
        [Column("PositionCode", "��λ����", true, true, true, 160, "")]
        public string PositionCode { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [Column("MaterialID", "����", true, true, true, 160, "MaterialName")]
        public int MaterialID { get; set; }

        /// <summary>
        /// ���ϱ���
        /// </summary>
        [Column("MaterialCode", "���ϱ���", true, true, true, 160, "")]
        public string MaterialCode { get; set; }

        /// <summary>
        /// ��λ
        /// </summary>
        [Column("UnitID", "��λ", true, false, true, 160, "UnitName")]
        public int UnitID { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [Column("Batch", "����", true, false, true, 160, "")]
        public string Batch { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [Column("OwnerCode", "������", false, false, false, 160, "")]
        public string OwnerCode { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [Column("Factory", "����", false, false, false, 160, "")]
        public string Factory { get; set; }

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
        [Column("UpdateDate", "����ʱ��", false, true, true, 160, "")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [Column("UpdateBy", "������", false, true, true, 160, "UpdateByUserName")]
        public int UpdateBy { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Column("OutCount", "��������", true, true, false, 160, "")]
        public decimal OutCount { get; set; }

        /// <summary>
        /// ���ID
        /// </summary>
        [Column("StockID", "���ID", false, false, false, 160, "")]
        public int StockID { get; set; }

        /// <summary>
        /// ���ʱ��
        /// </summary>
        [Column("StockInDate", "���ʱ��", false, true, false, 160, "")]
        public DateTime StockInDate { get; set; }
        
        /// <summary>
        /// Remark
        /// </summary>
        [Column("Remark", "��ע", true, false, false, 160, "")]
        public string Remark { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [ResultColumn]
        public string MaterialName { get; set; }

        /// <summary>
        /// ��λ
        /// </summary>
        [ResultColumn]
        public string PositionName { get; set; }

        /// <summary>
        /// ��λ
        /// </summary>
        [ResultColumn]
        public string UnitName { get; set; }

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
    }
}
