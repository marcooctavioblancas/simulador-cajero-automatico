/* =========================================================================
   BankIA - DATOS DE PRUEBA (seed)

   TODOS LOS DATOS DE ESTE ARCHIVO SON INVENTADOS. No corresponden a ninguna
   persona, tarjeta ni cuenta reales. Los numeros de tarjeta son patrones de
   digitos repetidos que NO pasan la validacion de Luhn, por lo que no pueden
   ser tarjetas reales.

   ORDEN DE EJECUCION:  1) bankia.sql   2) banco2.sql   3) seed.sql (este)

   Es re-ejecutable: cada tabla solo se llena si esta vacia.
   Este archivo es ASCII puro (sin acentos) para que se lea igual en SSMS,
   sqlcmd o cualquier editor, con o sin BOM.

   TARJETAS DE PRUEBA
   Numero              NIP   Banco   Cuenta       Saldo       Uso
   ------------------  ----  ------  -----------  ----------  ---------------------------
   1111 1111 1111 1111 1234  BankIA  1000000001   $15,000.00  Flujo NORMAL (todas las operaciones)
   2222 2222 2222 2222 4321  BankIA  1000000002    $5,000.00  Segunda cuenta (destino de transferencias)
   3333 3333 3333 3333 1234  BankIA  1000000003    $1,000.00  Tarjeta BLOQUEADA (prueba de bloqueo)
   4444 4444 4444 4444 1234  Banco2  2000000001   $12,000.00  Flujo INTERBANCARIO (solo retiro, +$15)
   5555 5555 5555 5555 4321  Banco2  2000000002      $500.00  Interbancario con saldo bajo

   Retiro sin tarjeta: cuenta 1000000001 con NIP 1234 (o 1000000002 con NIP 4321).
   NIPs: BankIA guarda SHA-256 en hex mayusculas (varchar(64)); Banco2 guarda
   SHA-512 (64 bytes) en varbinary(64). Un NIP de 4 digitos se recupera por fuerza
   bruta en segundos: estos NIPs son de prueba, jamas uses uno real.
   ========================================================================= */

USE [BankIA]
GO

/* ---- Bancos (el codigo usa BancoID 1 = BankIA y 2 = Banco2). Pais usa su DEFAULT. ---- */
IF NOT EXISTS (SELECT 1 FROM [dbo].[Bancos])
BEGIN
    SET IDENTITY_INSERT [dbo].[Bancos] ON;
    INSERT INTO [dbo].[Bancos] ([BancoID], [CodigoBanco], [NombreBanco], [Activo])
    VALUES (1, 'BANKIA', N'BankIA', 1),
           (2, 'BANCO2', N'Banco2', 1);
    SET IDENTITY_INSERT [dbo].[Bancos] OFF;
END
GO

/* ---- Cajeros (SessionManager.CajeroID = 1 por defecto, AppConfig.ATMCodigo = ATM-001) ---- */
IF NOT EXISTS (SELECT 1 FROM [dbo].[Cajeros])
BEGIN
    SET IDENTITY_INSERT [dbo].[Cajeros] ON;
    INSERT INTO [dbo].[Cajeros] ([CajeroID], [BancoID], [CodigoCajero], [Ubicacion], [IPAddress], [Activo])
    VALUES (1, 1, N'ATM-001', N'Cajero de prueba (ubicacion ficticia)', '127.0.0.1', 1);
    SET IDENTITY_INSERT [dbo].[Cajeros] OFF;
END
GO

/* ---- Inventario de billetes del cajero 1 (BilletesDAL: 1000, 500, 200, 100, 50) ---- */
IF NOT EXISTS (SELECT 1 FROM [dbo].[InventarioBilletes])
BEGIN
    INSERT INTO [dbo].[InventarioBilletes] ([CajeroID], [Denominacion], [Cantidad])
    VALUES (1, 1000,  20),
           (1,  500,  40),
           (1,  200,  60),
           (1,  100,  80),
           (1,   50, 100);
END
GO

/* ---- Clientes (FechaAlta y Activo usan su DEFAULT; datos de contacto genericos) ---- */
IF NOT EXISTS (SELECT 1 FROM [dbo].[Clientes])
BEGIN
    SET IDENTITY_INSERT [dbo].[Clientes] ON;
    INSERT INTO [dbo].[Clientes] ([ClienteID], [NombreCompleto], [Identificacion], [Telefono], [Direccion], [Email])
    VALUES (1, N'Cliente Prueba Uno',  N'PRUEBA-0001', N'0000000001', N'Calle Ficticia 1, Ciudad de Prueba', N'cliente.uno@example.com'),
           (2, N'Cliente Prueba Dos',  N'PRUEBA-0002', N'0000000002', N'Calle Ficticia 2, Ciudad de Prueba', N'cliente.dos@example.com'),
           (3, N'Cliente Prueba Tres', N'PRUEBA-0003', N'0000000003', N'Calle Ficticia 3, Ciudad de Prueba', N'cliente.tres@example.com');
    SET IDENTITY_INSERT [dbo].[Clientes] OFF;
END
GO

/* ---- Cuentas (FechaCreacion usa su DEFAULT) ---- */
IF NOT EXISTS (SELECT 1 FROM [dbo].[Cuentas])
BEGIN
    SET IDENTITY_INSERT [dbo].[Cuentas] ON;
    INSERT INTO [dbo].[Cuentas] ([CuentaID], [ClienteID], [BancoID], [NumeroCuenta], [TipoCuenta], [SaldoDisponible], [SaldoContable], [Estado], [Activo], [LimiteRetiroDiario])
    VALUES (1, 1, 1, N'1000000001', N'Debito', 15000.00, 15000.00, N'Activa', 1, 10000.00),
           (2, 2, 1, N'1000000002', N'Debito',  5000.00,  5000.00, N'Activa', 1, 10000.00),
           (3, 3, 1, N'1000000003', N'Debito',  1000.00,  1000.00, N'Activa', 1, 10000.00);
    SET IDENTITY_INSERT [dbo].[Cuentas] OFF;
END
GO

/* ---- Tarjetas (NIPHash = SHA-256 hex MAYUSCULAS del NIP; FechaAlta usa su DEFAULT) ---- */
IF NOT EXISTS (SELECT 1 FROM [dbo].[Tarjetas])
BEGIN
    SET IDENTITY_INSERT [dbo].[Tarjetas] ON;
    INSERT INTO [dbo].[Tarjetas] ([TarjetaID], [CuentaID], [NumeroTarjeta], [NIPHash], [FechaExpiracion], [TipoTarjeta], [IntentosFallidos], [Bloqueada], [FechaBloqueo], [Activa])
    VALUES (1, 1, N'1111111111111111', '03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4', '20311231', N'Debito', 0, 0, NULL, 1),
           (2, 2, N'2222222222222222', 'FE2592B42A727E977F055947385B709CC82B16B9A87F88C6ABF3900D65D0CDC3', '20311231', N'Debito', 0, 0, NULL, 1),
           (3, 3, N'3333333333333333', '03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4', '20311231', N'Debito', 3, 1, GETDATE(), 1);
    SET IDENTITY_INSERT [dbo].[Tarjetas] OFF;
END
GO

/* ---- Servicios (TipoServicio: 'Servicio' o 'Recarga', ver Models/Servicio.cs) ----
   Los iconos son emojis escritos con NCHAR() para mantener el archivo en ASCII. */
IF NOT EXISTS (SELECT 1 FROM [dbo].[Servicios])
BEGIN
    SET IDENTITY_INSERT [dbo].[Servicios] ON;
    INSERT INTO [dbo].[Servicios] ([ServicioID], [Nombre], [Icono], [TipoServicio], [Instrucciones], [Activo])
    VALUES (1, N'Servicio de Luz', NCHAR(55357) + NCHAR(56481), N'Servicio', N'Ingrese la referencia de su recibo y el monto a pagar.', 1),  -- U+1F4A1
           (2, N'Servicio de Agua', NCHAR(55357) + NCHAR(56487), N'Servicio', N'Ingrese la referencia de su recibo y el monto a pagar.', 1),  -- U+1F4A7
           (3, N'Servicio de Gas', NCHAR(55357) + NCHAR(56613), N'Servicio', N'Ingrese la referencia de su recibo y el monto a pagar.', 1),  -- U+1F525
           (4, N'Internet Hogar', NCHAR(55356) + NCHAR(57104), N'Servicio', N'Ingrese la referencia de su contrato y el monto a pagar.', 1),  -- U+1F310
           (5, N'Recarga Operador A', NCHAR(55357) + NCHAR(56561), N'Recarga', N'Seleccione el monto e ingrese el numero de telefono a 10 digitos.', 1),  -- U+1F4F1
           (6, N'Recarga Operador B', NCHAR(55357) + NCHAR(56561), N'Recarga', N'Seleccione el monto e ingrese el numero de telefono a 10 digitos.', 1),  -- U+1F4F1
           (7, N'Recarga Operador C', NCHAR(55357) + NCHAR(56561), N'Recarga', N'Seleccione el monto e ingrese el numero de telefono a 10 digitos.', 1);  -- U+1F4F1
    SET IDENTITY_INSERT [dbo].[Servicios] OFF;
END
GO

/* ---- MontosRecarga (solo para los servicios de tipo Recarga: 5, 6 y 7). MontoID es identity automatico. ---- */
IF NOT EXISTS (SELECT 1 FROM [dbo].[MontosRecarga])
BEGIN
    INSERT INTO [dbo].[MontosRecarga] ([ServicioID], [Monto], [Descripcion])
    VALUES (5, 20.00, N'Recarga de saldo'),
           (5, 50.00, N'Recarga de saldo'),
           (5, 100.00, N'Recarga de saldo'),
           (5, 200.00, N'Recarga de saldo'),
           (5, 500.00, N'Recarga de saldo'),
           (6, 20.00, N'Recarga de saldo'),
           (6, 50.00, N'Recarga de saldo'),
           (6, 100.00, N'Recarga de saldo'),
           (6, 200.00, N'Recarga de saldo'),
           (6, 500.00, N'Recarga de saldo'),
           (7, 20.00, N'Recarga de saldo'),
           (7, 50.00, N'Recarga de saldo'),
           (7, 100.00, N'Recarga de saldo'),
           (7, 200.00, N'Recarga de saldo'),
           (7, 500.00, N'Recarga de saldo');
END
GO

USE [Banco2]
GO

/* IDs 1001+ a proposito: FrmSaldo/FrmHistorial buscan la cuenta por CuentaID en la base de
   BankIA tambien para tarjetas de Banco2; si un ID de Banco2 coincidiera con uno de BankIA,
   el cliente externo veria datos del cliente local. */

/* ---- Clientes de Banco2 (Nombre + ApellidoP + ApellidoM; Activo usa su DEFAULT) ---- */
IF NOT EXISTS (SELECT 1 FROM [dbo].[Clientes])
BEGIN
    SET IDENTITY_INSERT [dbo].[Clientes] ON;
    INSERT INTO [dbo].[Clientes] ([ClienteID], [Nombre], [ApellidoP], [ApellidoM])
    VALUES (1001, N'Cliente', N'Prueba', N'Cuatro'),
           (1002, N'Cliente', N'Prueba', N'Cinco');
    SET IDENTITY_INSERT [dbo].[Clientes] OFF;
END
GO

/* ---- Cuentas de Banco2 (FechaApertura y Activa usan su DEFAULT) ---- */
IF NOT EXISTS (SELECT 1 FROM [dbo].[Cuentas])
BEGIN
    SET IDENTITY_INSERT [dbo].[Cuentas] ON;
    INSERT INTO [dbo].[Cuentas] ([CuentaID], [ClienteID], [NumeroCuenta], [Saldo])
    VALUES (1001, 1001, '2000000001', 12000.00),
           (1002, 1002, '2000000002',   500.00);
    SET IDENTITY_INSERT [dbo].[Cuentas] OFF;
END
GO

/* ---- Tarjetas de Banco2 (NIPHash = SHA-512 en bytes, varbinary(64); el codigo lo lee con CONVERT(VARCHAR(128), NIPHash, 2)) ---- */
IF NOT EXISTS (SELECT 1 FROM [dbo].[Tarjetas])
BEGIN
    SET IDENTITY_INSERT [dbo].[Tarjetas] ON;
    INSERT INTO [dbo].[Tarjetas] ([TarjetaID], [CuentaID], [NumeroTarjeta], [Terminacion], [FechaExpiracion], [NIPHash], [Bloqueada])
    VALUES (1001, 1001, '4444444444444444', '4444', '20311231',
            0xD404559F602EAB6FD602AC7680DACBFAADD13630335E951F097AF3900E9DE176B6DB28512F2E000B9D04FBA5133E8B1C6E8DF59DB3A8AB9D60BE4B97CC9E81DB, 0),
           (1002, 1002, '5555555555555555', '5555', '20311231',
            0x7E2FEAC95DCD7D1DF803345E197369AF4B156E4E7A95FCB2955BDBBB3A11AFD8BB9D35931BF15511370B18143E38B01B903F55C5ECBDED4AF99934602FCDF38C, 0);
    SET IDENTITY_INSERT [dbo].[Tarjetas] OFF;
END
GO
