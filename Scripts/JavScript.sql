USE [JavLibraryDownload]
GO
/****** Object:  Table [dbo].[Actress]    Script Date: 2018/1/15 14:20:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Actress](
	[ActressID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[URL] [nvarchar](200) NULL,
	[CreateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ActressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[all]    Script Date: 2018/1/15 14:20:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[all](
	[Column 1] [varchar](50) NULL,
	[Column 2] [varchar](50) NULL,
	[Column 3] [varchar](50) NULL,
	[Column 4] [varchar](50) NULL,
	[Column 5] [varchar](50) NULL,
	[Column 6] [varchar](50) NULL,
	[Column 7] [varchar](50) NULL,
	[Column 8] [varchar](50) NULL,
	[Column 9] [varchar](50) NULL,
	[Column 10] [varchar](50) NULL,
	[Column 11] [varchar](50) NULL,
	[Column 12] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AV]    Script Date: 2018/1/15 14:20:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AV](
	[AVID] [int] IDENTITY(1,1) NOT NULL,
	[ID] [nvarchar](100) NULL,
	[Name] [nvarchar](300) NULL,
	[Company] [nvarchar](1000) NULL,
	[Director] [nvarchar](1000) NULL,
	[Publisher] [nvarchar](1000) NULL,
	[Category] [nvarchar](1000) NULL,
	[Actress] [nvarchar](1000) NULL,
	[ReleaseDate] [datetime] NULL,
	[AvLength] [int] NULL,
	[CreateTime] [datetime] NULL,
	[PictureURL] [nvarchar](1000) NULL,
	[URL] [nchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[AVID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Batch]    Script Date: 2018/1/15 14:20:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Batch](
	[BatchID] [int] IDENTITY(1,1) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BatchID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 2018/1/15 14:20:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[URL] [nvarchar](200) NULL,
	[CreateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comment]    Script Date: 2018/1/15 14:20:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comment](
	[CommentID] [int] IDENTITY(1,1) NOT NULL,
	[AvID] [nvarchar](300) NOT NULL,
	[AvTitle] [nvarchar](300) NOT NULL,
	[Comment] [nvarchar](4000) NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Company]    Script Date: 2018/1/15 14:20:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[CompanyID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[URL] [nvarchar](200) NULL,
	[CreateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Director]    Script Date: 2018/1/15 14:20:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Director](
	[DirectorID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[URL] [nvarchar](200) NULL,
	[CreateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DirectorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JavLibraryLog]    Script Date: 2018/1/15 14:20:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JavLibraryLog](
	[JavLibraryLog] [int] IDENTITY(1,1) NOT NULL,
	[Logger] [nvarchar](100) NOT NULL,
	[URL] [nvarchar](1000) NULL,
	[Content] [nvarchar](max) NULL,
	[IsException] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[BatchID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[JavLibraryLog] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Publisher]    Script Date: 2018/1/15 14:20:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Publisher](
	[PublisherID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[URL] [nvarchar](200) NULL,
	[CreateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PublisherID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ScanURL]    Script Date: 2018/1/15 14:20:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScanURL](
	[ScanURLID] [int] IDENTITY(1,1) NOT NULL,
	[Category] [nvarchar](100) NULL,
	[URL] [nvarchar](200) NULL,
	[ID] [nvarchar](500) NULL,
	[Title] [nvarchar](500) NULL,
	[IsDownload] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ScanURLID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SecondTry]    Script Date: 2018/1/15 14:20:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecondTry](
	[SecondTryID] [int] IDENTITY(1,1) NOT NULL,
	[Logger] [nchar](100) NOT NULL,
	[Url] [nvarchar](1000) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_SecondTry] PRIMARY KEY CLUSTERED 
(
	[SecondTryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Actress]    Script Date: 2018/1/15 14:20:51 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Actress] ON [dbo].[Actress]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AV]    Script Date: 2018/1/15 14:20:51 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_AV] ON [dbo].[AV]
(
	[ID] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Category]    Script Date: 2018/1/15 14:20:51 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Category] ON [dbo].[Category]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Comment]    Script Date: 2018/1/15 14:20:51 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Comment] ON [dbo].[Comment]
(
	[CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Company]    Script Date: 2018/1/15 14:20:51 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Company] ON [dbo].[Company]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Director]    Script Date: 2018/1/15 14:20:51 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Director] ON [dbo].[Director]
(
	[DirectorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Publisher]    Script Date: 2018/1/15 14:20:51 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Publisher] ON [dbo].[Publisher]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ScanURL]    Script Date: 2018/1/15 14:20:51 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ScanURL] ON [dbo].[ScanURL]
(
	[URL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
