USE [almondcovedb]
GO

/****** Object:  Table [dbo].[TblBlogLike]    Script Date: 24-11-2023 00:15:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TblBlogLike](
	[Id] [int] NOT NULL,
	[BlogId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL
) ON [PRIMARY]
GO

