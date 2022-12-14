USE [Master]
GO
/****** Object:  Database [RegistroFacturasSAP_DESA]    Script Date: 13/04/2016 2:11:42 p. m. ******/
CREATE DATABASE [Desarrollo] ON  PRIMARY 
( NAME = N'RegistroFacturasSAP', FILENAME = N'D:\MSSQL_DATA\INTERFACES\Desarrollo.mdf' , SIZE = 524288KB , MAXSIZE = UNLIMITED, FILEGROWTH = 102400KB )
 LOG ON 
( NAME = N'RegistroFacturasSAP_log', FILENAME = N'L:\MSSQL_LOGS\INTERFACES\[Desarrollo_log.ldf' , SIZE = 102400KB , MAXSIZE = 2048GB , FILEGROWTH = 10240KB )
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET COMPATIBILITY_LEVEL = 100
GO


/****** Object:  Schema [rf]    Script Date: 13/04/2016 2:16:30 p. m. ******/
CREATE SCHEMA [rf]
GO
/****** Object:  Schema [sap]    Script Date: 13/04/2016 2:16:30 p. m. ******/
CREATE SCHEMA [sap]
GO
/****** Object:  Table [rf].[CajaChicaDetalle]    Script Date: 13/04/2016 2:16:30 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [rf].[CajaChicaDetalle](
	[BUKRS] [char](4) NOT NULL,
	[DOCUMENT] [bigint] NOT NULL,
	[BLDAT] [char](8) NOT NULL,
	[TYPE] [char](2) NOT NULL,
	[BUDAT] [char](8) NOT NULL,
	[BUZEI] [int] NOT NULL,
	[DUMMY] [char](1) NULL,
	[BSCHL] [char](2) NULL,
	[HKONT] [char](10) NULL,
	[WRBTR] [decimal](11, 2) NULL,
	[WRIVA] [decimal](11, 2) NULL,
	[MWSKZ] [char](2) NULL,
	[SGTXT] [char](40) NULL,
	[SGTXT2] [char](10) NULL,
	[KOSTL] [varchar](12) NULL,
	[AUFNR] [varchar](12) NULL,
	[ZUONR] [char](12) NULL,
	[GSBER] [char](18) NULL,
	[ZFBDT] [char](8) NULL,
	[UMSKZ] [char](8) NULL,
	[CO_AREA] [char](4) NULL,
	[S_WORKP] [bigint] NULL,
	[ACCT_TYPE] [char](1) NULL,
	[UNIT] [char](3) NULL,
 CONSTRAINT [PK_CajaChica_Detalle] PRIMARY KEY CLUSTERED 
(
	[BUKRS] ASC,
	[DOCUMENT] ASC,
	[BLDAT] ASC,
	[TYPE] ASC,
	[BUDAT] ASC,
	[BUZEI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [rf].[CajaChicaEncabezado]    Script Date: 13/04/2016 2:16:30 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [rf].[CajaChicaEncabezado](
	[BUKRS] [char](4) NOT NULL,
	[DOCUMENT] [bigint] NOT NULL,
	[BLDAT] [char](8) NOT NULL,
	[TYPE] [char](2) NOT NULL,
	[BUDAT] [char](8) NOT NULL,
	[XBLNR] [varchar](16) NULL,
	[BKTXT] [varchar](25) NULL,
	[BLART] [char](2) NULL,
	[CURRENCY] [varchar](5) NULL,
	[KURSF] [char](1) NULL,
	[RECORDMODE] [char](1) NULL,
	[NAME] [varchar](35) NULL,
	[NAME2] [varchar](35) NULL,
	[NAME3] [varchar](35) NULL,
	[NAME4] [varchar](35) NULL,
	[ORT01] [varchar](40) NULL,
	[STCD1] [varchar](16) NULL,
	[STCD2] [varchar](11) NULL,
	[DUMMY] [char](10) NULL,
	[ZSTCDT] [char](2) NULL,
 CONSTRAINT [PK_CajaChica_Encabezado] PRIMARY KEY CLUSTERED 
(
	[BUKRS] ASC,
	[DOCUMENT] ASC,
	[BLDAT] ASC,
	[TYPE] ASC,
	[BUDAT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [rf].[CajaChicaEstadoDetalle]    Script Date: 13/04/2016 2:16:30 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [rf].[CajaChicaEstadoDetalle](
	[BUKRS] [char](4) NOT NULL,
	[DOCUMENT] [bigint] NOT NULL,
	[BLDAT] [char](8) NOT NULL,
	[TYPE] [char](2) NOT NULL,
	[BUDAT] [char](8) NOT NULL,
	[FechaLectura] [datetime] NULL CONSTRAINT [DF_CajaChicaEstadoDetalle_FechaLectura]  DEFAULT (getdate()),
	[BUZEI] [int] NOT NULL,
	[Estado] [char](1) NULL,
 CONSTRAINT [PK__CajaChic__D27C4B3A0A54486F] PRIMARY KEY CLUSTERED 
(
	[BUKRS] ASC,
	[DOCUMENT] ASC,
	[BLDAT] ASC,
	[TYPE] ASC,
	[BUDAT] ASC,
	[BUZEI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [rf].[CajaChicaEstadoEncabezado]    Script Date: 13/04/2016 2:16:30 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [rf].[CajaChicaEstadoEncabezado](
	[BUKRS] [char](4) NOT NULL,
	[DOCUMENT] [bigint] NOT NULL,
	[BLDAT] [char](8) NOT NULL,
	[TYPE] [char](2) NOT NULL,
	[BUDAT] [char](8) NOT NULL,
	[FechaLectura] [datetime] NULL CONSTRAINT [DF_CajaChicaEstadoEncabezado_FechaLectura]  DEFAULT (getdate()),
	[Estado] [char](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[BUKRS] ASC,
	[DOCUMENT] ASC,
	[BLDAT] ASC,
	[TYPE] ASC,
	[BUDAT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [sap].[VistaCajaChicaDetalle]    Script Date: 13/04/2016 2:16:30 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [sap].[VistaCajaChicaDetalle]
AS
SELECT        
x.BUKRS, 
x.DOCUMENT, 
x.BLDAT, 
x.TYPE, 
x.BUDAT, 
x.BUZEI, 
x.DUMMY, 
x.BSCHL, 
x.HKONT, 
x.WRBTR, 
x.WRIVA, 
x.MWSKZ, 
x.SGTXT, 
x.SGTXT2, 
x.KOSTL, 
x.AUFNR, 
x.ZUONR, 
x.GSBER, 
x.ZFBDT, 
x.UMSKZ, 
x.CO_AREA, 
x.S_WORKP, 
x.ACCT_TYPE,
x.UNIT
FROM            rf.CajaChicaDetalle x
LEFT JOIN rf.CajaChicaEstadoDetalle  e on
x.BUKRS = e.BUKRS
and x.DOCUMENT = e.DOCUMENT 
and x.BLDAT = e.BLDAT 
and x.TYPE = e.TYPE 
and x.BUDAT = e.BUDAT
and x.BUZEI = e.BUZEI
where e.Estado is null 




GO
/****** Object:  View [sap].[VistaCajaChicaEncabezado]    Script Date: 13/04/2016 2:16:30 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [sap].[VistaCajaChicaEncabezado]
AS
SELECT        
x.BUKRS, 
x.DOCUMENT, 
x.BLDAT, 
x.TYPE, 
x.BUDAT, 
x.XBLNR, 
x.BKTXT, 
x.BLART, 
x.CURRENCY, 
x.KURSF, 
x.RECORDMODE, 
x.NAME, 
x.NAME2,
x.NAME3,
x.NAME4,
x.ORT01, 
x.STCD1, 
x.STCD2, 
x.DUMMY, 
x.ZSTCDT,
e.estado
FROM            rf.CajaChicaEncabezado x
LEFT JOIN rf.CajaChicaEstadoEncabezado  e on
x.BUKRS = e.BUKRS
and x.DOCUMENT = e.DOCUMENT 
and x.BLDAT = e.BLDAT 
and x.TYPE = e.TYPE 
and x.BUDAT = e.BUDAT
where e.Estado is null

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "CajaChicaDetalle (rf)"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 20
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'sap', @level1type=N'VIEW',@level1name=N'VistaCajaChicaDetalle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'sap', @level1type=N'VIEW',@level1name=N'VistaCajaChicaDetalle'
GO
