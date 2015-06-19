IF EXISTS(SELECT * FROM msdb.sys.syslogins WHERE UPPER([name]) = 'WebDbUserAccount') 
  BEGIN
	DROP LOGIN [WebDbUserAccount]
	PRINT '<<< REMOVED INSTANCE LOGIN FOR WebDbUserAccount >>>'
  END
GO

CREATE LOGIN [WebDbUserAccount] WITH PASSWORD=N'RHr0x0r!', DEFAULT_DATABASE=[Bootcamp.Expense], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
PRINT '<<< CREATED INSTANCE LOGIN FOR WebDbUserAccount >>>'
GO

PRINT '<<- Switching to Bootcamp.Expense for remainder of permissions ->>'
GO
USE [Bootcamp.Expense]
GO

IF EXISTS(SELECT * FROM sys.sysusers WHERE UPPER([name]) = 'WebDbUserAccount') 
  BEGIN
	DROP USER [WebDbUserAccount]
	PRINT '<<< REMOVED Bootcamp.Expense Security Login for WebDbUserAccount >>>'
  END
GO

CREATE USER [WebDbUserAccount] FOR LOGIN [WebDbUserAccount]
GO
PRINT '<<< CREATED Bootcamp.Expense Security Login for WebDbUserAccount >>>'
GO

EXEC sp_addrolemember N'db_owner', N'WebDbUserAccount'
GO
PRINT '<<< Added WebDbUserAccount to the db_owner group for Bootcamp.Expense >>>'
GO