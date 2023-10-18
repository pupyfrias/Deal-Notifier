USE [DealNotifier2.0]
GO


IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Update_StockStatus' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    DROP PROCEDURE [dbo].[Update_StockStatus]
END
GO

/****** Object:  StoredProcedure [dbo].[Update_StockStatus]    Script Date: 10/17/2023 10:13:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Update_StockStatus]
    @IdListString varchar(max),
    @OnlineStoreId int,
    @Result BIT OUTPUT,
	@ErrorMessage NVARCHAR(4000) OUTPUT
AS
BEGIN
    BEGIN TRY
        DECLARE @IdList TABLE (Id int);
        INSERT INTO @IdList (Id)
        SELECT value FROM STRING_SPLIT(@IdListString, ',');

        UPDATE ITEM
        SET StockStatusId = CASE 
                               WHEN IdList.Id IS NOT NULL THEN 1
                               ELSE 2
                            END,
		LastModified = GETDATE(),
		LastModifiedBy = 'default'
        FROM ITEM
        LEFT JOIN @IdList AS IdList
        ON ITEM.Id = IdList.Id
        WHERE ITEM.OnlineStoreId = @OnlineStoreId
        AND (
             (ITEM.StockStatusId = 2 AND IdList.Id IS NOT NULL)
             OR (ITEM.StockStatusId = 1 AND IdList.Id IS NULL)
            );

        SET @Result = 1;
    END TRY
    BEGIN CATCH
        SET @Result = 0;
		SET @ErrorMessage = ERROR_MESSAGE()
    END CATCH
END
GO

