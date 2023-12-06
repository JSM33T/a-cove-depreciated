USE [almondcovedb]
GO

/****** Object:  Table [dbo].[TblBlogCategory]    Script Date: 24-11-2023 00:14:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TblBlogCategory](
	[Id] [int] NOT NULL,
	[Title] [nvarchar](100) NULL,
	[Description] [nvarchar](200) NULL,
	[AddedById] [nvarchar](4) NULL,
	[IsActive] [bit] NOT NULL,
	[Locator] [nvarchar](100) NULL,
 CONSTRAINT [PK_TblBlogCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

