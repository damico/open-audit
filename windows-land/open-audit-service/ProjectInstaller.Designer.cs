namespace open_audit_service
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
            this.OpenAuditServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.OpenAuditServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // OpenAuditServiceProcessInstaller
            // 
            this.OpenAuditServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.OpenAuditServiceProcessInstaller.Password = null;
            this.OpenAuditServiceProcessInstaller.Username = null;
            // 
            // OpenAuditServiceInstaller
            // 
            this.OpenAuditServiceInstaller.Description = "Service responsible by Open Audit async calls";
            this.OpenAuditServiceInstaller.DisplayName = "Open-Audit-Service";
            this.OpenAuditServiceInstaller.ServiceName = "open-audit-service";
            this.OpenAuditServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.OpenAuditServiceProcessInstaller,
            this.OpenAuditServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller OpenAuditServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller OpenAuditServiceInstaller;
    }
}