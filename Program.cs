using DevExpress.XtraWaitForm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtiConnect
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);
    }

    internal static class Program
    {
        private static Mutex _mutex;
        private const string MutexName = "ArtiConnectSingleInstanceMutex";

        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            CleanupMutex();

            // Tek örnek uygulamayı zorla
            bool createdNew;
            _mutex = new Mutex(true, MutexName, out createdNew);

            if (!createdNew)
            {
                // Uygulama zaten çalışıyor, mevcut pencereyi öne getir
                BringExistingInstanceToFront();
                return;
            }

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmAna());
            }
            finally
            { 
                if (_mutex != null)
                {
                    _mutex.ReleaseMutex();
                    _mutex.Dispose();
                }
            }
        }

        private static void CleanupMutex()
        {
            try
            {
                // Squirrel mutex'ini temizle
                string squirrelMutexName = $"Squirrel-{Squirrel.UpdateManager.GitHubUpdateManager("https://github.com/A1640A/ArtiConnect").Result.ApplicationName}";

                using (var mutex = Mutex.OpenExisting(squirrelMutexName))
                {
                    if (mutex != null)
                    {
                        try { mutex.ReleaseMutex(); } catch { }
                        mutex.Dispose();
                    }
                }
            }
            catch
            {
                // Mutex bulunamadı veya açılamadı, sorun değil
            }
        }

        private static void BringExistingInstanceToFront()
        {
            // Mevcut uygulamayı bul ve öne getir
            Process current = Process.GetCurrentProcess();
            foreach (Process process in Process.GetProcessesByName(current.ProcessName))
            {
                if (process.Id != current.Id)
                {
                    NativeMethods.SetForegroundWindow(process.MainWindowHandle);
                    break;
                }
            }
        }
    }
}
