using open_audit_lib;
using open_audit_lib.dataobjects;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace open_audit_config
{
    static class OpenAuditConfigImpl
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            String arg = null;
            if (args != null && args.Length == 1)
            {
                String[] parts = args[0].Split('=');
                if(parts[0].Trim().Equals("id")) arg = parts[1];
            } 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // kill the open audit services
            MessageBox.Show("Terminando serviços OpenAudit....");
            Constants.KillServices();

            //Utils u = new Utils();
            //ConfigObj config = u.readConfig();
            //if (config.strId.Length <= 2)
            {
                Application.Run(new OpenAuditConfigForm(arg));
            }
            //else System.Environment.Exit(0);
        }
    }
}
