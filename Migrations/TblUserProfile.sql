USE [almondcovedb]
GO

/****** Object:  Table [dbo].[TblUserProfile]    Script Date: 24-11-2023 00:16:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TblUserProfile](
	[Id] [int] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[EMail] [nvarchar](100) NOT NULL,
	[Phone] [nvarchar](15) NULL,
	[Gender] [nvarchar](1) NULL,
	[DateJoined] [datetime] NULL,
	[DateUpdated] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
	[Role] [nvarchar](50) NULL,
	[Bio] [nvarchar](200) NULL,
	[AvatarId] [int] NULL,
	[OTP] [nvarchar](6) NULL,
	[OTPTime] [datetime] NULL,
	[IsVerified] [bit] NULL,
	[CryptedPassword] [nvarchar](200) NULL,
	[SessionKey] [nvarchar](200) NULL,
 CONSTRAINT [PK_TblUserProfile] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

