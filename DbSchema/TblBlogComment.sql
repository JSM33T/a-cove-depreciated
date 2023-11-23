USE [almondcovedb]
GO

/****** Object:  Table [dbo].[TblBlogComment]    Script Date: 24-11-2023 00:14:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TblBlogComment](
	[Id] [int] NOT NULL,
	[PostId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Comment] [nvarchar](400) NOT NULL,
	[DatePosted] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL
) ON [PRIMARY]
GO

