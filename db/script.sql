CREATE DATABASE ChatSystem;
GO
USE ChatSystem;

CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Email] [nvarchar](128) NOT NULL,
	[FirstName] [nvarchar](128) NOT NULL,
	[LastName] [nvarchar](128) NOT NULL,
	[CreatedOn] [datetime] NOT NULL DEFAULT (getdate())
	)

CREATE TABLE [dbo].[MessageType](
	[Id] [int] NOT NULL PRIMARY KEY,
	[Name] [nvarchar](256) NOT NULL
)
GO

INSERT INTO [MessageType](Id,[Name])VALUES(1,'Send')
INSERT INTO [MessageType](Id,[Name])VALUES(2,'Received')



CREATE TABLE [dbo].[Chats](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[UserId] [int] NOT NULL,
	[FriendId] [int] NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[MessageType] [int] NOT NULL,
	[CreatedOn] [datetime] DEFAULT (getdate()) NOT NULL
)

ALTER TABLE [dbo].[Chats]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Chats]  WITH CHECK ADD FOREIGN KEY([FriendId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Chats]  WITH CHECK ADD FOREIGN KEY([MessageType])
REFERENCES [dbo].[MessageType] ([Id])
GO