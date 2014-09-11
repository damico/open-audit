using open_audit_lib;
using open_audit_lib.dataobjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace open_audit_config
{
    public partial class OpenAuditConfigForm : Form
    {
        public OpenAuditConfigForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                buttonContinue.Enabled = false;
                textBoxDetails.Text = "PROCESSING...";
                if (textBoxId != null && textBoxId.Text.Length > 3)
                {
                    Utils util = new Utils();
                    ConfigObj cfg = util.readConfig();
                    if (cfg != null && cfg.remoteServer != null && cfg.remoteServer.Length > 12)
                    {
                        String newCfg = util.getTextFromUrl(cfg.remoteServer + "?action=config&data=" + textBoxId.Text);
                        if (newCfg != null)
                        {
                            cfg = util.parseConfig(newCfg);
                            if (cfg != null && cfg.remoteServer != null && cfg.remoteTarget != null && cfg.remoteTarget != null)
                            {
                                util.writeConfig(newCfg, Constants.CONF_PATH);
                                cfg = util.readConfig();
                                if (cfg != null && cfg.remoteTarget.Length > 12)
                                {
                                    String code = util.getUrlStatusCode(cfg.remoteTarget);
                                    if (code.Equals("OK"))
                                    {
                                        textBoxDetails.Text = "SUCCESS!";
                                        Application.Exit();
                                    }
                                    else textBoxDetails.Text = "Unable to reach remote target!";
                                }

                            }
                            else
                            {
                                textBoxDetails.Text = "Unable to reach remote server!";
                            }
                        }
                        else
                        {
                            textBoxDetails.Text = "Invalid config file.";
                        }
                    }
                    else
                    {
                        textBoxDetails.Text = "Unable to read pre-config file.";
                    }
                }
                else
                {
                    textBoxDetails.Text = "ID cannot be null!";
                    buttonContinue.Enabled = true;
                }
            }
            catch (Exception exc)
            {
                textBoxDetails.Text = ("Failed at initialization. " + exc.Message);
                buttonContinue.Enabled = true;
            }
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBoxDetails.SelectAll();
            System.Windows.Forms.Clipboard.SetText(textBoxDetails.Text);
            MessageBox.Show("Message copied!",  Constants.APP_NAME);
        }
    }
}
