namespace open_audit_check_service
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.checkServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // checkServiceProcessInstaller
            // 
            this.checkServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.checkServiceProcessInstaller.Password = null;
            this.checkServiceProcessInstaller.Username = null;
            // 
            // checkServiceInstaller
            // 
            this.checkServiceInstaller.Description = "OpenAudit Service Checker";
            this.checkServiceInstaller.DisplayName = "open-audit-check-service";
            this.checkServiceInstaller.ServiceName = "open-audit-check-service";
            this.checkServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.checkServiceProcessInstaller,
            this.checkServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller checkServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller checkServiceInstaller;
    }
}