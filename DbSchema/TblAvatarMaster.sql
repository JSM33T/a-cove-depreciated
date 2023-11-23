USE [almondcovedb]
GO

/****** Object:  Table [dbo].[TblAvatarMaster]    Script Date: 24-11-2023 00:14:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TblAvatarMaster](
	[Id] [int] NOT NULL,
	[Title] [nvarchar](20) NULL,
	[Image] [nvarchar](20) NULL,
	[Description] [nvarchar](100) NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_TblAvatarMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

