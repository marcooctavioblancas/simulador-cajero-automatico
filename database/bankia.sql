USE [master]
GO
/****** Objeto: Database [BankIA] Fecha de script: 10/11/2026 04:20:02 p. m. ******/
CREATE DATABASE [BankIA]
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BankIA].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BankIA] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BankIA] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BankIA] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BankIA] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BankIA] SET ARITHABORT OFF 
GO
ALTER DATABASE [BankIA] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BankIA] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BankIA] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BankIA] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BankIA] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BankIA] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BankIA] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BankIA] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BankIA] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BankIA] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BankIA] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BankIA] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BankIA] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BankIA] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BankIA] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BankIA] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BankIA] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BankIA] SET RECOVERY FULL 
GO
ALTER DATABASE [BankIA] SET  MULTI_USER 
GO
ALTER DATABASE [BankIA] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BankIA] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BankIA] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BankIA] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BankIA] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BankIA] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'BankIA', N'ON'
GO
ALTER DATABASE [BankIA] SET QUERY_STORE = ON
GO
ALTER DATABASE [BankIA] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [BankIA]
GO
/****** Objeto: Table [dbo].[Bancos] Fecha de script: 10/11/2026 04:20:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bancos](
	[BancoID] [int] IDENTITY(1,1) NOT NULL,
	[CodigoBanco] [char](10) NOT NULL,
	[NombreBanco] [nvarchar](100) NOT NULL,
	[Pais] [nvarchar](50) NOT NULL,
	[Activo] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BancoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[CodigoBanco] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Cajeros] Fecha de script: 10/11/2026 04:20:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cajeros](
	[CajeroID] [int] IDENTITY(1,1) NOT NULL,
	[BancoID] [int] NOT NULL,
	[CodigoCajero] [nvarchar](20) NOT NULL,
	[Ubicacion] [nvarchar](200) NULL,
	[IPAddress] [varchar](45) NULL,
	[Activo] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CajeroID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[CodigoCajero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[CambiosNIP] Fecha de script: 10/11/2026 04:20:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CambiosNIP](
	[CambioID] [int] IDENTITY(1,1) NOT NULL,
	[TarjetaID] [int] NOT NULL,
	[CajeroID] [int] NULL,
	[Exitoso] [bit] NOT NULL,
	[FechaCambio] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CambioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Clientes] Fecha de script: 10/11/2026 04:20:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clientes](
	[ClienteID] [int] IDENTITY(1,1) NOT NULL,
	[NombreCompleto] [nvarchar](150) NOT NULL,
	[Identificacion] [nvarchar](20) NOT NULL,
	[Telefono] [nvarchar](15) NULL,
	[Direccion] [nvarchar](300) NULL,
	[Email] [nvarchar](100) NULL,
	[FechaAlta] [datetime] NOT NULL,
	[Activo] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ClienteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Identificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Cuentas] Fecha de script: 10/11/2026 04:20:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cuentas](
	[CuentaID] [int] IDENTITY(1,1) NOT NULL,
	[ClienteID] [int] NOT NULL,
	[BancoID] [int] NOT NULL,
	[NumeroCuenta] [nvarchar](20) NOT NULL,
	[TipoCuenta] [nvarchar](20) NOT NULL,
	[SaldoDisponible] [decimal](18, 2) NOT NULL,
	[SaldoContable] [decimal](18, 2) NOT NULL,
	[Estado] [nvarchar](20) NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[Activo] [bit] NOT NULL,
	[LimiteRetiroDiario] [decimal](18, 2) NOT NULL,
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
/****** Objeto: Table [dbo].[InventarioBilletes] Fecha de script: 10/11/2026 04:20:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InventarioBilletes](
	[CajeroID] [int] NOT NULL,
	[Denominacion] [int] NOT NULL,
	[Cantidad] [int] NOT NULL,
 CONSTRAINT [PK_InventarioBilletes] PRIMARY KEY CLUSTERED 
(
	[CajeroID] ASC,
	[Denominacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[MensajesInterbancarios] Fecha de script: 10/11/2026 04:20:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MensajesInterbancarios](
	[MensajeID] [int] IDENTITY(1,1) NOT NULL,
	[BancoOrigenID] [int] NOT NULL,
	[BancoDestinoID] [int] NOT NULL,
	[TipoMensaje] [nvarchar](50) NULL,
	[Monto] [decimal](18, 2) NULL,
	[CuentaOrigen] [nvarchar](20) NULL,
	[CuentaDestino] [nvarchar](20) NULL,
	[Estado] [nvarchar](20) NULL,
	[FechaEnvio] [datetime] NULL,
	[FechaProcesado] [datetime] NULL,
	[Respuesta] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[MensajeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[MontosRecarga] Fecha de script: 10/11/2026 04:20:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MontosRecarga](
	[MontoID] [int] IDENTITY(1,1) NOT NULL,
	[ServicioID] [int] NOT NULL,
	[Monto] [decimal](18, 2) NOT NULL,
	[Descripcion] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[MontoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Movimientos] Fecha de script: 10/11/2026 04:20:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Movimientos](
	[MovimientoID] [int] IDENTITY(1,1) NOT NULL,
	[CuentaID] [int] NOT NULL,
	[TarjetaID] [int] NULL,
	[CajeroID] [int] NULL,
	[TipoMovimiento] [nvarchar](30) NOT NULL,
	[Monto] [decimal](18, 2) NULL,
	[SaldoAnterior] [decimal](18, 2) NOT NULL,
	[SaldoPosterior] [decimal](18, 2) NOT NULL,
	[CuentaDestinoID] [int] NULL,
	[Referencia] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](200) NULL,
	[Estado] [nvarchar](20) NOT NULL,
	[EsInterbancario] [bit] NOT NULL,
	[BancoOrigenID] [int] NULL,
	[BancoDestinoID] [int] NULL,
	[FechaMovimiento] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MovimientoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Movimientos_Referencia] UNIQUE NONCLUSTERED 
(
	[Referencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Servicios] Fecha de script: 10/11/2026 04:20:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Servicios](
	[ServicioID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](80) NOT NULL,
	[Icono] [nvarchar](10) NOT NULL,
	[TipoServicio] [nvarchar](20) NOT NULL,
	[Instrucciones] [nvarchar](200) NULL,
	[Activo] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ServicioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Sesiones] Fecha de script: 10/11/2026 04:20:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sesiones](
	[SesionID] [int] IDENTITY(1,1) NOT NULL,
	[TarjetaID] [int] NOT NULL,
	[CajeroID] [int] NULL,
	[FechaInicio] [datetime] NOT NULL,
	[FechaFin] [datetime] NULL,
	[Estado] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SesionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Tarjetas] Fecha de script: 10/11/2026 04:20:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tarjetas](
	[TarjetaID] [int] IDENTITY(1,1) NOT NULL,
	[CuentaID] [int] NOT NULL,
	[NumeroTarjeta] [nvarchar](16) NOT NULL,
	[NIPHash] [varchar](64) NOT NULL,
	[FechaExpiracion] [date] NOT NULL,
	[TipoTarjeta] [nvarchar](20) NOT NULL,
	[IntentosFallidos] [int] NOT NULL,
	[Bloqueada] [bit] NOT NULL,
	[FechaBloqueo] [datetime] NULL,
	[FechaAlta] [datetime] NOT NULL,
	[Activa] [bit] NOT NULL,
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
ALTER TABLE [dbo].[Bancos] ADD  DEFAULT ('México') FOR [Pais]
GO
ALTER TABLE [dbo].[Bancos] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Cajeros] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[CambiosNIP] ADD  DEFAULT ((1)) FOR [Exitoso]
GO
ALTER TABLE [dbo].[CambiosNIP] ADD  DEFAULT (getdate()) FOR [FechaCambio]
GO
ALTER TABLE [dbo].[Clientes] ADD  DEFAULT (getdate()) FOR [FechaAlta]
GO
ALTER TABLE [dbo].[Clientes] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Cuentas] ADD  DEFAULT ('Debito') FOR [TipoCuenta]
GO
ALTER TABLE [dbo].[Cuentas] ADD  DEFAULT ((0)) FOR [SaldoDisponible]
GO
ALTER TABLE [dbo].[Cuentas] ADD  DEFAULT ((0)) FOR [SaldoContable]
GO
ALTER TABLE [dbo].[Cuentas] ADD  DEFAULT ('Activa') FOR [Estado]
GO
ALTER TABLE [dbo].[Cuentas] ADD  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Cuentas] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Cuentas] ADD  DEFAULT ((10000)) FOR [LimiteRetiroDiario]
GO
ALTER TABLE [dbo].[InventarioBilletes] ADD  DEFAULT ((0)) FOR [Cantidad]
GO
ALTER TABLE [dbo].[MensajesInterbancarios] ADD  DEFAULT ('Pendiente') FOR [Estado]
GO
ALTER TABLE [dbo].[MensajesInterbancarios] ADD  DEFAULT (getdate()) FOR [FechaEnvio]
GO
ALTER TABLE [dbo].[Movimientos] ADD  DEFAULT ('Exitoso') FOR [Estado]
GO
ALTER TABLE [dbo].[Movimientos] ADD  DEFAULT ((0)) FOR [EsInterbancario]
GO
ALTER TABLE [dbo].[Movimientos] ADD  DEFAULT (getdate()) FOR [FechaMovimiento]
GO
ALTER TABLE [dbo].[Servicios] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Sesiones] ADD  DEFAULT (getdate()) FOR [FechaInicio]
GO
ALTER TABLE [dbo].[Sesiones] ADD  DEFAULT ('Activa') FOR [Estado]
GO
ALTER TABLE [dbo].[Tarjetas] ADD  DEFAULT ('Debito') FOR [TipoTarjeta]
GO
ALTER TABLE [dbo].[Tarjetas] ADD  DEFAULT ((0)) FOR [IntentosFallidos]
GO
ALTER TABLE [dbo].[Tarjetas] ADD  DEFAULT ((0)) FOR [Bloqueada]
GO
ALTER TABLE [dbo].[Tarjetas] ADD  DEFAULT (getdate()) FOR [FechaAlta]
GO
ALTER TABLE [dbo].[Tarjetas] ADD  DEFAULT ((1)) FOR [Activa]
GO
ALTER TABLE [dbo].[Cajeros]  WITH CHECK ADD FOREIGN KEY([BancoID])
REFERENCES [dbo].[Bancos] ([BancoID])
GO
ALTER TABLE [dbo].[CambiosNIP]  WITH CHECK ADD FOREIGN KEY([CajeroID])
REFERENCES [dbo].[Cajeros] ([CajeroID])
GO
ALTER TABLE [dbo].[CambiosNIP]  WITH CHECK ADD FOREIGN KEY([TarjetaID])
REFERENCES [dbo].[Tarjetas] ([TarjetaID])
GO
ALTER TABLE [dbo].[Cuentas]  WITH CHECK ADD FOREIGN KEY([BancoID])
REFERENCES [dbo].[Bancos] ([BancoID])
GO
ALTER TABLE [dbo].[Cuentas]  WITH CHECK ADD FOREIGN KEY([ClienteID])
REFERENCES [dbo].[Clientes] ([ClienteID])
GO
ALTER TABLE [dbo].[InventarioBilletes]  WITH CHECK ADD FOREIGN KEY([CajeroID])
REFERENCES [dbo].[Cajeros] ([CajeroID])
GO
ALTER TABLE [dbo].[MensajesInterbancarios]  WITH CHECK ADD FOREIGN KEY([BancoOrigenID])
REFERENCES [dbo].[Bancos] ([BancoID])
GO
ALTER TABLE [dbo].[MensajesInterbancarios]  WITH CHECK ADD FOREIGN KEY([BancoDestinoID])
REFERENCES [dbo].[Bancos] ([BancoID])
GO
ALTER TABLE [dbo].[MontosRecarga]  WITH CHECK ADD FOREIGN KEY([ServicioID])
REFERENCES [dbo].[Servicios] ([ServicioID])
GO
ALTER TABLE [dbo].[Movimientos]  WITH CHECK ADD FOREIGN KEY([BancoOrigenID])
REFERENCES [dbo].[Bancos] ([BancoID])
GO
ALTER TABLE [dbo].[Movimientos]  WITH CHECK ADD FOREIGN KEY([BancoDestinoID])
REFERENCES [dbo].[Bancos] ([BancoID])
GO
ALTER TABLE [dbo].[Movimientos]  WITH CHECK ADD FOREIGN KEY([CajeroID])
REFERENCES [dbo].[Cajeros] ([CajeroID])
GO
ALTER TABLE [dbo].[Movimientos]  WITH CHECK ADD FOREIGN KEY([CuentaID])
REFERENCES [dbo].[Cuentas] ([CuentaID])
GO
ALTER TABLE [dbo].[Movimientos]  WITH CHECK ADD FOREIGN KEY([CuentaDestinoID])
REFERENCES [dbo].[Cuentas] ([CuentaID])
GO
ALTER TABLE [dbo].[Movimientos]  WITH CHECK ADD FOREIGN KEY([TarjetaID])
REFERENCES [dbo].[Tarjetas] ([TarjetaID])
GO
ALTER TABLE [dbo].[Sesiones]  WITH CHECK ADD FOREIGN KEY([CajeroID])
REFERENCES [dbo].[Cajeros] ([CajeroID])
GO
ALTER TABLE [dbo].[Sesiones]  WITH CHECK ADD FOREIGN KEY([TarjetaID])
REFERENCES [dbo].[Tarjetas] ([TarjetaID])
GO
ALTER TABLE [dbo].[Tarjetas]  WITH CHECK ADD FOREIGN KEY([CuentaID])
REFERENCES [dbo].[Cuentas] ([CuentaID])
GO
USE [master]
GO
ALTER DATABASE [BankIA] SET  READ_WRITE 
GO
