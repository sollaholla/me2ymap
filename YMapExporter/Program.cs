using System;
using System.Windows.Forms;

namespace YMapExporter
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 1) 
            {
                Application.Run(new YMapExporter(args[0]));
            }
            else
            {
                Application.Run(new YMapExporter(""));
            }
        }
    }
}