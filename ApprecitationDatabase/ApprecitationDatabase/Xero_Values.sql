CREATE TABLE [dbo].[Xero_Values]
(
	value_id int NOT NULL IDENTITY(1,1),
	value_name nvarchar(100) NOT NULL,
	CONSTRAINT PK_Xero_Values_value_id PRIMARY KEY CLUSTERED (value_id)
);
