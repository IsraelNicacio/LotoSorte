using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace FormApostas
{
    static class Program
    {
        #region Enums

        public enum ExitCodes
        {
            Sucess = 0,
            Error = 1,
        }

        #endregion Enums

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            #region Testa se só existe um processo do sistema

            bool createdNew;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, "LotoApostas", out createdNew);
            if (!createdNew)
                return;

            #endregion Testa se só existe um processo do sistema

            // Handle the ApplicationExit event to know when the application is exiting.
            Application.ApplicationExit += OnApplicationExit;

            ConfigureCefSettings();

            LoadApp();

            //Fechou aplicação sem consultar com sucesso
            Environment.Exit((int) ExitCodes.Error);
        }
        
        private static void ConfigureCefSettings()
        {
            // Handle Assembly Resolve
            AppDomain.CurrentDomain.AssemblyResolve += Resolver;

            Cef.EnableHighDPISupport();

            var settings = new CefSettings()
            {
                //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache")
            };

            settings.CefCommandLineArgs.Add("enable-media-stream");
            settings.CefCommandLineArgs.Add("use-fake-ui-for-media-stream");
            settings.CefCommandLineArgs.Add("enable-usermedia-screen-capturing");

            //Perform dependency check to make sure all relevant resources are in our output directory.
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void LoadApp()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLotoApostas());
        }

        private static System.Reflection.Assembly Resolver(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("CefSharp"))
            {
                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                string archSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                       Environment.Is64BitProcess ? "x64" : "x86",
                                                       assemblyName);

                return File.Exists(archSpecificPath)
                           ? System.Reflection.Assembly.LoadFile(archSpecificPath)
                           : null;
            }

            return null;
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            try
            {
                // Ignore any errors that might occur while closing the browser
                Cef.Shutdown();
            }
            catch { }
        }
    }
}
