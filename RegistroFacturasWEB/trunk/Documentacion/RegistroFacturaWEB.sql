USE [master]
GO
/****** Object:  Database [RegistroFacturasSAP_DESA]    Script Date: 13/04/2016 2:11:42 p. m. ******/
CREATE DATABASE [RegistroFacturasSAP_DESA] ON  PRIMARY 
( NAME = N'RegistroFacturasSAP', FILENAME = N'D:\MSSQL_DATA\INTERFACES\RegistroFacturasSAP_DESA.mdf' , SIZE = 524288KB , MAXSIZE = UNLIMITED, FILEGROWTH = 102400KB )
 LOG ON 
( NAME = N'RegistroFacturasSAP_log', FILENAME = N'L:\MSSQL_LOGS\INTERFACES\RegistroFacturasSAP_DESA_log.ldf' , SIZE = 102400KB , MAXSIZE = 2048GB , FILEGROWTH = 10240KB )
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RegistroFacturasSAP_DESA].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET ARITHABORT OFF 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET  DISABLE_BROKER 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET  MULTI_USER 
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET DB_CHAINING OFF 
GO
USE [RegistroFacturasSAP_DESA]
GO
/****** Object:  Schema [sap]    Script Date: 13/04/2016 2:11:43 p. m. ******/
CREATE SCHEMA [sap]
GO
/****** Object:  Table [dbo].[AprobacionFacturas]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AprobacionFacturas](
	[IdAprobacionFacturas] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
	[IdFactura] [decimal](18, 0) NOT NULL,
	[IdAprobadorCentro] [int] NULL,
	[Estado] [smallint] NOT NULL,
	[UsuarioAlta] [varchar](16) NULL,
	[FechaAlta] [datetime] NULL CONSTRAINT [DF_AprobacionFacturas_FechaAlta]  DEFAULT (getdate()),
	[UsuarioModificacion] [varchar](16) NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_AprobacionFacturas] PRIMARY KEY CLUSTERED 
(
	[IdAprobacionFacturas] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AprobadorCentro]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AprobadorCentro](
	[IdAprobadorCentro] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[KOSTL] [varchar](10) NOT NULL,
	[AUFNR] [varchar](12) NOT NULL,
	[IdSociedadCentro] [int] NULL,
	[IdNivel] [smallint] NULL,
	[Alta] [bit] NULL CONSTRAINT [DF_AprobadorCentro_Alta]  DEFAULT ((1)),
	[UsuarioCreacion] [varchar](16) NULL,
	[FechaCreacion] [datetime] NULL CONSTRAINT [DF_AprobadorCentro_FechaCreacion]  DEFAULT (getdate()),
	[UsuarioModificacion] [varchar](16) NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_AprobadorCentro] PRIMARY KEY CLUSTERED 
(
	[IdAprobadorCentro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CajaChicaEncabezado]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CajaChicaEncabezado](
	[IdCajaChica] [decimal](6, 0) IDENTITY(1,1) NOT NULL,
	[IdSociedadCentro] [int] NOT NULL,
	[Correlativo] [int] NOT NULL,
	[NumeroCajaChica] [varchar](4) NOT NULL,
	[Descripcion] [varchar](25) NOT NULL,
	[Estado] [smallint] NOT NULL CONSTRAINT [DF_CajaChicaEncabezado_Alta]  DEFAULT ((1)),
	[UsuarioAlta] [varchar](16) NULL,
	[FechaCreacion] [datetime] NOT NULL CONSTRAINT [DF_CajaChicaEncabezado_FechaCreacion]  DEFAULT (getdate()),
	[UsuarioModificacion] [varchar](16) NULL,
	[FechaModificacion] [datetime] NULL,
	[TipoOperacion] [varchar](25) NULL,
	[EncargadoCC] [varchar](70) NULL,
	[IdSociedadMoneda] [smallint] NULL,
 CONSTRAINT [PK_CajaChicaEncabezado] PRIMARY KEY CLUSTERED 
(
	[IdCajaChica] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Centro]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Centro](
	[IdCentro] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Alta] [bit] NOT NULL CONSTRAINT [DF_Centros_Alta]  DEFAULT ((1)),
	[UsuarioAlta] [varchar](16) NULL,
	[FechaAlta] [datetime] NOT NULL CONSTRAINT [DF_Centros_FechaAlta]  DEFAULT (getdate()),
	[UsuarioModificacion] [varchar](16) NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Centros] PRIMARY KEY CLUSTERED 
(
	[IdCentro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Departamento]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departamento](
	[IdDepartamento] [smallint] NOT NULL,
	[Nombre] [nchar](10) NULL,
	[Alta] [nchar](10) NULL,
	[IdUsuarioAlta] [nchar](10) NULL,
	[FechaCreacion] [nchar](10) NULL,
	[IdUsuarioModificacion] [nchar](10) NULL,
	[FechaModificacion] [nchar](10) NULL,
 CONSTRAINT [PK_Departamento] PRIMARY KEY CLUSTERED 
(
	[IdDepartamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FacturaDetalle]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FacturaDetalle](
	[IdFacturaDetalle] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
	[Numero] [int] NOT NULL,
	[Cantidad] [float] NOT NULL,
	[IVA] [float] NOT NULL,
	[Valor] [float] NOT NULL,
	[Impuesto] [float] NOT NULL,
	[CuentaContable] [varchar](10) NOT NULL,
	[DefinicionCuentaContable] [varchar](70) NOT NULL,
	[CargoAbono] [smallint] NOT NULL,
	[IdentificadorIVA] [varchar](2) NOT NULL,
	[Descripcion] [varchar](40) NOT NULL,
	[FechaModificacion] [datetime] NULL,
	[IdFactura] [decimal](18, 0) NOT NULL,
	[Alta] [bit] NOT NULL CONSTRAINT [DF_FacturaDetalle_Alta]  DEFAULT ((1)),
 CONSTRAINT [PK_FacturaDetalle] PRIMARY KEY CLUSTERED 
(
	[IdFacturaDetalle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FacturaEncabezado]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FacturaEncabezado](
	[IdFactura] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
	[CentroCosto] [varchar](50) NULL,
	[OrdenesCosto] [varchar](50) NULL,
	[Serie] [varchar](17) NULL,
	[Numero] [decimal](20, 0) NOT NULL,
	[FechaFactura] [datetime] NOT NULL,
	[EsEspecial] [bit] NOT NULL,
	[RetencionIVA] [bit] NOT NULL,
	[RetencionISR] [bit] NOT NULL,
	[IVA] [float] NOT NULL,
	[ValorTotal] [float] NOT NULL,
	[Estado] [smallint] NOT NULL CONSTRAINT [DF_FacturaEncabezado_Vigente]  DEFAULT ((1)),
	[UsuarioCreacion] [varchar](16) NULL,
	[FechaCreacion] [datetime] NOT NULL CONSTRAINT [DF_FacturaEncabezado_FechaCreacion]  DEFAULT (getdate()),
	[UsuarioModifico] [varchar](16) NULL,
	[FechaModificacion] [datetime] NULL,
	[IdProveedor] [int] NOT NULL,
	[IdCajaChica] [decimal](6, 0) NOT NULL,
	[Aprobada] [bit] NULL,
	[TipoFactura] [varchar](2) NOT NULL,
	[NumeroRetencionISR] [decimal](18, 0) NULL,
	[NumeroRetencionIVA] [decimal](18, 0) NULL,
	[ValorRetencionISR] [float] NULL,
	[ValorRetencionIVA] [float] NULL,
	[Nivel] [smallint] NULL CONSTRAINT [DF_FacturaEncabezado_Nivel]  DEFAULT ((2)),
 CONSTRAINT [PK_FacturaEncabezado] PRIMARY KEY CLUSTERED 
(
	[IdFactura] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IdentificadoresIVA]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IdentificadoresIVA](
	[IdentificadorIVA] [varchar](2) NOT NULL,
	[Descripcion] [varchar](50) NOT NULL,
	[Importe] [smallint] NOT NULL,
	[activo] [bit] NOT NULL,
 CONSTRAINT [PK_Valores_IVA_1] PRIMARY KEY CLUSTERED 
(
	[IdentificadorIVA] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LogErrores]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LogErrores](
	[IdLogErrores] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](250) NOT NULL,
	[Usuario] [varchar](16) NOT NULL,
	[Funcion] [varchar](50) NOT NULL,
	[FechaEvento] [datetime] NOT NULL CONSTRAINT [DF_LogErrores_FechaEvento]  DEFAULT (getdate()),
 CONSTRAINT [PK_LogErrores] PRIMARY KEY CLUSTERED 
(
	[IdLogErrores] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Moneda]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Moneda](
	[Moneda] [varchar](5) NOT NULL,
	[Descripcion] [varchar](30) NOT NULL,
	[Estado] [bit] NOT NULL CONSTRAINT [DF_Moneda_Estado]  DEFAULT ((1)),
	[UsuarioAlta] [varchar](16) NOT NULL,
	[FechaCreacion] [datetime] NOT NULL CONSTRAINT [DF_Moneda_FechaCreacion]  DEFAULT (getdate()),
	[UsuarioModificacion] [varchar](16) NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Moneda] PRIMARY KEY CLUSTERED 
(
	[Moneda] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Nivel]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Nivel](
	[IdNivel] [smallint] IDENTITY(1,1) NOT NULL,
	[Nivel] [varchar](10) NOT NULL,
	[Alta] [bit] NOT NULL CONSTRAINT [DF_Nivel_Alta]  DEFAULT ((1)),
 CONSTRAINT [PK_Nivel] PRIMARY KEY CLUSTERED 
(
	[IdNivel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Proveedor]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Proveedor](
	[IdProveedor] [int] IDENTITY(1,1) NOT NULL,
	[IdTipoDocumento] [smallint] NOT NULL,
	[NumeroIdentificacion] [varchar](15) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Direccion] [varchar](60) NOT NULL,
	[EsPequenioContribuyente] [bit] NOT NULL,
	[Alta] [bit] NOT NULL CONSTRAINT [DF_Proveedor_Alta]  DEFAULT ((1)),
	[UsuarioAlta] [varchar](16) NULL,
	[FechaCreacion] [datetime] NOT NULL CONSTRAINT [DF_Proveedor_FechaCreacion]  DEFAULT (getdate()),
	[UsuarioModificacion] [varchar](16) NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Proveedor] PRIMARY KEY CLUSTERED 
(
	[IdProveedor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RegistroContable]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RegistroContable](
	[IdRegistroContable] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
	[Correlativo] [smallint] NOT NULL,
	[CuentaContable] [varchar](10) NOT NULL,
	[DefinicionCuentaContable] [varchar](70) NOT NULL,
	[CargoAbono] [smallint] NOT NULL,
	[Valor] [float] NOT NULL,
	[IndicadorIVA] [varchar](2) NOT NULL,
	[Alta] [bit] NOT NULL CONSTRAINT [DF_RegistroContable_Alta]  DEFAULT ((1)),
	[FechaAlta] [datetime] NOT NULL CONSTRAINT [DF_RegistroContable_FechaAlta]  DEFAULT (getdate()),
	[FechaModificacion] [datetime] NULL,
	[IdFactura] [decimal](18, 0) NOT NULL,
 CONSTRAINT [PK_RegistroContable] PRIMARY KEY CLUSTERED 
(
	[IdRegistroContable] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rol]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rol](
	[IdRol] [smallint] IDENTITY(1,1) NOT NULL,
	[Rol] [varchar](25) NOT NULL,
	[Alta] [bit] NOT NULL CONSTRAINT [DF_Rol_Alta]  DEFAULT ((1)),
	[UsuarioAlta] [varchar](16) NULL,
	[FechaAlta] [datetime] NULL CONSTRAINT [DF_Rol_FechaAlta]  DEFAULT (getdate()),
	[UsuarioModificacion] [varchar](16) NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sociedad]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sociedad](
	[CodigoSociedad] [char](4) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[MesesFactura] [smallint] NOT NULL,
	[Pais] [varchar](35) NOT NULL,
	[Moneda] [varchar](3) NOT NULL,
	[MontoCompraCC] [float] NOT NULL,
	[ToleranciaCompraCC] [smallint] NOT NULL,
	[Alta] [bit] NOT NULL CONSTRAINT [DF_Sociedad_Alta]  DEFAULT ((1)),
	[UsuarioAlta] [varchar](16) NULL,
	[FechaAlta] [datetime] NOT NULL CONSTRAINT [DF_Sociedad_FechaAlta]  DEFAULT (getdate()),
	[UsuarioModificacion] [varchar](16) NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_CentroCosto] PRIMARY KEY CLUSTERED 
(
	[CodigoSociedad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SociedadCentro]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SociedadCentro](
	[IdSociedadCentro] [int] IDENTITY(1,1) NOT NULL,
	[CodigoSociedad] [char](4) NULL,
	[IdCentro] [int] NOT NULL,
	[Alta] [bit] NOT NULL CONSTRAINT [DF_SociedadCentro_Alta]  DEFAULT ((1)),
	[UsuarioAlta] [varchar](16) NULL,
	[FechaAlta] [datetime] NOT NULL CONSTRAINT [DF_SociedadCentro_FechaAlta]  DEFAULT (getdate()),
	[UsuarioModificacion] [varchar](16) NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_SociedadCentro_1] PRIMARY KEY CLUSTERED 
(
	[IdSociedadCentro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SociedadMoneda]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SociedadMoneda](
	[IdSociedadMoneda] [smallint] IDENTITY(1,1) NOT NULL,
	[CodigoSociedad] [char](4) NOT NULL,
	[Moneda] [varchar](5) NOT NULL,
	[Estado] [bit] NOT NULL CONSTRAINT [DF_SociedadMoneda_Estado]  DEFAULT ((1)),
	[UsuarioCreacion] [varchar](16) NOT NULL,
	[FechaCreacion] [datetime] NOT NULL CONSTRAINT [DF_SociedadMoneda_FechaCreacion]  DEFAULT (getdate()),
	[UsuarioModificacion] [varchar](16) NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_SociedadMoneda] PRIMARY KEY CLUSTERED 
(
	[IdSociedadMoneda] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TipoDocumento]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TipoDocumento](
	[IdTipoDocumento] [smallint] IDENTITY(1,1) NOT NULL,
	[TipoDocumento] [varchar](50) NOT NULL,
	[Descripcion] [varchar](50) NOT NULL,
	[Alta] [bit] NOT NULL CONSTRAINT [DF_TipoDocumento_Alta]  DEFAULT ((1)),
	[UsuarioAlta] [varchar](16) NULL,
	[FechaCreacion] [datetime] NOT NULL CONSTRAINT [DF_TipoDocumento_FechaCreacion]  DEFAULT (getdate()),
	[UsuarioModificacion] [varchar](16) NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_TipoDocumento] PRIMARY KEY CLUSTERED 
(
	[IdTipoDocumento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Usuario](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Usuario] [varchar](16) NULL,
	[Nombre] [varchar](60) NULL,
	[Correo] [varchar](30) NULL,
	[Alta] [bit] NOT NULL CONSTRAINT [DF_Usuario_Alta]  DEFAULT ((1)),
	[UsuarioAlta] [varchar](16) NULL CONSTRAINT [DF_Usuario_UsuarioCreacion]  DEFAULT (getdate()),
	[FechaAlta] [datetime] NOT NULL CONSTRAINT [DF_Usuario_FechaAlta]  DEFAULT (getdate()),
	[UsuarioModificacion] [varchar](16) NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UsuarioCentroCosto]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UsuarioCentroCosto](
	[IdUsuarioCentroCosto] [int] IDENTITY(1,1) NOT NULL,
	[Usuario] [varchar](16) NULL,
	[CentroCosto] [varchar](50) NOT NULL,
	[IdSociedadCentro] [int] NOT NULL,
	[Alta] [bit] NOT NULL CONSTRAINT [DF_MapeoUsuarioCentroCosto_Alta]  DEFAULT ((1)),
	[UsuarioCreacion] [varchar](16) NULL,
	[FechaCreacion] [datetime] NOT NULL CONSTRAINT [DF_MapeoUsuarioCentroCosto_FechaCreacion]  DEFAULT (getdate()),
	[UsuarioModificacion] [varchar](16) NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_MapeoUsuarioCentroCosto] PRIMARY KEY CLUSTERED 
(
	[IdUsuarioCentroCosto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UsuarioOrdenCompra]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UsuarioOrdenCompra](
	[IdUsuarioOrdenCompra] [int] IDENTITY(1,1) NOT NULL,
	[Usuario] [varchar](16) NULL,
	[OrdenCompra] [varchar](50) NOT NULL,
	[IdSociedadCentro] [int] NULL,
	[Alta] [bit] NOT NULL CONSTRAINT [DF_UsuarioOrdenCompra_Alta]  DEFAULT ((1)),
	[UsuarioCreacion] [varchar](16) NULL,
	[FechaCreacion] [datetime] NOT NULL CONSTRAINT [DF_UsuarioOrdenCompra_FechaCreacion]  DEFAULT (getdate()),
	[UsuarioModificacion] [varchar](16) NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_UsuarioOrdenCompra] PRIMARY KEY CLUSTERED 
(
	[IdUsuarioOrdenCompra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UsuarioRol]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UsuarioRol](
	[IdUsuario] [int] NOT NULL,
	[IdRol] [smallint] NOT NULL,
	[Alta] [bit] NOT NULL CONSTRAINT [DF_UsuarioRol_Alta]  DEFAULT ((1)),
	[UsuarioAlta] [varchar](16) NULL,
	[FechaAlta] [datetime] NOT NULL CONSTRAINT [DF_UsuarioRol_FechaAlta]  DEFAULT (getdate()),
	[UsuarioModificacion] [varchar](16) NULL,
	[FechaModificacion] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UsuarioSociedadCentro]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UsuarioSociedadCentro](
	[IdUsuarioSociedadCentro] [int] IDENTITY(1,1) NOT NULL,
	[Usuario] [varchar](16) NULL,
	[IdSociedadCentro] [int] NOT NULL,
	[Alta] [bit] NOT NULL CONSTRAINT [DF_UsuarioSociedadCentro_Alta]  DEFAULT ((1)),
	[UsuarioCreacion] [varchar](16) NULL,
	[FechaCreacion] [datetime] NOT NULL CONSTRAINT [DF_UsuarioSociedadCentro_FechaCreacion]  DEFAULT (getdate()),
	[UsuarioModificacion] [varchar](16) NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_UsuarioSociedadCentro] PRIMARY KEY CLUSTERED 
(
	[IdUsuarioSociedadCentro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VALORES_ISR]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VALORES_ISR](
	[ID_ISR] [int] IDENTITY(1,1) NOT NULL,
	[TIPO_ISR] [smallint] NOT NULL,
	[RANGO_INICIAL] [float] NOT NULL,
	[RANGO_FINAL] [float] NOT NULL,
	[IMPORTE_FIJO] [float] NOT NULL,
	[TIPO_IMPOSITIVO] [smallint] NOT NULL,
 CONSTRAINT [PK_VALORES_ISR] PRIMARY KEY CLUSTERED 
(
	[ID_ISR] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Valores_IVA_]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Valores_IVA_](
	[IdentificadorIVA] [varchar](2) NOT NULL,
	[Descripcion] [varchar](50) NOT NULL,
	[Importe] [smallint] NOT NULL,
	[activo] [bit] NOT NULL,
 CONSTRAINT [PK_Valores_IVA] PRIMARY KEY CLUSTERED 
(
	[IdentificadorIVA] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [sap].[CajaChicaTMP]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [sap].[CajaChicaTMP](
	[BUKR] [varchar](4) NOT NULL,
	[LIFNR] [varchar](4) NOT NULL,
	[NAME] [varchar](50) NOT NULL,
	[WITHT] [varchar](2) NOT NULL,
	[ERDAT] [datetime] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [sap].[CentroCostoTMP]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [sap].[CentroCostoTMP](
	[KOKRS] [char](4) NOT NULL,
	[KOSTL] [varchar](10) NOT NULL,
	[BUKRS] [char](4) NOT NULL,
	[GSBER] [char](4) NOT NULL,
	[KOSAR] [char](1) NULL,
	[VERAK] [varchar](20) NULL,
	[KTEXT] [varchar](20) NULL,
	[KHINR] [varchar](12) NULL,
	[BKZKP] [char](1) NULL,
 CONSTRAINT [PK_CentroCostoTMP] PRIMARY KEY CLUSTERED 
(
	[KOSTL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [sap].[CuentaPorCentroCostoTMP]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [sap].[CuentaPorCentroCostoTMP](
	[KOKRS] [char](4) NOT NULL,
	[KOSTL] [varchar](10) NOT NULL,
	[KTOPL] [varchar](4) NULL,
	[HKONT_LOW] [varchar](10) NULL,
	[HKONT_HIGH] [varchar](10) NULL,
	[BUKRS] [varchar](4) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [sap].[CuentaPorOrdenCOTMP]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [sap].[CuentaPorOrdenCOTMP](
	[KOKRS] [char](4) NOT NULL,
	[BUKRS] [char](4) NOT NULL,
	[AUART] [varchar](4) NOT NULL,
	[KTOPL] [varchar](4) NULL,
	[KSTAR_LOW] [varchar](10) NULL,
	[KSTAR_HIGH] [varchar](10) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [sap].[CuentasContables]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [sap].[CuentasContables](
	[BUKRS] [char](4) NOT NULL,
	[KTOKS] [varchar](4) NOT NULL,
	[TXT30] [varchar](30) NULL,
	[SAKNR] [varchar](10) NOT NULL,
	[TXT50] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [sap].[OrdenCOTMP]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [sap].[OrdenCOTMP](
	[AUFNR] [varchar](12) NOT NULL,
	[AUART] [varchar](4) NOT NULL,
	[KTEXT] [varchar](40) NULL,
	[BUKRS] [char](4) NOT NULL,
	[PHAS1] [char](1) NULL,
	[PHAS2] [char](1) NULL,
	[PHAS3] [char](1) NULL,
 CONSTRAINT [PK_OrdenCOTMP] PRIMARY KEY CLUSTERED 
(
	[AUFNR] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[AprobacionFacturas]  WITH CHECK ADD  CONSTRAINT [FK_AprobacionFacturas_AprobadorCentro] FOREIGN KEY([IdAprobadorCentro])
REFERENCES [dbo].[AprobadorCentro] ([IdAprobadorCentro])
GO
ALTER TABLE [dbo].[AprobacionFacturas] CHECK CONSTRAINT [FK_AprobacionFacturas_AprobadorCentro]
GO
ALTER TABLE [dbo].[AprobacionFacturas]  WITH CHECK ADD  CONSTRAINT [FK_AprobacionFacturas_FacturaEncabezado] FOREIGN KEY([IdFactura])
REFERENCES [dbo].[FacturaEncabezado] ([IdFactura])
GO
ALTER TABLE [dbo].[AprobacionFacturas] CHECK CONSTRAINT [FK_AprobacionFacturas_FacturaEncabezado]
GO
ALTER TABLE [dbo].[AprobadorCentro]  WITH CHECK ADD  CONSTRAINT [FK_AprobadorCentro_Nivel] FOREIGN KEY([IdNivel])
REFERENCES [dbo].[Nivel] ([IdNivel])
GO
ALTER TABLE [dbo].[AprobadorCentro] CHECK CONSTRAINT [FK_AprobadorCentro_Nivel]
GO
ALTER TABLE [dbo].[AprobadorCentro]  WITH CHECK ADD  CONSTRAINT [FK_AprobadorCentro_SociedadCentro] FOREIGN KEY([IdSociedadCentro])
REFERENCES [dbo].[SociedadCentro] ([IdSociedadCentro])
GO
ALTER TABLE [dbo].[AprobadorCentro] CHECK CONSTRAINT [FK_AprobadorCentro_SociedadCentro]
GO
ALTER TABLE [dbo].[AprobadorCentro]  WITH CHECK ADD  CONSTRAINT [FK_AprobadorCentro_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[AprobadorCentro] CHECK CONSTRAINT [FK_AprobadorCentro_Usuario]
GO
ALTER TABLE [dbo].[CajaChicaEncabezado]  WITH CHECK ADD  CONSTRAINT [FK_CajaChicaEncabezado_SociedadMoneda] FOREIGN KEY([IdSociedadMoneda])
REFERENCES [dbo].[SociedadMoneda] ([IdSociedadMoneda])
GO
ALTER TABLE [dbo].[CajaChicaEncabezado] CHECK CONSTRAINT [FK_CajaChicaEncabezado_SociedadMoneda]
GO
ALTER TABLE [dbo].[FacturaDetalle]  WITH CHECK ADD  CONSTRAINT [FK_FacturaDetalle_FacturaEncabezado] FOREIGN KEY([IdFactura])
REFERENCES [dbo].[FacturaEncabezado] ([IdFactura])
GO
ALTER TABLE [dbo].[FacturaDetalle] CHECK CONSTRAINT [FK_FacturaDetalle_FacturaEncabezado]
GO
ALTER TABLE [dbo].[FacturaDetalle]  WITH CHECK ADD  CONSTRAINT [FK_FacturaDetalle_IdentificadoresIVA] FOREIGN KEY([IdentificadorIVA])
REFERENCES [dbo].[IdentificadoresIVA] ([IdentificadorIVA])
GO
ALTER TABLE [dbo].[FacturaDetalle] CHECK CONSTRAINT [FK_FacturaDetalle_IdentificadoresIVA]
GO
ALTER TABLE [dbo].[FacturaEncabezado]  WITH CHECK ADD  CONSTRAINT [FK_FacturaEncabezado_CajaChicaEncabezado] FOREIGN KEY([IdCajaChica])
REFERENCES [dbo].[CajaChicaEncabezado] ([IdCajaChica])
GO
ALTER TABLE [dbo].[FacturaEncabezado] CHECK CONSTRAINT [FK_FacturaEncabezado_CajaChicaEncabezado]
GO
ALTER TABLE [dbo].[FacturaEncabezado]  WITH CHECK ADD  CONSTRAINT [FK_FacturaEncabezado_Proveedor] FOREIGN KEY([IdProveedor])
REFERENCES [dbo].[Proveedor] ([IdProveedor])
GO
ALTER TABLE [dbo].[FacturaEncabezado] CHECK CONSTRAINT [FK_FacturaEncabezado_Proveedor]
GO
ALTER TABLE [dbo].[Proveedor]  WITH CHECK ADD  CONSTRAINT [FK_Proveedor_TipoDocumento] FOREIGN KEY([IdTipoDocumento])
REFERENCES [dbo].[TipoDocumento] ([IdTipoDocumento])
GO
ALTER TABLE [dbo].[Proveedor] CHECK CONSTRAINT [FK_Proveedor_TipoDocumento]
GO
ALTER TABLE [dbo].[RegistroContable]  WITH CHECK ADD  CONSTRAINT [FK_RegistroContable_FacturaEncabezado] FOREIGN KEY([IdFactura])
REFERENCES [dbo].[FacturaEncabezado] ([IdFactura])
GO
ALTER TABLE [dbo].[RegistroContable] CHECK CONSTRAINT [FK_RegistroContable_FacturaEncabezado]
GO
ALTER TABLE [dbo].[SociedadCentro]  WITH CHECK ADD  CONSTRAINT [FK_SociedadCentro_Centro] FOREIGN KEY([IdCentro])
REFERENCES [dbo].[Centro] ([IdCentro])
GO
ALTER TABLE [dbo].[SociedadCentro] CHECK CONSTRAINT [FK_SociedadCentro_Centro]
GO
ALTER TABLE [dbo].[SociedadCentro]  WITH CHECK ADD  CONSTRAINT [FK_SociedadCentro_Sociedad] FOREIGN KEY([CodigoSociedad])
REFERENCES [dbo].[Sociedad] ([CodigoSociedad])
GO
ALTER TABLE [dbo].[SociedadCentro] CHECK CONSTRAINT [FK_SociedadCentro_Sociedad]
GO
ALTER TABLE [dbo].[SociedadMoneda]  WITH CHECK ADD  CONSTRAINT [FK_SociedadMoneda_Moneda] FOREIGN KEY([Moneda])
REFERENCES [dbo].[Moneda] ([Moneda])
GO
ALTER TABLE [dbo].[SociedadMoneda] CHECK CONSTRAINT [FK_SociedadMoneda_Moneda]
GO
ALTER TABLE [dbo].[SociedadMoneda]  WITH CHECK ADD  CONSTRAINT [FK_SociedadMoneda_Sociedad] FOREIGN KEY([CodigoSociedad])
REFERENCES [dbo].[Sociedad] ([CodigoSociedad])
GO
ALTER TABLE [dbo].[SociedadMoneda] CHECK CONSTRAINT [FK_SociedadMoneda_Sociedad]
GO
ALTER TABLE [dbo].[UsuarioCentroCosto]  WITH CHECK ADD  CONSTRAINT [FK_UsuarioCentroCosto_SociedadCentro] FOREIGN KEY([IdSociedadCentro])
REFERENCES [dbo].[SociedadCentro] ([IdSociedadCentro])
GO
ALTER TABLE [dbo].[UsuarioCentroCosto] CHECK CONSTRAINT [FK_UsuarioCentroCosto_SociedadCentro]
GO
ALTER TABLE [dbo].[UsuarioOrdenCompra]  WITH CHECK ADD  CONSTRAINT [FK_UsuarioOrdenCompra_SociedadCentro] FOREIGN KEY([IdSociedadCentro])
REFERENCES [dbo].[SociedadCentro] ([IdSociedadCentro])
GO
ALTER TABLE [dbo].[UsuarioOrdenCompra] CHECK CONSTRAINT [FK_UsuarioOrdenCompra_SociedadCentro]
GO
ALTER TABLE [dbo].[UsuarioRol]  WITH CHECK ADD  CONSTRAINT [FK_UsuarioRol_Rol] FOREIGN KEY([IdRol])
REFERENCES [dbo].[Rol] ([IdRol])
GO
ALTER TABLE [dbo].[UsuarioRol] CHECK CONSTRAINT [FK_UsuarioRol_Rol]
GO
ALTER TABLE [dbo].[UsuarioRol]  WITH CHECK ADD  CONSTRAINT [FK_UsuarioRol_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[UsuarioRol] CHECK CONSTRAINT [FK_UsuarioRol_Usuario]
GO
/****** Object:  StoredProcedure [dbo].[BuscarFacturaAprobar]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[BuscarFacturaAprobar]

	 @usuario varchar(16),
	 @idFactura bigint

as
Begin
select distinct(e.IdFactura), e.Numero, e.Serie, 
				e.CentroCosto, e.OrdenesCosto, 
				e.Aprobada, e.FechaFactura, e.IVA, e.IdCajaChica, d.Descripcion,
				e.IdProveedor, e.TipoFactura, 
				e.ValorTotal, a.IdSociedadCentro, 
				a.IdAprobadorCentro, e.Nivel, b.Usuario, h.Nombre, i.VERAK, j.KTEXT, e.UsuarioCreacion, 
				e.FechaCreacion, case when m.Moneda is null then n.Moneda else m.Moneda end moneda
from AprobadorCentro a
inner join Usuario b on a.IdUsuario = b.IdUsuario and b.Alta = 1
inner join SociedadCentro c on c.IdSociedadCentro = a.IdSociedadCentro and c.Alta = 1
inner join CajaChicaEncabezado d on d.IdSociedadCentro = c.IdSociedadCentro and d.Estado = 1
left join FacturaEncabezado e on e.IdCajaChica = d.IdCajaChica and e.estado = 1
left join sap.CentroCostoTMP i on i.KOSTL = e.CentroCosto 
left join FacturaEncabezado g on g.IdCajaChica = d.IdCajaChica and g.Estado = 1
left join sap.OrdenCOTMP j on j.AUFNR = g.OrdenesCosto
inner join Nivel f on f.IdNivel = a.IdNivel and f.IdNivel = e.Nivel
inner join proveedor h on h.IdProveedor = e.idproveedor
inner join SociedadCentro k on k.IdSociedadCentro = d.IdSociedadCentro
left join SociedadMoneda l on l.CodigoSociedad = k.CodigoSociedad and l.IdSociedadMoneda = d.IdSociedadMoneda
left join Moneda m on m.Moneda = l.Moneda
inner join Sociedad n on n.CodigoSociedad = k.CodigoSociedad
where b.Usuario = @usuario
and e.IdFactura = @idFactura
end
GO
/****** Object:  StoredProcedure [dbo].[BuscarFacturasAprobar]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE procedure [dbo].[BuscarFacturasAprobar]

			@usuario varchar(16) = 'stuctuc',
			@CodigoSociedad varchar(4) = '-1',
			@IdSociedadCentro int = -1,
			@nivel smallint = -1,
			@CentroCosto varchar(10) = '-1',
			@OrdenCosto varchar(12) = '-1'

as
begin			
--select distinct(e.IdFactura), e.Numero, e.Serie, e.CentroCosto, e.OrdenesCosto,
--				e.Aprobada, e.FechaFactura, e.IVA, e.IdCajaChica, d.Descripcion,
--				e.IdProveedor, e.TipoFactura, 
--				e.ValorTotal, a.IdSociedadCentro, 
--				a.IdAprobadorCentro, e.Nivel, b.Usuario
--from AprobadorCentro a
--inner join Usuario b on a.IdUsuario = b.IdUsuario and b.Alta = 1
--inner join SociedadCentro c on c.IdSociedadCentro = a.IdSociedadCentro and c.Alta = 1
--inner join CajaChicaEncabezado d on d.IdSociedadCentro = c.IdSociedadCentro and d.Estado = 1
--left join FacturaEncabezado e on e.IdCajaChica = d.IdCajaChica
--left join FacturaEncabezado g on g.IdCajaChica = d.IdCajaChica
--inner join Nivel f on f.IdNivel = a.IdNivel and f.IdNivel = e.Nivel
--where b.Usuario = @Usuario
--and e.Aprobada is null 
--and ((a.KOSTL = e.CentroCosto) and (a.AUFNR = g.OrdenesCosto))
--and c.CodigoSociedad = case when @CodigoSociedad = '-1' then c.CodigoSociedad else @CodigoSociedad end
--and c.IdSociedadCentro = case when @IdSociedadCentro = '-1' then c.IdSociedadCentro else @IdSociedadCentro end
--and f.IdNivel = case when @nivel = '-1' then f.IdNivel else @Nivel end
--and ((e.CentroCosto = case when @CentroCosto = '-1' then e.CentroCosto else @CentroCosto end)
--  and (e.OrdenesCosto = case when @OrdenCosto = '-1' then e.OrdenesCosto else @OrdenCosto end))

select distinct(e.IdFactura), e.Numero, e.Serie, 
				e.CentroCosto, e.OrdenesCosto, 
				e.Aprobada, e.FechaFactura, e.IVA, e.IdCajaChica, d.Descripcion,
				e.IdProveedor, e.TipoFactura, 
				e.ValorTotal, a.IdSociedadCentro, 
				a.IdAprobadorCentro, e.Nivel, b.Usuario, h.Nombre, i.VERAK, j.KTEXT, e.UsuarioCreacion, 
				e.FechaCreacion, m.Descripcion, case when p.Moneda is null then q.Moneda else p.Moneda end moneda
from AprobadorCentro a
inner join Usuario b on a.IdUsuario = b.IdUsuario and b.Alta = 1
inner join SociedadCentro c on c.IdSociedadCentro = a.IdSociedadCentro and c.Alta = 1
inner join CajaChicaEncabezado d on d.IdSociedadCentro = c.IdSociedadCentro and d.Estado = 1
left join FacturaEncabezado e on e.IdCajaChica = d.IdCajaChica and e.Estado = 1
left join sap.CentroCostoTMP i on i.KOSTL = e.CentroCosto 
left join FacturaEncabezado g on g.IdCajaChica = d.IdCajaChica and g.Estado = 1
left join sap.OrdenCOTMP j on j.AUFNR = g.OrdenesCosto
inner join Nivel f on f.IdNivel = a.IdNivel and f.IdNivel = e.Nivel
inner join proveedor h on h.IdProveedor = e.idproveedor
OUTER APPLY(
	select idFActura,
	STUFF((SELECT ','+ upper(Descripcion)
       FROM FacturaDetalle
       WHERE IdFactura = e.IdFactura
	   and Alta = 1
	   FOR XML PATH('')),1,1,'' ) as Descripcion
		from FacturaEncabezado a
		where a.IdFactura = e.IdFactura
)m 
inner join SociedadCentro n on n.IdSociedadCentro = d.IdSociedadCentro
left join SociedadMoneda o on o.CodigoSociedad = n.CodigoSociedad and o.IdSociedadMoneda = d.IdSociedadMoneda
left join Moneda p on p.Moneda = o.Moneda 
inner join Sociedad q on q.CodigoSociedad = o.CodigoSociedad

where b.Usuario = @Usuario
and e.Aprobada is null 
and ((a.KOSTL = e.CentroCosto) and (a.AUFNR = g.OrdenesCosto))
and c.CodigoSociedad = case when @CodigoSociedad = '-1' then c.CodigoSociedad else @CodigoSociedad end
and c.IdSociedadCentro = case when @IdSociedadCentro = '-1' then c.IdSociedadCentro else @IdSociedadCentro end
and f.IdNivel = case when @nivel = '-1' then f.IdNivel else @Nivel end
and ((e.CentroCosto = case when @CentroCosto = '-1' then e.CentroCosto else @CentroCosto end)
  and (e.OrdenesCosto = case when @OrdenCosto = '-1' then e.OrdenesCosto else @OrdenCosto end))
end



GO
/****** Object:  StoredProcedure [dbo].[BuscarFacturasRevisadasAprobador]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE procedure [dbo].[BuscarFacturasRevisadasAprobador]

			@usuario varchar(8) = 'stuctuc',
			@CodigoSociedad varchar(4) = '-1',
			@IdSociedadCentro int = -1,
			@nivel smallint = -1,
			@CentroCosto varchar(10) = '-1',
			@OrdenCosto varchar(12) = '-1',
			@Aprobada bit

as
begin			
 
  
select distinct(e.IdFactura), e.Numero, e.Serie, 
				e.CentroCosto, e.OrdenesCosto, 
				e.Aprobada, e.FechaFactura, e.IVA, e.IdCajaChica, d.Descripcion,
				e.IdProveedor, e.TipoFactura, 
				e.ValorTotal, a.IdSociedadCentro, 
				a.IdAprobadorCentro, e.Nivel, b.Usuario, h.Nombre, i.VERAK, j.KTEXT, e.UsuarioCreacion, 
				e.FechaCreacion, m.Descripcion, case when o.Moneda is null then p.Moneda else o.Moneda end moneda
from AprobadorCentro a
inner join Usuario b on a.IdUsuario = b.IdUsuario and b.Alta = 1
inner join SociedadCentro c on c.IdSociedadCentro = a.IdSociedadCentro and c.Alta = 1
inner join CajaChicaEncabezado d on d.IdSociedadCentro = c.IdSociedadCentro and d.Estado = 1
left join FacturaEncabezado e on e.IdCajaChica = d.IdCajaChica and e.estado = 1
left join sap.CentroCostoTMP i on i.KOSTL = e.CentroCosto 
left join FacturaEncabezado g on g.IdCajaChica = d.IdCajaChica and g.estado = 1
left join sap.OrdenCOTMP j on j.AUFNR = g.OrdenesCosto
inner join Nivel f on f.IdNivel = a.IdNivel and f.IdNivel = e.Nivel
inner join proveedor h on h.IdProveedor = e.idproveedor  
OUTER APPLY(
	select idFActura,
	STUFF((SELECT ','+ upper(Descripcion)
       FROM FacturaDetalle
       WHERE IdFactura = e.IdFactura
	   and Alta = 1
	   FOR XML PATH('')),1,1,'' ) as Descripcion
		from FacturaEncabezado a
		where a.IdFactura = e.IdFactura
)m 
inner join SociedadCentro n on n.IdSociedadCentro = d.IdSociedadCentro
left join SociedadMoneda o on o.CodigoSociedad = n.CodigoSociedad and o.IdSociedadMoneda = d.IdSociedadMoneda
left join Moneda p on p.Moneda = o.Moneda 
inner join Sociedad q on q.CodigoSociedad = o.CodigoSociedad
where b.Usuario = @Usuario
and  (e.Aprobada = case		
							when @Aprobada = 1 then @Aprobada 
							when @Aprobada = 0 then @Aprobada END)
and ((a.KOSTL = e.CentroCosto) and (a.AUFNR = g.OrdenesCosto))
and c.CodigoSociedad = case when @CodigoSociedad = '-1' then c.CodigoSociedad else @CodigoSociedad end
and c.IdSociedadCentro = case when @IdSociedadCentro = '-1' then c.IdSociedadCentro else @IdSociedadCentro end
and f.IdNivel = case when @nivel = '-1' then f.IdNivel else @Nivel end
and ((e.CentroCosto = case when @CentroCosto = '-1' then e.CentroCosto else @CentroCosto end)
  and (e.OrdenesCosto = case when @OrdenCosto = '-1' then e.OrdenesCosto else @OrdenCosto end))

end






GO
/****** Object:  StoredProcedure [dbo].[CentroCosto]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CentroCosto]

	@BUKRS VARCHAR(4) = 0
as
begin
select distinct a.BUKRS, a.KOSTL, a.VERAK
from sap.CentroCostoTMP a
inner join sap.CuentaPorCentroCostoTMP b on b.BUKRS = a.BUKRS
											and b.KOSTL = a.KOSTL
inner join Sociedad c on c.CodigoSociedad = b.BUKRS
where c.CodigoSociedad = case when @BUKRS = 0 then c.CodigoSociedad else @BUKRS end
end
GO
/****** Object:  StoredProcedure [dbo].[Liquidacion]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[Liquidacion]
	
		@IdCajaChica decimal
	
as
begin
select distinct b.TipoOperacion, b.FechaCreacion FechaCreacionCC, e.CodigoSociedad, e.Nombre NombreSociedad, f.IdCentro, f.Nombre NombreCentro, b.NumeroCajaChica, 
CAST(e.CodigoSociedad as varchar(4)) + '-' + CAST(f.IdCentro as varchar(2)) + '-' + CAST(b.NumeroCajaChica as varchar(4)) + '-' + RIGHT(REPLICATE('0', 6)+ CAST(b.Correlativo  AS VARCHAR(6)), 6) CodigoCajaChica,
a.IdFactura, a.CentroCosto, a.OrdenesCosto, a.IdProveedor, g.IdTipoDocumento, h.TipoDocumento Identificacion, g.Nombre NombreProveedor, a.Serie, a.Numero, a.EsEspecial, a.ValorTotal, 
a.IVA IVA_Total, a.FechaFactura, a.FechaCreacion FechaCreacionFac, 
CASE 
	WHEN a.Estado = 0 THEN 'ANULADO'
	WHEN a.Estado = 3 then 'ANULADO'
	ELSE 'VIGENTE' END EstadoFac, 
round((sum(c.Valor) - sum(c.Impuesto)) / (1.12), 2) base, sum(c.Impuesto) Impuesto, sum(c.IVA) IVA,
0 ISR, 
case 
	when b.Estado = 0 then 'ANULADO' 
	when b.Estado = 1 then 'VIGENTE' 
	ELSE 'CERRADA' END EstadoCC, 
isnull(i.base,0) BaseVigente, isnull(i.Impuesto,0) ImpuestoVigente, ISNULL(i.IVA,0) IVAVigente, isnull(i.ISR,0) ISRVigente, isnull(i.ValorTotal,0) ValorTotalVigente,
isnull(j.base,0) BaseAnulado, isnull(j.Impuesto,0) ImpuestoAnulado, isnull(j.IVA,0) IVAAnulado, isnull(j.ISR,0) ISRAnulado, isnull(j.ValorTotal,0) ValorTotalAnulado,
k.Nombre NombreUsuario
from FacturaEncabezado a
inner join CajaChicaEncabezado b on b.IdCajaChica = a.IdCajaChica and b.Estado != 0
inner join FacturaDetalle c on c.IdFactura = a.IdFactura and c.Alta = 1
inner join SociedadCentro d on d.IdSociedadCentro = b.IdSociedadCentro
inner join Sociedad e on e.CodigoSociedad = d.CodigoSociedad
inner join Centro f on f.IdCentro = d.IdCentro
inner join Proveedor g on g.IdProveedor = a.IdProveedor
inner join TipoDocumento h on h.IdTipoDocumento = g.IdTipoDocumento
LEFT join(
	select distinct a.IdCajaChica, round((sum(b.Valor) - sum(b.Impuesto)) / (1.12), 2) base, sum(b.Impuesto) Impuesto, sum(b.IVA) IVA,
	0 ISR, SUM(b.Valor) ValorTotal
	from FacturaEncabezado a
	inner join FacturaDetalle b on b.IdFactura = a.IdFactura and b.Alta = 1
	inner join CajaChicaEncabezado c on c.IdCajaChica = a.IdCajaChica --and c.Estado = 1
	where a.IdCajaChica = @IdCajaChica
	and ((a.Estado = case when c.Estado = 1 and a.UsuarioModifico is null or a.UsuarioCreacion = a.UsuarioModifico then 1 end)
	or (a.Estado = case when c.Estado = 2 then 1 end)
	or (a.estado = case when c.Estado = 2 then 2 end))
	and (a.Aprobada is null or a.Aprobada = 1)
	group by a.IdCajaChica
)i on i.IdCajaChica = a.IdCajaChica
left join(
	select distinct a.IdCajaChica, round((sum(isnull(b.Valor,0)) - sum(isnull(b.Impuesto,0))) / (1.12), 2) base, sum(isnull(b.Impuesto,0)) Impuesto, sum(isnull(b.IVA,0)) IVA,
	0 ISR, SUM(b.Valor) ValorTotal
	from FacturaEncabezado a
	inner join FacturaDetalle b on b.IdFactura = a.IdFactura and b.Alta = 1
	inner join CajaChicaEncabezado c on c.IdCajaChica = a.IdCajaChica --and c.Estado = 1
	where a.IdCajaChica = @IdCajaChica
	and a.Estado = 3
	group by a.IdCajaChica
)j on j.IdCajaChica = a.IdCajaChica
inner join Usuario k on k.Usuario = a.UsuarioCreacion
where b.IdCajaChica = @IdCajaChica
and ((a.Estado = case when b.Estado = 1 and a.UsuarioModifico is null or a.UsuarioCreacion = a.UsuarioModifico then 1 end)
	or (a.Estado = case when b.Estado = 2 then 1 end)
	or (a.estado = case when b.Estado = 2 then 2 end)
	or (a.estado = case when b.Estado = 2 then 3 end))
and (a.Aprobada is null or a.Aprobada = 1)
group by b.TipoOperacion, b.FechaCreacion, e.CodigoSociedad, e.Nombre, f.IdCentro, f.Nombre, b.NumeroCajaChica,
a.IdFactura, a.CentroCosto, a.OrdenesCosto, a.IdProveedor, g.IdTipoDocumento, h.TipoDocumento, g.Nombre, a.Serie,
a.Numero, a.EsEspecial, a.ValorTotal, a.IVA, a.FechaFactura, a.FechaCreacion, b.Correlativo, a.Estado, c.IdFactura,
b.Estado, i.base, i.Impuesto, i.IVA, i.ISR, j.base, j.Impuesto, j.IVA, j.ISR, i.ValorTotal, j.ValorTotal,
k.Nombre
end
GO
/****** Object:  StoredProcedure [dbo].[ListarCentrosCosto]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ListarCentrosCosto]

	@BUKRS varchar(4),
	@CeCos varchar(1000)

as
begin

declare @str varchar(2000)

set @str = ('select DISTINCT a.KOSTL, ' + CONVERT(varchar(100), + ' a.KOSTL + ''-'' + KTEXT') + ' Descripcion
from sap.CuentaPorCentroCostoTMP a
inner join sap.CentroCostoTMP b on b.KOSTL = a.KOSTL
						and b.KOKRS = a.KOKRS
						and b.BUKRS = '+ @BUKRS +' 
						and b.kostl in ('+ @CeCos +')')
--select @str

execute(@str)

end
GO
/****** Object:  StoredProcedure [dbo].[ListarOrdenCosto]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ListarOrdenCosto]

	@BUKRS varchar(4),
	@OrCo varchar(1000)

as
begin

declare @str varchar(2000)

set @str = ('select AUFNR, ' + CONVERT(varchar(100), + ' AUFNR + ''-'' +  KTEXT') + ' Descripcion
            from sap.OrdenCOTMP a
            inner join sap.CuentaPorOrdenCOTMP b on a.AUART = b.AUART
											and b.BUKRS = ' + @BUKRS + '
											and AUFNR in ('+ @OrCo +')')

--select @str

execute(@str)

end
GO
/****** Object:  StoredProcedure [dbo].[NotificacionAprobaciones]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[NotificacionAprobaciones]

as
begin	
select Usuario, Nombre, Correo, sum(Facturas) Facturas
from (
select b.Usuario, b.Nombre, b.Correo, COUNT(f.IdCajaChica) Facturas
from AprobadorCentro a
inner join Usuario b on b.IdUsuario = a.IdUsuario 
								and a.Alta = 1
inner join SociedadCentro c on c.IdSociedadCentro = a.IdSociedadCentro 
								and c.Alta = 1
inner join CajaChicaEncabezado d on d.IdSociedadCentro = a.IdSociedadCentro 
								and d.Estado = 1
left join FacturaEncabezado f on f.IdCajaChica = d.IdCajaChica 
								and f.Estado = 1
								and f.OrdenesCosto = a.AUFNR
								and f.aprobada is null
								and f.Nivel = a.IdNivel
where (f.OrdenesCosto != '' and LTRIM(RTRIM(f.CentroCosto)) = '')
group by b.Usuario, b.Nombre, b.Correo
union all
select b.Usuario, b.Nombre, b.Correo, COUNT(e.IdCajaChica) Facturas
from AprobadorCentro a
inner join Usuario b on b.IdUsuario = a.IdUsuario 
						and a.Alta = 1
inner join SociedadCentro c on c.IdSociedadCentro = a.IdSociedadCentro 
						and c.Alta = 1
inner join CajaChicaEncabezado d on d.IdSociedadCentro = a.IdSociedadCentro 
						and d.Estado = 1
left join FacturaEncabezado e on e.IdCajaChica = d.IdCajaChica 
						and e.Estado = 1
						and e.CentroCosto = a.KOSTL
						and e.aprobada is null
						and e.Nivel = a.IdNivel
where (e.CentroCosto != '' and LTRIM(RTRIM(e.OrdenesCosto)) = '')
group by b.Usuario, b.Nombre, b.Correo) a
group by Usuario, Nombre, Correo
end
GO
/****** Object:  StoredProcedure [dbo].[Proveedores]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[Proveedores]
	@IdTipoDocumento smallint,
	@NumeroIdentificacion varchar(17),
	@Usuario varchar(16)
	
as
begin
select TipoDocumento, Descripcion, NumeroIdentificacion, Nombre, Direccion, a.UsuarioAlta, a.FechaCreacion, a.UsuarioModificacion, a.FechaModificacion
from Proveedor a
inner join TipoDocumento b on b.IdTipoDocumento = a.IdTipoDocumento
where b.IdTipoDocumento = case when @IdTipoDocumento = 0 then b.IdTipoDocumento else @IdTipoDocumento end
and a.NumeroIdentificacion = case when @NumeroIdentificacion = '' then a.NumeroIdentificacion else @NumeroIdentificacion  end
end
GO
/****** Object:  StoredProcedure [dbo].[RegistroContableCC]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[RegistroContableCC]
	@IdCajaChica int	

as
begin
select CuentaContable, IndicadorIVA, DefinicionCuentaContable, CargoAbono, 
case	
	when CargoAbono = 0 then sum(Valor)
	else 0 end cargo,
case	
	when CargoAbono = 1 then sum(Valor)
	else 0 end abono	
from RegistroContable a
inner join FacturaEncabezado b on a.IdFactura = b.IdFactura
where b.IdCajaChica = @IdCajaChica
and a.Alta = 1
and (b.Estado = 1 or b.Estado = 2)
and (b.Aprobada is null or b.Aprobada = 1)
group by CuentaContable, IndicadorIVA, DefinicionCuentaContable, CargoAbono
order by CargoAbono asc                
end
GO
/****** Object:  StoredProcedure [dbo].[RegistroContableFactura]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[RegistroContableFactura]
	@idfactura bigint
as
begin

declare @descripcion varchar(250);

set @descripcion = (SELECT LTRIM(STUFF((SELECT DISTINCT '; '+CAST(upper(Descripcion) AS VARCHAR(40)) 
					FROM facturadetalle 
					where IdFactura = @IdFactura FOR XML PATH('')),1,1,'')))

select CuentaContable, IndicadorIVA, DefinicionCuentaContable, CargoAbono, 
case	
	when CargoAbono = 0 then sum(Valor)
	else 0 end cargo,
case	
	when CargoAbono = 1 then sum(Valor)
	else 0 end abono, b.Serie, b.Numero,
d.Descripcion, NumeroIdentificacion, c.Nombre, c.Direccion,
cast(f.CodigoSociedad as varchar(4))+ '-' + cast(f.IdCentro as varchar(4)) + '-' + e.NumeroCajaChica +'-' + RIGHT(REPLICATE('0', 6)+ CAST(e.Correlativo  AS VARCHAR(6)), 6)   CodigoCC,
b.FechaFactura, g.cargo, h.abono, a.IdFactura, @descripcion
from RegistroContable a
inner join FacturaEncabezado b on a.IdFactura = b.IdFactura
inner join Proveedor c on c.IdProveedor = b.IdProveedor
inner join TipoDocumento d on d.IdTipoDocumento = c.IdTipoDocumento
inner join CajaChicaEncabezado e on e.IdCajaChica = b.IdCajaChica
inner join SociedadCentro f on f.IdSociedadCentro = e.IdSociedadCentro
inner join (
	select IdFactura, SUM(Valor) cargo
	from RegistroContable
	where IdFactura = @idfactura
	and CargoAbono = 0
	and Alta = 1
	group by IdFactura
)g on g.IdFactura = a.IdFactura
inner join (
	select IdFactura, SUM(Valor) abono
	from RegistroContable
	where IdFactura = @idfactura
	and CargoAbono = 1
	and Alta = 1
	group by IdFactura
)h on h.IdFactura = a.IdFactura
where a.IdFactura = @idfactura
and a.Alta = 1
and b.Estado = 1
group by CuentaContable, IndicadorIVA, DefinicionCuentaContable, CargoAbono, d.Descripcion, NumeroIdentificacion, c.Nombre, 
c.Direccion, f.CodigoSociedad, f.IdCentro, e.NumeroCajaChica, e.Correlativo, b.FechaFactura, b.Serie, b.Numero, g.IdFactura,
g.cargo, h.IdFactura, h.abono, a.IdFactura
order by CargoAbono asc                

end
GO
/****** Object:  StoredProcedure [dbo].[RPT_CajaChica]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE procedure [dbo].[RPT_CajaChica]

@Usuario as varchar(16) = '',
@FechaInicio as datetime = '',
@FechaFin as datetime = '',
@SociedadCentro as int = 0


as
begin			

Select
distinct(a.IdCajaChica),
CAST(c.CodigoSociedad as varchar(4)) + '-' + CAST(d.IdCentro as varchar(2)) + '-' + 
CAST(a.NumeroCajaChica as varchar(4)) + '-' + RIGHT(REPLICATE('0', 6)+ CAST(a.Correlativo  AS VARCHAR(6)), 6) CodigoCajaChica,
a.Descripcion, a.EncargadoCC, 
case 
	when a.Estado = 1 then 'Vigente' 
	when a.Estado = 2 then 'Cerrada' 
	when a.Estado = 0 then 'Anulada'
	end Estado,
a.FechaCreacion,
--c.Nombre Sociedad, 
case 
	when @SociedadCentro = 0 then ''
	else c.Nombre end Sociedad,  
d.Nombre Centro, f.Nombre Usuario
 from CajaChicaEncabezado a
 inner join SociedadCentro b on b.IdSociedadCentro = a.IdSociedadCentro
 inner join Sociedad c on c.CodigoSociedad = b.CodigoSociedad
 inner join Centro d on d.IdCentro = b.IdCentro
 inner join UsuarioSociedadCentro e on e.Usuario = @Usuario
 inner join Usuario f on f.Usuario = e.Usuario
 where 
 a.FechaCreacion between @FechaInicio and @FechaFin
 and b.CodigoSociedad = case when @SociedadCentro = 0 then b.CodigoSociedad else @SociedadCentro end
end

GO
/****** Object:  StoredProcedure [dbo].[RPT_CentroCosto]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[RPT_CentroCosto]

	@BUKRS VARCHAR(4) = 0,
	@Usuario varchar(16) = ''
as
begin
select distinct a.BUKRS, c.Nombre NombreSociedad, a.KOSTL, a.VERAK
from sap.CentroCostoTMP a
inner join sap.CuentaPorCentroCostoTMP b on b.BUKRS = a.BUKRS
											and b.KOSTL = a.KOSTL
inner join Sociedad c on c.CodigoSociedad = b.BUKRS
where c.CodigoSociedad = case when @BUKRS = 0 then c.CodigoSociedad else @BUKRS end
end
GO
/****** Object:  StoredProcedure [dbo].[RPT_CuentasContables]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[RPT_CuentasContables]

	@codigoSociedad varchar(4) = 0,
	@Usuario varchar(16)=''
as

begin
select BUKRS, b.Nombre, SAKNR, TXT50
from sap.CuentasContables a
inner join Sociedad b on b.CodigoSociedad = a.BUKRS
where b.CodigoSociedad = case when @codigoSociedad = '0' then b.CodigoSociedad else @codigoSociedad end
end
GO
/****** Object:  StoredProcedure [dbo].[RPT_Facturas]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE procedure [dbo].[RPT_Facturas]

@FechaInicio as datetime,
@FechaFin as datetime,
@Usuario as varchar(15),
@Sociedad as varchar(15) = '0'

as
begin			

select distinct(a.IdFactura), a.Serie, a.Numero, a.FechaFactura, a.EsEspecial,
a.CentroCosto, a.OrdenesCosto,
 a.ValorTotal, a.IVA,
b.NumeroIdentificacion, b.Nombre,
CAST(h.CodigoSociedad as varchar(4)) + '-' + CAST(i.IdCentro as varchar(2)) + '-' + 
CAST(c.NumeroCajaChica as varchar(4)) + '-' + RIGHT(REPLICATE('0', 6)+ CAST(c.Correlativo  AS VARCHAR(6)), 6) CodigoCajaChica,
c.EncargadoCC, c.Descripcion, c.TipoOperacion,
case
	when a.Aprobada = 0 then 'NO'
	when a.Aprobada = 1 then 'SI'
	else '' end Aprobada,
case when a.Estado = 0 then 'ANULADA' 
	 when a.Estado = 1 then 'VIGENTE'
	 when a.Estado = 2 then 'PROCESADA CXP'
	 else 'RECHAZADO CXP' END [ESTADO FACTURA],
--Registro
a.UsuarioCreacion UsuarioRegistro, a.FechaCreacion FechaRegistro,
--Aprobacion
f.Usuario UsuarioAprobo, d.FechaAlta FechaAprobacion,
--Revision 
--case 
--	when a.Estado = 2 then 'Aprobada'
--	when a.Estado = 3 then 'Rechazada'
--	else '' end Revisada,
case 
	--when j.Usuario != NULL 
	--then j.Usuario 
	--else '' end UusarioRevisor,
	when a.Estado = 2 or a.Estado = 3 
	then j.Usuario 
	else '' end UsuarioRevisor, 
case 
	when (j.Usuario != null or j.Usuario != '') and (a.Estado = 2 or a.Estado = 3 )
	then a.FechaModificacion
	else null end FechaRevision,  
--j.Usuario Revisor,
--a.UsuarioModifico UsuarioRevisor, a.FechaModificacion FechaRevision,
--h.Nombre Sociedad, 
case 
	when @Sociedad = '0' then ''
	else h.Nombre end Sociedad,
i.Nombre Centro, f.Nombre Usuario
from FacturaEncabezado a
inner join Proveedor b on b.IdProveedor = a.IdProveedor
inner join CajaChicaEncabezado c on c.IdCajaChica = a.IdCajaChica and c.Estado != 0
left join AprobacionFacturas d on d.IdFactura = a.IdFactura
left join AprobadorCentro e on e.IdAprobadorCentro = d.IdAprobadorCentro
left join Usuario f on f.IdUsuario = e.IdUsuario
inner join SociedadCentro g on g.IdSociedadCentro = c.IdSociedadCentro
inner join Sociedad h on h.CodigoSociedad = g.CodigoSociedad
inner join Centro i on i.IdCentro = g.IdCentro
left join UsuarioSociedadCentro j on j.Usuario = a.UsuarioModifico
where a.FechaFactura between @FechaInicio and @FechaFin and
--h.CodigoSociedad = @Sociedad 
h.CodigoSociedad = case when @Sociedad = '0' then h.CodigoSociedad else @Sociedad end

end

GO
/****** Object:  StoredProcedure [dbo].[RPT_OrdenCosto]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[RPT_OrdenCosto]

	@BUKRS VARCHAR(4) = 0,
	@Usuario varchar(16) = ''
as
begin
select distinct a.BUKRS, c.Nombre NombreSociedad, a.AUFNR, a.KTEXT
from sap.OrdenCOTMP  a
inner join sap.CuentaPorOrdenCOTMP b on b.BUKRS = a.BUKRS
											and b.AUART = a.AUART
inner join Sociedad c on c.CodigoSociedad = b.BUKRS
where c.CodigoSociedad = case when @BUKRS = 0 then c.CodigoSociedad else @BUKRS end
end
GO
/****** Object:  StoredProcedure [dbo].[RPT_UsuarioCentroCosto]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[RPT_UsuarioCentroCosto]

		@bukrs varchar(4) = '0',
		@centroCosto varchar(10) = '1512010001',
		@usuario varchar(16) 

as
begin
select distinct e.CodigoSociedad, e.Nombre sociedad, f.Nombre centro, a.Nombre NombreUsuario, c.KOSTL, c.VERAK, b.Alta, b.UsuarioCreacion, b.FechaCreacion, b.UsuarioModificacion, b.FechaModificacion
from Usuario a
inner join UsuarioCentroCosto b on b.Usuario = a.Usuario
left join sap.CentroCostoTMP c on c.KOSTL = b.CentroCosto
inner join SociedadCentro d on d.CodigoSociedad = c.BUKRS
inner join Sociedad e on e.CodigoSociedad = d.CodigoSociedad
inner join Centro f on f.IdCentro = d.IdCentro
where b.IdSociedadCentro = d.IdSociedadCentro
and e.CodigoSociedad = case when @bukrs = '0' then e.CodigoSociedad else @bukrs end
and c.KOSTL = case when @centroCosto = '' then c.KOSTL else @centroCosto end
end 


GO
/****** Object:  StoredProcedure [dbo].[RPT_UsuarioOrdenCosto]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[RPT_UsuarioOrdenCosto]

		@bukrs varchar(4) = '0',
		@ordenCosto varchar(12) = '000000100000',
		@usuario varchar(16) 

as
begin

select distinct e.CodigoSociedad, e.Nombre sociedad, f.Nombre centro, a.Nombre NombreUsuario, c.AUFNR, c.KTEXT, b.Alta, b.UsuarioCreacion, b.FechaCreacion, b.UsuarioModificacion, b.FechaModificacion
from Usuario a
inner join UsuarioOrdenCompra b on b.Usuario = a.Usuario
left join sap.OrdenCOTMP c on c.AUFNR = b.OrdenCompra
inner join SociedadCentro d on d.IdSociedadCentro = b.IdSociedadCentro
inner join Sociedad e on e.CodigoSociedad = d.CodigoSociedad
inner join Usuario f on f.Usuario = a.Usuario
where b.IdSociedadCentro = d.IdSociedadCentro
and e.CodigoSociedad = case when @bukrs = '0' then e.CodigoSociedad else @bukrs end
and c.AUFNR = case when @ordenCosto = '' then c.AUFNR else @ordenCosto end
end 


GO
/****** Object:  StoredProcedure [dbo].[RPT_UsuarioRol]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[RPT_UsuarioRol]

		@codigoSociedad varchar(4) = 0,
		@usuario varchar(16) = '',
		@usuarioRep varchar(16)= ''

as
begin
select distinct d.CodigoSociedad, d.Nombre NombreSociedad, e.Nombre NombreCentro, a.Usuario, a.Nombre, z.Rol, y.Alta, y.UsuarioAlta, y.FechaAlta, y.UsuarioModificacion, y.FechaModificacion
from Rol z
inner join UsuarioRol y on y.IdRol = z.IdRol
inner join Usuario a on a.IdUsuario = y.IdUsuario
inner join UsuarioCentroCosto b on b.Usuario = a.Usuario
inner join SociedadCentro c on c.IdSociedadCentro = b.IdSociedadCentro
inner join Sociedad d on d.CodigoSociedad = c.CodigoSociedad
inner join Centro e on e.IdCentro = c.IdCentro
where a.Usuario = case when @usuario = '' then a.Usuario else @usuario end 
and d.CodigoSociedad = case when @codigoSociedad = 0 then d.CodigoSociedad else @codigoSociedad end
union
select distinct d.CodigoSociedad, d.Nombre NombreSociedad, e.Nombre NombreCentro, a.Usuario, a.Nombre, z.Rol, y.Alta, y.UsuarioAlta, y.FechaAlta, y.UsuarioModificacion, y.FechaModificacion
from Rol z
inner join UsuarioRol y on y.IdRol = z.IdRol
inner join Usuario a on a.IdUsuario = y.IdUsuario
inner join UsuarioOrdenCompra b on b.Usuario = a.Usuario
inner join SociedadCentro c on c.IdSociedadCentro = b.IdSociedadCentro
inner join Sociedad d on d.CodigoSociedad = c.CodigoSociedad
inner join Centro e on e.IdCentro = c.IdCentro
where a.Usuario = case when @usuario = '' then a.Usuario else @usuario end 
and d.CodigoSociedad = case when @codigoSociedad = 0 then d.CodigoSociedad else @codigoSociedad end
union 
select distinct d.CodigoSociedad, d.Nombre NombreSociedad, e.Nombre NombreCentro, a.Usuario, a.Nombre, z.Rol, y.Alta, y.UsuarioAlta, y.FechaAlta, y.UsuarioModificacion, y.FechaModificacion
from Rol z
inner join UsuarioRol y on y.IdRol = z.IdRol
inner join Usuario a on a.IdUsuario = y.IdUsuario
inner join UsuarioSociedadCentro b on b.Usuario = a.Usuario
inner join SociedadCentro c on c.IdSociedadCentro = b.IdSociedadCentro
inner join Sociedad d on d.CodigoSociedad = c.CodigoSociedad
inner join Centro e on e.IdCentro = c.IdCentro
where a.Usuario = case when @usuario = '' then a.Usuario else @usuario end 
and d.CodigoSociedad = case when @codigoSociedad = 0 then d.CodigoSociedad else @codigoSociedad end
union
select distinct d.CodigoSociedad, d.Nombre NombreSociedad, e.Nombre NombreCentro, a.Usuario, a.Nombre, z.Rol, y.Alta, y.UsuarioAlta, y.FechaAlta, y.UsuarioModificacion, y.FechaModificacion
from Rol z
inner join UsuarioRol y on y.IdRol = z.IdRol
inner join Usuario a on a.IdUsuario = y.IdUsuario
inner join AprobadorCentro b on b.IdUsuario = a.IdUsuario
inner join SociedadCentro c on c.IdSociedadCentro = b.IdSociedadCentro
inner join Sociedad d on d.CodigoSociedad = c.CodigoSociedad
inner join Centro e on e.IdCentro = c.IdCentro
where a.Usuario = case when @usuario = '' then a.Usuario else @usuario end 
and d.CodigoSociedad = case when @codigoSociedad = 0 then d.CodigoSociedad else @codigoSociedad end
end
GO
/****** Object:  StoredProcedure [dbo].[RPT_Usuarios]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[RPT_Usuarios]
		@codigoSociedad varchar(4) = 0,
		@usuario varchar(16) = '',
		@usuarioRep varchar(16)= ''
		

as
begin
select distinct d.CodigoSociedad, d.Nombre NombreSociedad, e.Nombre NombreCentro, a.Usuario, a.Nombre, a.Correo, a.Alta, a.UsuarioAlta, a.FechaAlta, a.UsuarioModificacion, a.FechaModificacion
from Usuario a
inner join UsuarioCentroCosto b on b.Usuario = a.Usuario
inner join SociedadCentro c on c.IdSociedadCentro = b.IdSociedadCentro
inner join Sociedad d on d.CodigoSociedad = c.CodigoSociedad
inner join Centro e on e.IdCentro = c.IdCentro
where a.Usuario = case when @usuario = '' then a.Usuario else @usuario end 
and d.CodigoSociedad = case when @codigoSociedad = 0 then d.CodigoSociedad else @codigoSociedad end
union
select distinct d.CodigoSociedad, d.Nombre NombreSociedad, e.Nombre NombreCentro, a.Usuario, a.Nombre, a.Correo, a.Alta, a.UsuarioAlta, a.FechaAlta, a.UsuarioModificacion, a.FechaModificacion
from Usuario a
inner join UsuarioOrdenCompra b on b.Usuario = a.Usuario
inner join SociedadCentro c on c.IdSociedadCentro = b.IdSociedadCentro
inner join Sociedad d on d.CodigoSociedad = c.CodigoSociedad
inner join Centro e on e.IdCentro = c.IdCentro
where a.Usuario = case when @usuario = '' then a.Usuario else @usuario end 
and d.CodigoSociedad = case when @codigoSociedad = 0 then d.CodigoSociedad else @codigoSociedad end
union 
select distinct d.CodigoSociedad, d.Nombre NombreSociedad, e.Nombre NombreCentro, a.Usuario, a.Nombre, a.Correo, a.Alta, a.UsuarioAlta, a.FechaAlta, a.UsuarioModificacion, a.FechaModificacion
from Usuario a
inner join UsuarioSociedadCentro b on b.Usuario = a.Usuario
inner join SociedadCentro c on c.IdSociedadCentro = b.IdSociedadCentro
inner join Sociedad d on d.CodigoSociedad = c.CodigoSociedad
inner join Centro e on e.IdCentro = c.IdCentro
where a.Usuario = case when @usuario = '' then a.Usuario else @usuario end 
and d.CodigoSociedad = case when @codigoSociedad = 0 then d.CodigoSociedad else @codigoSociedad end
union
select distinct d.CodigoSociedad, d.Nombre NombreSociedad, e.Nombre NombreCentro, a.Usuario, a.Nombre, a.Correo, a.Alta, a.UsuarioAlta, a.FechaAlta, a.UsuarioModificacion, a.FechaModificacion
from Usuario a
inner join AprobadorCentro b on b.IdUsuario = a.IdUsuario
inner join SociedadCentro c on c.IdSociedadCentro = b.IdSociedadCentro
inner join Sociedad d on d.CodigoSociedad = c.CodigoSociedad
inner join Centro e on e.IdCentro = c.IdCentro
where a.Usuario = case when @usuario = '' then a.Usuario else @usuario end 
and d.CodigoSociedad = case when @codigoSociedad = 0 then d.CodigoSociedad else @codigoSociedad end
end
GO
/****** Object:  StoredProcedure [dbo].[RPT_UsuarioSociedad]    Script Date: 13/04/2016 2:11:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE [dbo].[RPT_UsuarioSociedad]
		@Usuario varchar(16) = ''

as
begin

SELECT 0 CodigoSociedad, 'TODAS LAS SOCIEDADES' Nombre
union
select distinct d.CodigoSociedad, d.Nombre NombreSociedad
from Usuario a
inner join UsuarioCentroCosto b on b.Usuario = a.Usuario
inner join SociedadCentro c on c.IdSociedadCentro = b.IdSociedadCentro
inner join Sociedad d on d.CodigoSociedad = c.CodigoSociedad
inner join Centro e on e.IdCentro = c.IdCentro
where a.Usuario = @Usuario 
union
select distinct d.CodigoSociedad, d.Nombre NombreSociedad
from Usuario a
inner join UsuarioOrdenCompra b on b.Usuario = a.Usuario
inner join SociedadCentro c on c.IdSociedadCentro = b.IdSociedadCentro
inner join Sociedad d on d.CodigoSociedad = c.CodigoSociedad
inner join Centro e on e.IdCentro = c.IdCentro
where a.Usuario = @Usuario 
union 
select distinct d.CodigoSociedad, d.Nombre NombreSociedad
from Usuario a
inner join UsuarioSociedadCentro b on b.Usuario = a.Usuario
inner join SociedadCentro c on c.IdSociedadCentro = b.IdSociedadCentro
inner join Sociedad d on d.CodigoSociedad = c.CodigoSociedad
inner join Centro e on e.IdCentro = c.IdCentro
where a.Usuario = @Usuario 
union
select distinct d.CodigoSociedad, d.Nombre NombreSociedad
from Usuario a
inner join AprobadorCentro b on b.IdUsuario = a.IdUsuario
inner join SociedadCentro c on c.IdSociedadCentro = b.IdSociedadCentro
inner join Sociedad d on d.CodigoSociedad = c.CodigoSociedad
inner join Centro e on e.IdCentro = c.IdCentro
where a.Usuario = @Usuario 
end
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 rechazado, 1 aprobado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AprobacionFacturas', @level2type=N'COLUMN',@level2name=N'Estado'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 anulado, 1 vigente, 2 cerrada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CajaChicaEncabezado', @level2type=N'COLUMN',@level2name=N'Estado'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 baja, 1 vigente, 2 aprobado, 3 rechazado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FacturaEncabezado', @level2type=N'COLUMN',@level2name=N'Estado'
GO
USE [master]
GO
ALTER DATABASE [RegistroFacturasSAP_DESA] SET  READ_WRITE 
GO
