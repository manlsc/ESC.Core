--TRUNCATE TABLE WStock;  --库存
--TRUNCATE TABLE WOtherIn;  --其他入库
--TRUNCATE TABLE WOtherInLine;  --其他入库行
--TRUNCATE TABLE WOtherOut;  --其他出库
--TRUNCATE TABLE WOtherOutLine;  --其他出库行
--TRUNCATE TABLE WStockOut; --出库明细
--TRUNCATE TABLE WStockIn;  --入库明细


SELECT * FROM WStock;  --库存
SELECT * FROM WOtherIn;  --其他入库
SELECT * FROM WOtherInLine;  --其他入库行
SELECT * FROM WStockIn;  --入库明细

SELECT * FROM WOtherOut; --其他出库
SELECT * FROM WOtherOutLine; --其他出库行