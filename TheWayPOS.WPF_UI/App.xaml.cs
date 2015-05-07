using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpf.Core;
using System.Windows;

namespace TheWayPOS.WPF_UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Start(() => base.OnStartup(e), this);
        }

        public static void Start(Action baseStart, Application application)
        {
            ThemeManager.ApplicationThemeName = Theme.HybridApp.Name;
        }
    }
}
