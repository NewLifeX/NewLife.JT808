using NewLife.Log;

namespace Client;

static class Program
{
    [STAThread]
    static void Main()
    {
        XTrace.UseWinForm();

        ApplicationConfiguration.Initialize();
        Application.Run(new FrmMain());
    }
}