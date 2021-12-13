using Infrastructure.ConfigurationProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmazonSynchronizer
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
            ConnectionStringManager.SetConnectionString(Settings.SQLServerConnectionString);
            Application.Run(new Form1());
        }
    }
}
