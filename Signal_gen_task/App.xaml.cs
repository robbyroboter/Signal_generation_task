using System.Configuration;
using System.Data;
using System.Windows;
using SQLitePCL;

namespace Signal_gen_task
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Batteries.Init();

            base.OnStartup(e);
        }
    }

}
