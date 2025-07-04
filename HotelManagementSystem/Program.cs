using HotelManagementSystem.Login;
using HotelManagementSystem.GlobalClasses;
using System;
using System.Windows.Forms;

namespace HotelManagementSystem
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Idle += (s, e) =>
            {
                foreach (Form frm in Application.OpenForms)
                    clsUtility.RemoveIconAnimation(frm);
            };

            frmLogin loginForm = new frmLogin();
            clsUtility.RemoveIconAnimation(loginForm);
            Application.Run(loginForm);

        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
