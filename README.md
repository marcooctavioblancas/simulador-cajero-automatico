# BankIA: simulador de cajero automático

Aplicación de escritorio en C# que simula un cajero automático: el usuario inserta una tarjeta, escribe su NIP y realiza operaciones contra bases de datos SQL Server. Incluye tarjetas de un segundo banco, con operaciones interbancarias.

## Qué hace

- Inserción de tarjeta y validación de NIP. El NIP se guarda como hash, no en texto plano, y la tarjeta se bloquea tras superar el máximo de intentos fallidos.
- Cambio de NIP, consulta de saldo e historial de movimientos.
- Retiro (también sin tarjeta), depósito y transferencia.
- Pago de servicios y recargas, y donaciones.
- Control del inventario de billetes del cajero.
- Tarjetas de un segundo banco (`Banco2`), que vive en otra base de datos del mismo servidor, con una comisión interbancaria de $15 MXN por operación.
- Comprobantes en PDF.

## Tecnologías

- C# con Windows Forms y .NET 8
- SQL Server (`Microsoft.Data.SqlClient`), con consultas entre dos bases de datos
- QuestPDF para generar los comprobantes

## Estructura

```
├── Config/     Cadena de conexión y parámetros (comisión, intentos máximos)
├── Models/     Clases del dominio (Cliente, Cuenta, Tarjeta, Movimiento, Servicio...)
├── DAL/        Acceso a datos: consultas SQL por entidad
├── Forms/      Pantallas del cajero
├── Controls/   Controles personalizados
├── Helpers/    Navegación entre pantallas, sesión, seguridad y comprobantes
├── Resources/  Tipografías
└── database/   Scripts de las bases de datos
```

El código separa la interfaz (`Forms`), el acceso a datos (`DAL`) y las clases del dominio (`Models`). Casi todas las consultas viven en `DAL`; dos pantallas (`FrmInsertarTarjeta` y `FrmRetiroSinTarjeta`) todavía llevan una consulta propia, pendiente de mover.

## Cómo correrlo

1. Instala Visual Studio 2022 o superior con la carga de trabajo de escritorio de .NET, y SQL Server (basta la edición Express).
2. Crea las bases y carga los datos de prueba ejecutando, en este orden, `database/bankia.sql`, `database/banco2.sql` y `database/seed.sql` (por ejemplo desde SQL Server Management Studio). Los dos primeros crean las bases con `CREATE DATABASE`; si ya existen, bórralas antes de volver a correrlos.
3. En `Config/AppConfig.cs`, cambia `Server=localhost` por el nombre de tu instancia, por ejemplo `localhost\SQLEXPRESS`.
4. Abre `BankIA.sln`, restaura los paquetes NuGet y presiona F5.

## Datos de prueba

`database/seed.sql` carga datos inventados (ninguna persona, tarjeta ni cuenta es real). Tarjetas para probar:

| Tarjeta | NIP | Banco | Saldo | Sirve para |
|---|---|---|---|---|
| `1111 1111 1111 1111` | 1234 | BankIA | $15,000.00 | Flujo normal, todas las operaciones |
| `2222 2222 2222 2222` | 4321 | BankIA | $5,000.00 | Segunda cuenta, destino de transferencias |
| `3333 3333 3333 3333` | 1234 | BankIA | $1,000.00 | Tarjeta bloqueada |
| `4444 4444 4444 4444` | 1234 | Banco2 | $12,000.00 | Operación interbancaria (solo retiro, con comisión de $15) |
| `5555 5555 5555 5555` | 4321 | Banco2 | $500.00 | Interbancario con saldo bajo |

Retiro sin tarjeta: cuenta `1000000001` con NIP 1234, o `1000000002` con NIP 4321.

## Tipografías

Exo 2 y Orbitron, distribuidas bajo la licencia SIL Open Font License.

## Autor

Marco Octavio Blancas Guzmán
