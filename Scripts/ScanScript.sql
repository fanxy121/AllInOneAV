USE [ScanAllAv]
GO
/****** Object:  Table [dbo].[AllAv]    Script Date: 2018/1/30 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AllAv](
	[FileName] [varchar](500) NULL,
	[Location] [varchar](500) NULL,
	[Size] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Finish]    Script Date: 2018/1/30 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Finish](
	[IsFinish] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Match]    Script Date: 2018/1/30 15:11:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Match](
	[MatchID] [int] IDENTITY(1,1) NOT NULL,
	[AvID] [nchar](100) NOT NULL,
	[Name] [nchar](500) NOT NULL,
	[Location] [nchar](1000) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Match] PRIMARY KEY CLUSTERED 
(
	[MatchID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
