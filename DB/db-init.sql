USE [Geolocation]
GO
/****** Object:  Table [dbo].[DestinationSource]    Script Date: 4.10.2020 13:43:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DestinationSource](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WorkOrderId] [int] NOT NULL,
	[Source_latitude] [nvarchar](50) NULL,
	[Source_longitude] [nvarchar](50) NULL,
	[Destination_latitude] [nvarchar](50) NULL,
	[Destination_longitude] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_DestinationSource] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkOrder]    Script Date: 4.10.2020 13:43:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MqStartDate] [datetime] NULL,
	[MqEndDate] [datetime] NULL,
	[DbStartDate] [datetime] NULL,
	[DbEndDate] [datetime] NULL,
	[IsMqSuccess] [bit] NOT NULL,
	[IsDbSuccess] [bit] NOT NULL,
	[FilePath] [nvarchar](250) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_WorkOrder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
