
IF NOT EXISTS (SELECT * FROM [dbo].[Xero_Values])
BEGIN
	INSERT INTO [dbo].[Xero_Values] VALUES ('#Human');
	INSERT INTO [dbo].[Xero_Values] VALUES ('#Challenge');
	INSERT INTO [dbo].[Xero_Values] VALUES ('#Champion');
	INSERT INTO [dbo].[Xero_Values] VALUES ('#Ownership');
	INSERT INTO [dbo].[Xero_Values] VALUES ('#Beautiful');
END

