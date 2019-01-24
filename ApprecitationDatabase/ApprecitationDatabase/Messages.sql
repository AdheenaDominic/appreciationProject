CREATE TABLE [dbo].[Messages]
(
	message_id int NOT NULL IDENTITY(1, 1),
	content nvarchar(2000) NOT NULL,
	to_name nvarchar(100) NOT NULL,
	from_name nvarchar(100),
	message_date datetime NOT NULL,
	value_id int NOT NULL, 
	CONSTRAINT PK_Messages_message_id PRIMARY KEY CLUSTERED (message_id),
	CONSTRAINT FK_Messages_value_id FOREIGN KEY (value_id)
	REFERENCES [dbo].[Xero_Values] (value_id)
)
