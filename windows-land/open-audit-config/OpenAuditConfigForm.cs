using open_audit_lib;
using open_audit_lib.dataobjects;
using open_audit_lib.threads;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace open_audit_config
{
    public partial class OpenAuditConfigForm : Form
    {
        public OpenAuditConfigForm(String initParam)
        {
            InitializeComponent();
            Utils u = new Utils();
            this.Text = Constants.APP_NAME + " " + u.getAssemblyVersion();
            textBoxId.Focus();
            if (initParam != null && initParam.Length > 0)
            {
                textBoxId.Text = initParam;
                registerConfig(initParam);
            }
        }

        private bool restarted;

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Start();
        }

        private void Start()
        {
            if (!restarted)
            {
                // restart services
                Utils u = new Utils();
                Constants.StartServices(u);
                restarted = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            registerConfig(textBoxId.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBoxDetails.SelectAll();
            System.Windows.Forms.Clipboard.SetText(textBoxDetails.Text);
            MessageBox.Show("Message copied!", Constants.APP_NAME);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            ServiceThreadImpl sti = new ServiceThreadImpl();
            sti.runUploadTrafficSensor();
            sti.runDownloadTrafficSensor();
        }

        private void registerConfig(String id)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                buttonContinue.Enabled = false;
                textBoxDetails.Text = "PROCESSING...";
                if (id != null && id.Length > 3)
                {
                    Utils util = new Utils();
                    ConfigObj cfg = util.readConfig();
                    if (cfg != null && cfg.remoteServer != null && cfg.remoteServer.Length > 12)
                    {
                        String newCfg = util.getTextFromUrl(cfg.remoteServer + "?action=config&data=" + textBoxId.Text + "&version=" + util.getAssemblyVersion());

                        if (newCfg != null)
                        {
                            cfg = util.parseConfig(newCfg);
                            cfg.version = util.getAssemblyVersion();
                            if (cfg != null && cfg.remoteServer != null && cfg.remoteTarget != null && cfg.remoteTarget != null)
                            {
                                cfg.version = util.getAssemblyVersion();
                                util.writeConfig(cfg, util.getConfPath(Constants.CONF_PATH));
                                cfg = util.readConfig();
                                if (cfg != null && cfg.remoteTarget.Length > 12)
                                {
                                    String code = util.getUrlStatusCode(cfg.remoteTarget);
                                    if (code.Equals("OK"))
                                    {
                                        textBoxDetails.Text = "SUCCESS! ";
                                        Start();
                                        Environment.Exit(0);
                                    }
                                    else sb.Append("Unable to reach remote target! ");
                                }

                            }
                            else
                            {
                                sb.Append("Unable to reach remote server! ");
                            }
                        }
                        else
                        {
                            sb.Append("Invalid config file. ");
                        }
                    }
                    else
                    {
                        sb.Append("Unable to read pre-config file. ");
                    }
                }
                else
                {
                    sb.Append("ID cannot be null! ");
                    buttonContinue.Enabled = true;
                }

                if (sb.Length > 0) textBoxDetails.Text = (sb.ToString());
            }
            catch (Exception exc)
            {
                sb.Append(" Failed at initialization. " + exc.Message);
                buttonContinue.Enabled = true;
                textBoxDetails.Text = (sb.ToString());
            }
        }
    }
}
