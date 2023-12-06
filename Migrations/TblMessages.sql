USE [almondcovedb]
GO

/****** Object:  Table [dbo].[TblMessages]    Script Date: 24-11-2023 00:15:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TblMessages](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Message] [nvarchar](300) NOT NULL,
	[DateAdded] [datetime] NULL,
	[IsRead] [bit] NOT NULL,
 CONSTRAINT [PK_TblMessages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

