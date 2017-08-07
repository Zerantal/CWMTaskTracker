using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CWMTaskTracker.DAL;

namespace CWMTaskTracker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            const string dbFile = @"CWMSysAndTechJobs.sqlite";

            Database db = new Database(dbFile);

            if (!File.Exists(dbFile))
                db.InitDatabase();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(db));
        }
    }
}
