using BankIA.Forms;
using BankIA.Helpers;

namespace BankIA;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var bienvenida = new FrmBienvenida();
        Navigator.FormRaiz = bienvenida;
        Application.Run(bienvenida);
    }
}
