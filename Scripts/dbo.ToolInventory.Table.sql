/****** Object:  Table [dbo].[ToolInventory]    Script Date: 11/09/2010 09:53:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ToolInventory](
	[ToolID] [int] NOT NULL,
	[Quantity] [numeric](10, 2) NOT NULL,
	[OutQuantity] [numeric](10, 2) NOT NULL,
	[ScrapQuantity] [numeric](10, 2) NOT NULL,
	[RepairingQuantity] [numeric](10, 2) NOT NULL,
	[UnitPrice] [numeric](10, 2) NOT NULL,
	[SupplyID] [int] NULL,
	[LastInboundDate] [date] NULL,
 CONSTRAINT [PK_ToolInventory] PRIMARY KEY CLUSTERED 
(
	[ToolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ToolInventory]  WITH CHECK ADD  CONSTRAINT [FK_ToolInventory_Supply] FOREIGN KEY([SupplyID])
REFERENCES [dbo].[Supply] ([SupplyID])
GO
ALTER TABLE [dbo].[ToolInventory] CHECK CONSTRAINT [FK_ToolInventory_Supply]
GO
ALTER TABLE [dbo].[ToolInventory]  WITH CHECK ADD  CONSTRAINT [FK_ToolInventory_Tool] FOREIGN KEY([ToolID])
REFERENCES [dbo].[Tool] ([ToolID])
GO
ALTER TABLE [dbo].[ToolInventory] CHECK CONSTRAINT [FK_ToolInventory_Tool]
GO
ALTER TABLE [dbo].[ToolInventory] ADD  CONSTRAINT [DF_ToolInventory_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[ToolInventory] ADD  CONSTRAINT [DF_ToolInventory_OutQuantity]  DEFAULT ((0)) FOR [OutQuantity]
GO
ALTER TABLE [dbo].[ToolInventory] ADD  CONSTRAINT [DF_ToolInventory_ScrapQuantity]  DEFAULT ((0)) FOR [ScrapQuantity]
GO
ALTER TABLE [dbo].[ToolInventory] ADD  CONSTRAINT [DF_ToolInventory_RepairingQuantity]  DEFAULT ((0)) FOR [RepairingQuantity]
GO
ALTER TABLE [dbo].[ToolInventory] ADD  CONSTRAINT [DF_ToolInventory_UnitPrice]  DEFAULT ((0)) FOR [UnitPrice]
GO
