using System;

namespace ESC.Infrastructure.Enums
{
    public class BoolEnum
    {
        /// <summary>
        /// 否
        /// </summary>
        public const int No = 0;

        /// <summary>
        /// 是
        /// </summary>
        public const int Yes = 1;

        /// <summary>
        /// 数据库枚举名称
        /// </summary>
        public const string EnumName = "Bool";

    }

    public class SexEnum
    {
        /// <summary>
        /// 女
        /// </summary>
        public const int Woman = 0;

        /// <summary>
        /// 男
        /// </summary>
        public const int Man = 1;

        /// <summary>
        /// 数据库枚举名称
        /// </summary>
        public const string EnumName = "Sex";

    }

    public class EnbleEnum
    {
        /// <summary>
        /// 禁用
        /// </summary>
        public const int Disable = 0;

        /// <summary>
        /// 启用
        /// </summary>
        public const int Eabled = 1;

        /// <summary>
        /// 数据库枚举名称
        /// </summary>
        public const string EnumName = "Enble";

    }

    public class LocationTypeEnum
    {
        /// <summary>
        /// 仓库
        /// </summary>
        public const int WareHouse = 1;

        /// <summary>
        /// 货区
        /// </summary>
        public const int Area = 2;

        /// <summary>
        /// 货位
        /// </summary>
        public const int Positon = 3;

        /// <summary>
        /// 数据库枚举名称
        /// </summary>
        public const string EnumName = "LocationType";

    }

    public class UnitTypeEnum
    {
        /// <summary>
        /// 长度
        /// </summary>
        public const int L = 1;

        /// <summary>
        /// 质量
        /// </summary>
        public const int m = 2;

        /// <summary>
        /// 时间
        /// </summary>
        public const int t = 3;

        /// <summary>
        /// 电流
        /// </summary>
        public const int I = 4;

        /// <summary>
        /// 数据库枚举名称
        /// </summary>
        public const string EnumName = "UnitType";

    }

    public class BusinessPartnerTypeEnum
    {
        /// <summary>
        /// 客户
        /// </summary>
        public const int Customer = 1;

        /// <summary>
        /// 供应商
        /// </summary>
        public const int Supplier = 2;

        /// <summary>
        /// 客户-供应商
        /// </summary>
        public const int All = 3;

        /// <summary>
        /// 数据库枚举名称
        /// </summary>
        public const string EnumName = "BusinessPartnerType";

    }

    public class StockInEnum
    {
        /// <summary>
        /// 采购入库
        /// </summary>
        public const int Purchase = 1;

        /// <summary>
        /// 调拨入库
        /// </summary>
        public const int TransferIn = 2;

        /// <summary>
        /// 其他入库
        /// </summary>
        public const int OtherIn = 3;

        /// <summary>
        /// 盘盈入库
        /// </summary>
        public const int InvProfit = 4;

        /// <summary>
        /// 销售退库
        /// </summary>
        public const int SellReturn = 5;

        /// <summary>
        /// 数据库枚举名称
        /// </summary>
        public const string EnumName = "StockIn";

    }

    public class StockOutEnum
    {
        /// <summary>
        /// 销售出库
        /// </summary>
        public const int Sell = 1;

        /// <summary>
        /// 调拨出库
        /// </summary>
        public const int TransferOut = 2;

        /// <summary>
        /// 其他出库
        /// </summary>
        public const int OtherOut = 3;

        /// <summary>
        /// 盘亏出库
        /// </summary>
        public const int InvShortages = 4;

        /// <summary>
        /// 采购退库
        /// </summary>
        public const int PurchaseReturn = 5;

        /// <summary>
        /// 数据库枚举名称
        /// </summary>
        public const string EnumName = "StockOut";

    }

    public class StockStatusEnum
    {
        /// <summary>
        /// 新建
        /// </summary>
        public const int New = 1;

        /// <summary>
        /// 审核
        /// </summary>
        public const int Approve = 2;

        /// <summary>
        /// 数据库枚举名称
        /// </summary>
        public const string EnumName = "StockStatus";

    }

    public class NoticeStatusEnum
    {
        /// <summary>
        /// 新建
        /// </summary>
        public const int New = 1;

        /// <summary>
        /// 执行
        /// </summary>
        public const int Executing = 2;

        /// <summary>
        /// 完成
        /// </summary>
        public const int Complete = 3;

        /// <summary>
        /// 数据库枚举名称
        /// </summary>
        public const string EnumName = "NoticeStatus";

    }

    public class InventoryStatusEnum
    {
        /// <summary>
        /// 新建
        /// </summary>
        public const int New = 1;

        /// <summary>
        /// 已盘亏
        /// </summary>
        public const int Out = 2;

        /// <summary>
        /// 已盘盈
        /// </summary>
        public const int In	 = 3;

        /// <summary>
        /// 完成
        /// </summary>
        public const int Complete = 4;

        /// <summary>
        /// 数据库枚举名称
        /// </summary>
        public const string EnumName = "InventoryStatus";

    }

}
