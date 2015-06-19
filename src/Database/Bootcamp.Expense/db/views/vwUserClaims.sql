IF OBJECT_ID('[dbo].[vwUserClaims]','V') IS NULL
    BEGIN
        EXECUTE ('CREATE VIEW [dbo].[vwUserClaims] AS SELECT 0 AS X');
    END
GO

ALTER VIEW [dbo].[vwUserClaims]
AS
SELECT 0 AS X1