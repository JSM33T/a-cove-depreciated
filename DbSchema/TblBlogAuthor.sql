USE [almondcovedb]
GO

/****** Object:  Table [dbo].[TblBlogAuthor]    Script Date: 24-11-2023 00:14:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TblBlogAuthor](
	[Id] [int] NOT NULL,
	[BlogId] [int] NOT NULL,
	[AuthorId] [int] NOT NULL
) ON [PRIMARY]
GO

