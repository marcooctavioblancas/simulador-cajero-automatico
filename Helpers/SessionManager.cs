using BankIA.Models;
using System.Drawing;

namespace BankIA.Helpers;

internal static class SessionManager
{
    public static Tarjeta?  TarjetaActual { get; private set; }
    public static Cuenta?   CuentaActual  { get; private set; }
    public static Cliente?  ClienteActual { get; private set; }
    public static int       SesionID      { get; private set; }
    public static int       CajeroID      { get; set; } = 1; // ATM-001 por defecto

    public static bool HaySesionActiva => TarjetaActual != null;

    // ── Interoperabilidad ─────────────────────────────────────────────────
    public static bool   EsClienteExterno       { get; set; } = false;
    public static bool   BancoExternoDisponible { get; set; } = true;
    public static string NombreBancoExterno     { get; set; } = string.Empty;
    public static int    IntentosNIPExterno     { get; set; } = 0;

    // Color de acento del marco ATM — cyan para BankIA, púrpura para externos
    public static Color ColorAcento { get; set; } = Color.FromArgb(0, 212, 255);

    // Datos de la ultima operacion (para pantalla de exito)
    public static string  UltimaReferencia     { get; set; } = string.Empty;
    public static string  UltimaTipoOperacion  { get; set; } = string.Empty;
    public static decimal UltimoMonto          { get; set; }
    public static decimal    SaldoTrasOperacion  { get; set; }
    public static Movimiento? UltimoMovimiento   { get; set; }

    public static void IniciarSesion(Tarjeta tarjeta, Cuenta cuenta, Cliente cliente, int sesionID)
    {
        TarjetaActual = tarjeta;
        CuentaActual  = cuenta;
        ClienteActual = cliente;
        SesionID      = sesionID;
    }

    public static void RefrescarCuenta(Cuenta cuenta)
    {
        CuentaActual = cuenta;
    }

    public static void CerrarSesion()
    {
        TarjetaActual       = null;
        CuentaActual        = null;
        ClienteActual       = null;
        SesionID            = 0;
        UltimaReferencia    = string.Empty;
        UltimaTipoOperacion = string.Empty;
        UltimoMonto            = 0;
        SaldoTrasOperacion     = 0;
        UltimoMovimiento       = null;
        EsClienteExterno       = false;
        BancoExternoDisponible = true;
        NombreBancoExterno     = string.Empty;
        IntentosNIPExterno     = 0;
        ColorAcento            = Color.FromArgb(0, 212, 255); // reset a cyan
    }
}
