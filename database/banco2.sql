USE [master]
GO
/****** Objeto: Database [Banco2] Fecha de script: 10/11/2026 04:18:28 p. m. ******/
CREATE DATABASE [Banco2]
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Banco2].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Banco2] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Banco2] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Banco2] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Banco2] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Banco2] SET ARITHABORT OFF 
GO
ALTER DATABASE [Banco2] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Banco2] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Banco2] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Banco2] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Banco2] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Banco2] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Banco2] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Banco2] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Banco2] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Banco2] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Banco2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Banco2] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Banco2] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Banco2] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Banco2] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Banco2] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Banco2] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Banco2] SET RECOVERY FULL 
GO
ALTER DATABASE [Banco2] SET  MULTI_USER 
GO
ALTER DATABASE [Banco2] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Banco2] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Banco2] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Banco2] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Banco2] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Banco2] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Banco2', N'ON'
GO
ALTER DATABASE [Banco2] SET QUERY_STORE = ON
GO
ALTER DATABASE [Banco2] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Banco2]
GO
/****** Objeto: Table [dbo].[BancosExternos] Fecha de script: 10/11/2026 04:18:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BancosExternos](
	[BancoExternoID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[CodigoBanco] [varchar](10) NOT NULL,
	[Activo] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BancoExternoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[CodigoBanco] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Cajeros] Fecha de script: 10/11/2026 04:18:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cajeros](
	[CajeroID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Nombre] [nvarchar](80) NOT NULL,
	[Ubicacion] [nvarchar](200) NULL,
	[IP] [varchar](45) NOT NULL,
	[Puerto] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
	[UltimaConex] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CajeroID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[IP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Clientes] Fecha de script: 10/11/2026 04:18:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clientes](
	[ClienteID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Nombre] [nvarchar](60) NOT NULL,
	[ApellidoP] [nvarchar](60) NOT NULL,
	[ApellidoM] [nvarchar](60) NULL,
	[Activo] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ClienteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Comprobantes] Fecha de script: 10/11/2026 04:18:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comprobantes](
	[ComprobanteID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[OperacionID] [int] NOT NULL,
	[Impreso] [bit] NOT NULL,
	[FechaImpresion] [datetime] NULL,
	[Contenido] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ComprobanteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Cuentas] Fecha de script: 10/11/2026 04:18:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cuentas](
	[CuentaID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[ClienteID] [int] NOT NULL,
	[NumeroCuenta] [varchar](20) NOT NULL,
	[Saldo] [decimal](18, 2) NOT NULL,
	[FechaApertura] [date] NOT NULL,
	[Activa] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CuentaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[NumeroCuenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[DepositosEfectivo] Fecha de script: 10/11/2026 04:18:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DepositosEfectivo](
	[DepositoEfectivoID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[OperacionID] [int] NOT NULL,
	[CuentaDestinoID] [int] NOT NULL,
	[EsDonacion] [bit] NOT NULL,
	[EsPagoServicio] [bit] NOT NULL,
	[ReferenciaCliente] [nvarchar](50) NULL,
 CONSTRAINT [PK_DepositosEfectivo] PRIMARY KEY CLUSTERED 
(
	[DepositoEfectivoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Donaciones] Fecha de script: 10/11/2026 04:18:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Donaciones](
	[DonacionID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[OperacionID] [int] NOT NULL,
	[OrganizacionNombre] [nvarchar](150) NOT NULL,
	[CLABE] [varchar](18) NOT NULL,
	[CausaSocial] [nvarchar](200) NULL,
	[CertificadoDonacion] [nvarchar](50) NULL,
 CONSTRAINT [PK_Donaciones] PRIMARY KEY CLUSTERED 
(
	[DonacionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Operaciones] Fecha de script: 10/11/2026 04:18:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Operaciones](
	[OperacionID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[SesionID] [int] NOT NULL,
	[CuentaOrigenID] [int] NOT NULL,
	[TipoOperacion] [nvarchar](100) NOT NULL,
	[Monto] [decimal](18, 2) NOT NULL,
	[Comision] [decimal](18, 2) NOT NULL,
	[EsOtroBanco] [bit] NOT NULL,
	[Folio] [varchar](30) NOT NULL,
	[FechaHora] [datetime] NOT NULL,
	[Estatus] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OperacionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Folio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[OrganizacionesDonacion] Fecha de script: 10/11/2026 04:18:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganizacionesDonacion](
	[OrganizacionID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Nombre] [nvarchar](150) NOT NULL,
	[CLABE] [varchar](18) NOT NULL,
	[Icono] [nvarchar](10) NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_OrganizacionesDonacion] PRIMARY KEY CLUSTERED 
(
	[OrganizacionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[PagosServicios] Fecha de script: 10/11/2026 04:18:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PagosServicios](
	[PagoServicioID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[OperacionID] [int] NOT NULL,
	[ServicioID] [int] NOT NULL,
	[Referencia] [nvarchar](50) NOT NULL,
	[Periodo] [nvarchar](20) NULL,
	[Convenio] [nvarchar](30) NULL,
 CONSTRAINT [PK_PagosServicios] PRIMARY KEY CLUSTERED 
(
	[PagoServicioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[RetirosSinTarjeta] Fecha de script: 10/11/2026 04:18:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RetirosSinTarjeta](
	[RetiroSTID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[OperacionID] [int] NOT NULL,
	[CuentaID] [int] NOT NULL,
	[ClaveRetiro] [varchar](8) NOT NULL,
	[ClaveGenerada] [datetime] NOT NULL,
	[ClaveUsada] [datetime] NULL,
	[IntentosFallidos] [int] NOT NULL,
 CONSTRAINT [PK_RetirosSinTarjeta] PRIMARY KEY CLUSTERED 
(
	[RetiroSTID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Servicios] Fecha de script: 10/11/2026 04:18:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Servicios](
	[ServicioID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[NombreServicio] [nvarchar](100) NOT NULL,
	[Icono] [nvarchar](10) NULL,
	[Comision] [decimal](18, 2) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Servicios] PRIMARY KEY CLUSTERED 
(
	[ServicioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_Servicios_Nombre] UNIQUE NONCLUSTERED 
(
	[NombreServicio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Sesiones] Fecha de script: 10/11/2026 04:18:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sesiones](
	[SesionID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[TarjetaID] [int] NOT NULL,
	[CajeroID] [int] NOT NULL,
	[FechaInicio] [datetime] NOT NULL,
	[FechaFin] [datetime] NULL,
	[Exitosa] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SesionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Tarjetas] Fecha de script: 10/11/2026 04:18:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tarjetas](
	[TarjetaID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[CuentaID] [int] NOT NULL,
	[NumeroTarjeta] [char](16) NOT NULL,
	[Terminacion] [char](4) NOT NULL,
	[FechaExpiracion] [date] NOT NULL,
	[NIPHash] [varbinary](64) NOT NULL,
	[Bloqueada] [bit] NOT NULL,
	[FechaEmision] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TarjetaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[NumeroTarjeta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[TiposOperacion] Fecha de script: 10/11/2026 04:18:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TiposOperacion](
	[Descripcion] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Descripcion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BancosExternos] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Cajeros] ADD  DEFAULT ((8080)) FOR [Puerto]
GO
ALTER TABLE [dbo].[Cajeros] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Clientes] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Comprobantes] ADD  DEFAULT ((0)) FOR [Impreso]
GO
ALTER TABLE [dbo].[Cuentas] ADD  DEFAULT ((0.00)) FOR [Saldo]
GO
ALTER TABLE [dbo].[Cuentas] ADD  DEFAULT (getdate()) FOR [FechaApertura]
GO
ALTER TABLE [dbo].[Cuentas] ADD  DEFAULT ((1)) FOR [Activa]
GO
ALTER TABLE [dbo].[DepositosEfectivo] ADD  DEFAULT ((0)) FOR [EsDonacion]
GO
ALTER TABLE [dbo].[DepositosEfectivo] ADD  DEFAULT ((0)) FOR [EsPagoServicio]
GO
ALTER TABLE [dbo].[Operaciones] ADD  DEFAULT ((0.00)) FOR [Monto]
GO
ALTER TABLE [dbo].[Operaciones] ADD  DEFAULT ((0.00)) FOR [Comision]
GO
ALTER TABLE [dbo].[Operaciones] ADD  DEFAULT ((0)) FOR [EsOtroBanco]
GO
ALTER TABLE [dbo].[Operaciones] ADD  DEFAULT (getdate()) FOR [FechaHora]
GO
ALTER TABLE [dbo].[Operaciones] ADD  DEFAULT ('Completada') FOR [Estatus]
GO
ALTER TABLE [dbo].[OrganizacionesDonacion] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[RetirosSinTarjeta] ADD  DEFAULT ((0)) FOR [IntentosFallidos]
GO
ALTER TABLE [dbo].[Servicios] ADD  DEFAULT ((5.00)) FOR [Comision]
GO
ALTER TABLE [dbo].[Servicios] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Sesiones] ADD  DEFAULT (getdate()) FOR [FechaInicio]
GO
ALTER TABLE [dbo].[Sesiones] ADD  DEFAULT ((1)) FOR [Exitosa]
GO
ALTER TABLE [dbo].[Tarjetas] ADD  DEFAULT ((0)) FOR [Bloqueada]
GO
ALTER TABLE [dbo].[Tarjetas] ADD  DEFAULT (getdate()) FOR [FechaEmision]
GO
ALTER TABLE [dbo].[Comprobantes]  WITH CHECK ADD FOREIGN KEY([OperacionID])
REFERENCES [dbo].[Operaciones] ([OperacionID])
GO
ALTER TABLE [dbo].[Comprobantes]  WITH CHECK ADD FOREIGN KEY([OperacionID])
REFERENCES [dbo].[Operaciones] ([OperacionID])
GO
ALTER TABLE [dbo].[Cuentas]  WITH CHECK ADD FOREIGN KEY([ClienteID])
REFERENCES [dbo].[Clientes] ([ClienteID])
GO
ALTER TABLE [dbo].[Cuentas]  WITH CHECK ADD FOREIGN KEY([ClienteID])
REFERENCES [dbo].[Clientes] ([ClienteID])
GO
ALTER TABLE [dbo].[DepositosEfectivo]  WITH CHECK ADD  CONSTRAINT [FK_DepositosEfectivo_Cuentas] FOREIGN KEY([CuentaDestinoID])
REFERENCES [dbo].[Cuentas] ([CuentaID])
GO
ALTER TABLE [dbo].[DepositosEfectivo] CHECK CONSTRAINT [FK_DepositosEfectivo_Cuentas]
GO
ALTER TABLE [dbo].[DepositosEfectivo]  WITH CHECK ADD  CONSTRAINT [FK_DepositosEfectivo_Operaciones] FOREIGN KEY([OperacionID])
REFERENCES [dbo].[Operaciones] ([OperacionID])
GO
ALTER TABLE [dbo].[DepositosEfectivo] CHECK CONSTRAINT [FK_DepositosEfectivo_Operaciones]
GO
ALTER TABLE [dbo].[Donaciones]  WITH CHECK ADD  CONSTRAINT [FK_Donaciones_Operaciones] FOREIGN KEY([OperacionID])
REFERENCES [dbo].[Operaciones] ([OperacionID])
GO
ALTER TABLE [dbo].[Donaciones] CHECK CONSTRAINT [FK_Donaciones_Operaciones]
GO
ALTER TABLE [dbo].[Operaciones]  WITH CHECK ADD FOREIGN KEY([CuentaOrigenID])
REFERENCES [dbo].[Cuentas] ([CuentaID])
GO
ALTER TABLE [dbo].[Operaciones]  WITH CHECK ADD FOREIGN KEY([CuentaOrigenID])
REFERENCES [dbo].[Cuentas] ([CuentaID])
GO
ALTER TABLE [dbo].[Operaciones]  WITH CHECK ADD FOREIGN KEY([SesionID])
REFERENCES [dbo].[Sesiones] ([SesionID])
GO
ALTER TABLE [dbo].[Operaciones]  WITH CHECK ADD FOREIGN KEY([SesionID])
REFERENCES [dbo].[Sesiones] ([SesionID])
GO
ALTER TABLE [dbo].[Operaciones]  WITH CHECK ADD FOREIGN KEY([TipoOperacion])
REFERENCES [dbo].[TiposOperacion] ([Descripcion])
GO
ALTER TABLE [dbo].[Operaciones]  WITH CHECK ADD FOREIGN KEY([TipoOperacion])
REFERENCES [dbo].[TiposOperacion] ([Descripcion])
GO
ALTER TABLE [dbo].[PagosServicios]  WITH CHECK ADD  CONSTRAINT [FK_PagosServicios_Operaciones] FOREIGN KEY([OperacionID])
REFERENCES [dbo].[Operaciones] ([OperacionID])
GO
ALTER TABLE [dbo].[PagosServicios] CHECK CONSTRAINT [FK_PagosServicios_Operaciones]
GO
ALTER TABLE [dbo].[PagosServicios]  WITH CHECK ADD  CONSTRAINT [FK_PagosServicios_Servicios] FOREIGN KEY([ServicioID])
REFERENCES [dbo].[Servicios] ([ServicioID])
GO
ALTER TABLE [dbo].[PagosServicios] CHECK CONSTRAINT [FK_PagosServicios_Servicios]
GO
ALTER TABLE [dbo].[RetirosSinTarjeta]  WITH CHECK ADD  CONSTRAINT [FK_RetirosSinTarjeta_Cuentas] FOREIGN KEY([CuentaID])
REFERENCES [dbo].[Cuentas] ([CuentaID])
GO
ALTER TABLE [dbo].[RetirosSinTarjeta] CHECK CONSTRAINT [FK_RetirosSinTarjeta_Cuentas]
GO
ALTER TABLE [dbo].[RetirosSinTarjeta]  WITH CHECK ADD  CONSTRAINT [FK_RetirosSinTarjeta_Operaciones] FOREIGN KEY([OperacionID])
REFERENCES [dbo].[Operaciones] ([OperacionID])
GO
ALTER TABLE [dbo].[RetirosSinTarjeta] CHECK CONSTRAINT [FK_RetirosSinTarjeta_Operaciones]
GO
ALTER TABLE [dbo].[Sesiones]  WITH CHECK ADD FOREIGN KEY([CajeroID])
REFERENCES [dbo].[Cajeros] ([CajeroID])
GO
ALTER TABLE [dbo].[Sesiones]  WITH CHECK ADD FOREIGN KEY([CajeroID])
REFERENCES [dbo].[Cajeros] ([CajeroID])
GO
ALTER TABLE [dbo].[Sesiones]  WITH CHECK ADD FOREIGN KEY([TarjetaID])
REFERENCES [dbo].[Tarjetas] ([TarjetaID])
GO
ALTER TABLE [dbo].[Sesiones]  WITH CHECK ADD FOREIGN KEY([TarjetaID])
REFERENCES [dbo].[Tarjetas] ([TarjetaID])
GO
ALTER TABLE [dbo].[Tarjetas]  WITH CHECK ADD FOREIGN KEY([CuentaID])
REFERENCES [dbo].[Cuentas] ([CuentaID])
GO
ALTER TABLE [dbo].[Tarjetas]  WITH CHECK ADD FOREIGN KEY([CuentaID])
REFERENCES [dbo].[Cuentas] ([CuentaID])
GO
/****** Objeto: StoredProcedure [dbo].[sp_RegistrarOperacion] Fecha de script: 10/11/2026 04:18:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*==============================================================
  PROCEDIMIENTO: sp_RegistrarOperacion (VERSIÓN ACTUALIZADA)
==============================================================*/
CREATE PROCEDURE [dbo].[sp_RegistrarOperacion]
(
    @SesionID INT,
    @CuentaOrigenID INT,
    @TipoOperacion NVARCHAR(100),
    @Monto DECIMAL(18,2),
    @EsOtroBanco BIT = 0,
    @Folio VARCHAR(30),
    
    -- PARÁMETROS PARA PAGO DE SERVICIOS
    @ServicioID INT = NULL,                    
    @ReferenciaServicio NVARCHAR(50) = NULL,   
    @Periodo NVARCHAR(20) = NULL,              
    @Convenio NVARCHAR(30) = NULL,
    
    -- PARÁMETROS PARA DONACIONES
    @OrganizacionNombre NVARCHAR(150) = NULL,
    @CLABE VARCHAR(18) = NULL,
    @CausaSocial NVARCHAR(200) = NULL,
    @CertificadoDonacion NVARCHAR(50) = NULL,
    
    -- PARÁMETROS PARA RETIRO SIN TARJETA
    @ClaveRetiro VARCHAR(8) = NULL,
    
    -- PARÁMETROS PARA DEPÓSITO EN EFECTIVO
    @CuentaDestinoEfectivoID INT = NULL,
    @EsDonacionEfectivo BIT = 0,
    @EsPagoServicioEfectivo BIT = 0,
    @ReferenciaCliente NVARCHAR(50) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Comision DECIMAL(18,2) = 0.00;
    DECLARE @SaldoActual DECIMAL(18,2);
    DECLARE @OperacionID INT;

    -- =======================================================
    -- 1. VALIDAR TIPO DE OPERACIÓN
    -- =======================================================
    IF NOT EXISTS (SELECT 1 FROM TiposOperacion WHERE Descripcion = @TipoOperacion)
    BEGIN
        RAISERROR('Tipo de operación no válido.', 16, 1);
        RETURN;
    END

    -- =======================================================
    -- 2. CALCULAR COMISIÓN
    -- =======================================================
    IF @TipoOperacion = 'Retiro' AND @EsOtroBanco = 1
        SET @Comision = 15.00;
    ELSE IF @TipoOperacion = 'Deposito' AND @EsOtroBanco = 1
        SET @Comision = 10.00;
    ELSE IF @TipoOperacion = 'Servicios'
        SET @Comision = 5.00;
    ELSE IF @TipoOperacion IN ('Donaciones', 'Donacion')
        SET @Comision = 0.00;

    -- =======================================================
    -- 3. OBTENER SALDO ACTUAL
    -- =======================================================
    SELECT @SaldoActual = Saldo
    FROM Cuentas
    WHERE CuentaID = @CuentaOrigenID;

    IF @SaldoActual IS NULL
    BEGIN
        RAISERROR('Cuenta no encontrada.', 16, 1);
        RETURN;
    END

    -- =======================================================
    -- 4. OPERACIONES QUE DESCUENTAN SALDO
    -- =======================================================
    IF @TipoOperacion IN ('Retiro', 'Servicios', 'Donaciones', 'Donacion')
    BEGIN
        IF @SaldoActual < (@Monto + @Comision)
        BEGIN
            RAISERROR('Saldo insuficiente.', 16, 1);
            RETURN;
        END

        UPDATE Cuentas
        SET Saldo = Saldo - (@Monto + @Comision)
        WHERE CuentaID = @CuentaOrigenID;
    END

    -- =======================================================
    -- 5. DEPÓSITOS (abonan a la cuenta)
    -- =======================================================
    IF @TipoOperacion = 'Deposito'
    BEGIN
        IF @Monto <= @Comision
        BEGIN
            RAISERROR('El monto debe ser mayor que la comisión.', 16, 1);
            RETURN;
        END

        UPDATE Cuentas
        SET Saldo = Saldo + (@Monto - @Comision)
        WHERE CuentaID = @CuentaOrigenID;
    END

    -- =======================================================
    -- 6. INSERTAR EN TABLA OPERACIONES
    -- =======================================================
    INSERT INTO Operaciones (
        SesionID,
        CuentaOrigenID,
        TipoOperacion,
        Monto,
        Comision,
        EsOtroBanco,
        Folio,
        Estatus
    )
    VALUES (
        @SesionID,
        @CuentaOrigenID,
        @TipoOperacion,
        @Monto,
        @Comision,
        @EsOtroBanco,
        @Folio,
        'Completada'
    );
    
    SET @OperacionID = SCOPE_IDENTITY();

    -- =======================================================
    -- 7. INSERTAR EN TABLAS DE DETALLE SEGÚN TIPO
    -- =======================================================
    
    -- 7.1 PAGO DE SERVICIOS
    IF @TipoOperacion = 'Servicios' AND @ServicioID IS NOT NULL
    BEGIN
        INSERT INTO PagosServicios (
            OperacionID,
            ServicioID,
            Referencia,
            Periodo,
            Convenio
        )
        VALUES (
            @OperacionID,
            @ServicioID,
            @ReferenciaServicio,
            @Periodo,
            @Convenio
        );
    END
    
    -- 7.2 DONACIÓN
    IF @TipoOperacion IN ('Donaciones', 'Donacion') AND @OrganizacionNombre IS NOT NULL
    BEGIN
        INSERT INTO Donaciones (
            OperacionID,
            OrganizacionNombre,
            CLABE,
            CausaSocial,
            CertificadoDonacion
        )
        VALUES (
            @OperacionID,
            @OrganizacionNombre,
            @CLABE,
            @CausaSocial,
            @CertificadoDonacion
        );
    END
    
    -- 7.3 RETIRO SIN TARJETA
    IF @TipoOperacion = 'RetiroSinTarjeta' AND @ClaveRetiro IS NOT NULL
    BEGIN
        INSERT INTO RetirosSinTarjeta (
            OperacionID,
            CuentaID,
            ClaveRetiro,
            ClaveGenerada,
            IntentosFallidos
        )
        VALUES (
            @OperacionID,
            @CuentaOrigenID,
            @ClaveRetiro,
            GETDATE(),
            0
        );
    END
    
    -- 7.4 DEPÓSITO EN EFECTIVO SIN TARJETA
    IF @TipoOperacion = 'DepositoEfectivo' AND @CuentaDestinoEfectivoID IS NOT NULL
    BEGIN
        INSERT INTO DepositosEfectivo (
            OperacionID,
            CuentaDestinoID,
            EsDonacion,
            EsPagoServicio,
            ReferenciaCliente
        )
        VALUES (
            @OperacionID,
            @CuentaDestinoEfectivoID,
            @EsDonacionEfectivo,
            @EsPagoServicioEfectivo,
            @ReferenciaCliente
        );
    END

    -- =======================================================
    -- 8. RETORNAR ID DE OPERACIÓN
    -- =======================================================
    SELECT @OperacionID AS OperacionID;

END;
GO
USE [master]
GO
ALTER DATABASE [Banco2] SET  READ_WRITE 
GO
