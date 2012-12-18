using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Top4ever.Pos
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormLogin formLogin = new FormLogin();
            if (formLogin.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new FormDesk());
            }
        }
    }
}
