namespace open_audit_config
{
    partial class OpenAuditConfigForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenAuditConfigForm));
            this.buttonContinue = new System.Windows.Forms.Button();
            this.textBoxId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDetails = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSelectCopy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonContinue
            // 
            this.buttonContinue.BackColor = System.Drawing.Color.YellowGreen;
            this.buttonContinue.Location = new System.Drawing.Point(12, 35);
            this.buttonContinue.Name = "buttonContinue";
            this.buttonContinue.Size = new System.Drawing.Size(209, 44);
            this.buttonContinue.TabIndex = 0;
            this.buttonContinue.Text = "Continuar";
            this.buttonContinue.UseVisualStyleBackColor = false;
            this.buttonContinue.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxId
            // 
            this.textBoxId.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.textBoxId.Location = new System.Drawing.Point(30, 9);
            this.textBoxId.Name = "textBoxId";
            this.textBoxId.Size = new System.Drawing.Size(191, 20);
            this.textBoxId.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "ID:";
            // 
            // textBoxDetails
            // 
            this.textBoxDetails.BackColor = System.Drawing.Color.Maroon;
            this.textBoxDetails.ForeColor = System.Drawing.Color.Orange;
            this.textBoxDetails.Location = new System.Drawing.Point(12, 98);
            this.textBoxDetails.Multiline = true;
            this.textBoxDetails.Name = "textBoxDetails";
            this.textBoxDetails.Size = new System.Drawing.Size(208, 99);
            this.textBoxDetails.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Detalhes:";
            // 
            // buttonSelectCopy
            // 
            this.buttonSelectCopy.Location = new System.Drawing.Point(13, 204);
            this.buttonSelectCopy.Name = "buttonSelectCopy";
            this.buttonSelectCopy.Size = new System.Drawing.Size(207, 25);
            this.buttonSelectCopy.TabIndex = 5;
            this.buttonSelectCopy.Text = "Selecionar e Copiar";
            this.buttonSelectCopy.UseVisualStyleBackColor = true;
            this.buttonSelectCopy.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 239);
            this.ControlBox = false;
            this.Controls.Add(this.buttonSelectCopy);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxDetails);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxId);
            this.Controls.Add(this.buttonContinue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Open Audit Config";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonContinue;
        private System.Windows.Forms.TextBox textBoxId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDetails;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonSelectCopy;
    }
}

