
-- almondcoveDB.dbo.TblAvatarMaster definition

-- Drop table

-- DROP TABLE almondcoveDB.dbo.TblAvatarMaster;

CREATE TABLE TblAvatarMaster (
	Id int NOT NULL,
	Title nvarchar(20) COLLATE Latin1_General_CI_AI NULL,
	[Image] nvarchar(20) COLLATE Latin1_General_CI_AI NULL,
	Description nvarchar(100) COLLATE Latin1_General_CI_AI NULL,
	IsActive bit NULL,
	CONSTRAINT PK_TblAvatarMaster PRIMARY KEY (Id)
);


-- almondcoveDB.dbo.TblBlogAuthor definition

-- Drop table

-- DROP TABLE almondcoveDB.dbo.TblBlogAuthor;

CREATE TABLE TblBlogAuthor (
	Id int NOT NULL,
	BlogId int NOT NULL,
	AuthorId int NOT NULL
);


-- almondcoveDB.dbo.TblBlogCategory definition

-- Drop table

-- DROP TABLE almondcoveDB.dbo.TblBlogCategory;

CREATE TABLE TblBlogCategory (
	Id int NOT NULL,
	Title nvarchar(100) COLLATE Latin1_General_CI_AI NULL,
	Description nvarchar(200) COLLATE Latin1_General_CI_AI NULL,
	AddedById nvarchar(4) COLLATE Latin1_General_CI_AI NULL,
	IsActive bit NOT NULL,
	[Locator] nvarchar(100) COLLATE Latin1_General_CI_AI NULL,
	CONSTRAINT PK_TblBlogCategory PRIMARY KEY (Id)
);


-- almondcoveDB.dbo.TblBlogComment definition

-- Drop table

-- DROP TABLE almondcoveDB.dbo.TblBlogComment;

CREATE TABLE TblBlogComment (
	Id int NOT NULL,
	PostId int NOT NULL,
	UserId int NOT NULL,
	Comment nvarchar(400) COLLATE Latin1_General_CI_AI NOT NULL,
	DatePosted datetime NOT NULL,
	IsActive bit NOT NULL
);


-- almondcoveDB.dbo.TblBlogLike definition

-- Drop table

-- DROP TABLE almondcoveDB.dbo.TblBlogLike;

CREATE TABLE TblBlogLike (
	Id int NOT NULL,
	BlogId int NOT NULL,
	UserId int NOT NULL,
	DateAdded datetime NOT NULL
);


-- almondcoveDB.dbo.TblBlogMaster definition

-- Drop table

-- DROP TABLE almondcoveDB.dbo.TblBlogMaster;

CREATE TABLE TblBlogMaster (
	Id int NOT NULL,
	Title nvarchar(100) COLLATE Latin1_General_CI_AI NOT NULL,
	Description nvarchar(500) COLLATE Latin1_General_CI_AI NULL,
	UrlHandle nvarchar(100) COLLATE Latin1_General_CI_AI NOT NULL,
	PostContent nvarchar(400) COLLATE Latin1_General_CI_AI NOT NULL,
	CategoryId int NOT NULL,
	AuthorId int NOT NULL,
	Tags nvarchar(50) COLLATE Latin1_General_CI_AI NOT NULL,
	PostLike int NOT NULL,
	IsActive bit NOT NULL,
	DatePosted datetime NULL,
	DateUpdated datetime NULL
);


-- almondcoveDB.dbo.TblBlogReply definition

-- Drop table

-- DROP TABLE almondcoveDB.dbo.TblBlogReply;

CREATE TABLE TblBlogReply (
	Id int NOT NULL,
	CommentId int NOT NULL,
	UserId int NOT NULL,
	Reply nvarchar(400) COLLATE Latin1_General_CI_AI NOT NULL,
	DatePosted datetime NOT NULL,
	IsActive bit NOT NULL
);
 CREATE  UNIQUE NONCLUSTERED INDEX TblBlogReply_Id_IDX ON dbo.TblBlogReply (  Id ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- almondcoveDB.dbo.TblMailingList definition

-- Drop table

-- DROP TABLE almondcoveDB.dbo.TblMailingList;

CREATE TABLE TblMailingList (
	Id int NOT NULL,
	Email nvarchar(MAX) COLLATE Latin1_General_CI_AI NULL,
	Origin nvarchar(MAX) COLLATE Latin1_General_CI_AI NULL,
	DateAdded datetime NULL,
	CONSTRAINT PK_TblMailingList PRIMARY KEY (Id)
);


-- almondcoveDB.dbo.TblMessages definition

-- Drop table

-- DROP TABLE almondcoveDB.dbo.TblMessages;

CREATE TABLE TblMessages (
	Id int NOT NULL,
	Name nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Message nvarchar(300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	DateAdded datetime NULL,
	IsRead bit NOT NULL,
	CONSTRAINT PK_TblMessages PRIMARY KEY (Id)
);


-- almondcoveDB.dbo.TblPasswordReset definition

-- Drop table

-- DROP TABLE almondcoveDB.dbo.TblPasswordReset;

CREATE TABLE TblPasswordReset (
	Id int NOT NULL,
	UserId int NOT NULL,
	Token nvarchar(50) COLLATE Latin1_General_CI_AI NOT NULL,
	DateAdded datetime NOT NULL,
	IsValid bit NULL
);


-- almondcoveDB.dbo.TblSearchMaster definition

-- Drop table

-- DROP TABLE almondcoveDB.dbo.TblSearchMaster;

CREATE TABLE TblSearchMaster (
	Id int NOT NULL,
	Title nvarchar(50) COLLATE Latin1_General_CI_AI NOT NULL,
	Description nvarchar(50) COLLATE Latin1_General_CI_AI NOT NULL,
	Url nvarchar(100) COLLATE Latin1_General_CI_AI NOT NULL,
	[Image] nvarchar(50) COLLATE Latin1_General_CI_AI NOT NULL,
	IsActive bit NOT NULL,
	PermissionLevel nvarchar(10) COLLATE Latin1_General_CI_AI NOT NULL,
	[Type] varchar(20) COLLATE Latin1_General_CI_AI NULL
);


-- almondcoveDB.dbo.TblUserContribution definition

-- Drop table

-- DROP TABLE almondcoveDB.dbo.TblUserContribution;

CREATE TABLE TblUserContribution (
	Id int NOT NULL,
	Title nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Description nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	DateAdded datetime NULL,
	Url nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Type] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	IsActive bit NOT NULL,
	UserId int NULL
);


-- almondcoveDB.dbo.TblUserProfile definition

-- Drop table

-- DROP TABLE almondcoveDB.dbo.TblUserProfile;

CREATE TABLE TblUserProfile (
	Id int NOT NULL,
	FirstName nvarchar(50) COLLATE Latin1_General_CI_AI NOT NULL,
	LastName nvarchar(50) COLLATE Latin1_General_CI_AI NULL,
	UserName nvarchar(50) COLLATE Latin1_General_CI_AI NOT NULL,
	EMail nvarchar(100) COLLATE Latin1_General_CI_AI NOT NULL,
	Phone nvarchar(15) COLLATE Latin1_General_CI_AI NULL,
	Gender nvarchar(1) COLLATE Latin1_General_CI_AI NULL,
	DateJoined datetime NULL,
	DateUpdated datetime NULL,
	IsActive bit NOT NULL,
	[Role] nvarchar(50) COLLATE Latin1_General_CI_AI NULL,
	Bio nvarchar(200) COLLATE Latin1_General_CI_AI NULL,
	AvatarId int NULL,
	OTP nvarchar(6) COLLATE Latin1_General_CI_AI NULL,
	OTPTime datetime NULL,
	IsVerified bit NULL,
	CryptedPassword nvarchar(200) COLLATE Latin1_General_CI_AI NULL,
	SessionKey nvarchar(200) COLLATE Latin1_General_CI_AI NULL,
	CONSTRAINT PK_TblUserProfile PRIMARY KEY (Id)
);