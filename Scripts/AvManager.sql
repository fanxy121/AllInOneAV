USE [AvManager]
GO
/****** Object:  Table [dbo].[MoveLog]    Script Date: 2018/3/15 12:19:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MoveLog](
	[Src] [nchar](600) NOT NULL,
	[Des] [nchar](600) NOT NULL,
	[CreateTime] [datetime] NOT NULL
) ON [PRIMARY]
GO
