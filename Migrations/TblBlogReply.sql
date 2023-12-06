USE [almondcovedb]
GO

/****** Object:  Table [dbo].[TblBlogReply]    Script Date: 24-11-2023 00:15:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TblBlogReply](
	[Id] [int] NOT NULL,
	[CommentId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Reply] [nvarchar](400) NOT NULL,
	[DatePosted] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL
) ON [PRIMARY]
GO

