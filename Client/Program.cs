using NewLife.Log;
using NewLife.Model;
using Stardust;

namespace Client;

static class Program
{
    [STAThread]
    static void Main()
    {
        XTrace.UseWinForm();

        // 注册星尘
        var services = ObjectContainer.Current;
        services.AddStardust();

        ApplicationConfiguration.Initialize();
        Application.Run(new FrmMain());
    }
}