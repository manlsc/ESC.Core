--TRUNCATE TABLE WStock;  --库存
--TRUNCATE TABLE WOtherIn;  --其他入库
--TRUNCATE TABLE WOtherInLine;  --其他入库行
--TRUNCATE TABLE WOtherOut;  --其他出库
--TRUNCATE TABLE WOtherOutLine;  --其他出库行
--TRUNCATE TABLE WStockOut; --出库明细
--TRUNCATE TABLE WStockIn;  --入库明细
--TRUNCATE TABLE WInventory;  --盘点
--TRUNCATE TABLE WInventoryLine;  --盘点明细


--TRUNCATE TABLE WPurchaseNotice; --采购通知
--TRUNCATE TABLE WPurchaseNoticeLine;  --采购通知行
--TRUNCATE TABLE WPurchase;  --采购
--TRUNCATE TABLE WPurchaseLine;  --采购行
--TRUNCATE TABLE WPurchaseReturn;  --采购退库
--TRUNCATE TABLE WPurchaseReturnLine;  --采购退库行



SELECT * FROM WStock;  --库存
SELECT * FROM WOtherIn;  --其他入库
SELECT * FROM WOtherInLine;  --其他入库行
SELECT * FROM WStockIn;  --入库明细

SELECT * FROM WOtherOut; --其他出库
SELECT * FROM WOtherOutLine; --其他出库行

SELECT * FROM WInventory;  --盘点
SELECT * FROM WInventoryLine;  --盘点明细


SELECT * FROM WPurchaseNotice; --采购通知
SELECT * FROM WPurchaseNoticeLine;  --采购通知行
SELECT * FROM WPurchase;  --采购
SELECT * FROM WPurchaseLine;  --采购行
SELECT * FROM WPurchaseReturn;  --采购退库
SELECT * FROM WPurchaseReturnLine;  --采购退库行