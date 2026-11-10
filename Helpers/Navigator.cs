using BankIA.Forms;

namespace BankIA.Helpers;

internal static class Navigator
{
    public static FrmBienvenida? FormRaiz { get; set; }

    // Flag para no ejecutar FormClosed handlers durante cierre de sesion masivo
    public static bool TerminandoSesion { get; private set; } = false;

    /// <summary>
    /// Cierra la sesion, cierra todos los formularios excepto FrmBienvenida
    /// y vuelve a mostrarla.
    /// </summary>
    public static void TerminarSesion()
    {
        TerminandoSesion = true;
        SessionManager.CerrarSesion();

        foreach (Form f in Application.OpenForms.Cast<Form>().ToList())
        {
            if (f is not FrmBienvenida)
                f.Close();
        }

        TerminandoSesion = false;
        FormRaiz?.Show();
    }

    /// <summary>
    /// Navega de un formulario al siguiente:
    /// oculta 'origen', muestra 'destino'.
    /// Cuando 'destino' se cierre, 'origen' vuelve a aparecer
    /// (a menos que se este terminando sesion).
    /// </summary>
    public static void IrA(Form origen, Form destino)
    {
        origen.Hide();
        destino.FormClosed += (s, e) =>
        {
            if (!TerminandoSesion)
                origen.Show();
        };
        destino.Show();
    }

    /// <summary>
    /// Cierra toda la cadena de formularios de operación (Retiro/Depósito/
    /// Transferencia → Donación → Éxito) y regresa al Menú Principal,
    /// que siempre queda oculto en segundo plano durante las operaciones.
    /// </summary>
    public static void VolverAlMenu()
    {
        TerminandoSesion = true;

        var forms = Application.OpenForms.Cast<Form>().ToList();
        var menu  = forms.FirstOrDefault(f => f is FrmMenu);

        // Cerrar todos los formularios de operación (los intermedios)
        foreach (var f in forms)
            if (f is not FrmMenu && f is not FrmBienvenida)
                f.Close();

        TerminandoSesion = false;
        menu?.Show();
    }
}
