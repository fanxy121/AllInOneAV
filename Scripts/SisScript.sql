USE [SisDownload]
GO
/****** Object:  Table [dbo].[AlreadyScaned]    Script Date: 2018/1/15 14:21:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlreadyScaned](
	[AlreadyScanedID] [int] IDENTITY(1,1) NOT NULL,
	[Channel] [nvarchar](100) NOT NULL,
	[Url] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[ScannedDate] [datetime] NOT NULL,
	[IsDownloaded] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AlreadyScanedID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LastOperationEndDate]    Script Date: 2018/1/15 14:21:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LastOperationEndDate](
	[LastOperationEndDateID] [int] IDENTITY(1,1) NOT NULL,
	[LastOperationEndDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[LastOperationEndDateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SisDownloadLog]    Script Date: 2018/1/15 14:21:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SisDownloadLog](
	[SisDownloadLogid] [int] IDENTITY(1,1) NOT NULL,
	[Channel] [nvarchar](100) NULL,
	[Url] [nvarchar](500) NULL,
	[LogContent] [nvarchar](max) NULL,
	[Operation] [nvarchar](200) NULL,
	[CreateTime] [datetime] NULL,
	[Exception] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SisDownloadLogid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AlreadyScaned]    Script Date: 2018/1/15 14:21:53 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_AlreadyScaned] ON [dbo].[AlreadyScaned]
(
	[Url] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
