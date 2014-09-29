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
            Application.Run(new OpenAuditConfigForm(arg));
        }
    }
}
