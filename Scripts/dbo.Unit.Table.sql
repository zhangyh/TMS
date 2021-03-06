/****** Object:  Table [dbo].[Unit]    Script Date: 11/09/2010 09:53:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Unit](
	[UnitID] [int] IDENTITY(1000,1) NOT NULL,
	[Code] [varchar](20) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Comment] [nvarchar](400) NULL,
	[Deleted] [bit] NOT NULL,
	[InternalCode] [varchar](50) NULL,
	[ParentUnitID] [int] NULL,
 CONSTRAINT [PK_Unit] PRIMARY KEY CLUSTERED 
(
	[UnitID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Unit]  WITH CHECK ADD  CONSTRAINT [FK_ChildUnit_ParentUnit] FOREIGN KEY([ParentUnitID])
REFERENCES [dbo].[Unit] ([UnitID])
GO
ALTER TABLE [dbo].[Unit] CHECK CONSTRAINT [FK_ChildUnit_ParentUnit]
GO
ALTER TABLE [dbo].[Unit] ADD  CONSTRAINT [DF_Unit_Code]  DEFAULT ('') FOR [Code]
GO
ALTER TABLE [dbo].[Unit] ADD  CONSTRAINT [DF_Unit_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
