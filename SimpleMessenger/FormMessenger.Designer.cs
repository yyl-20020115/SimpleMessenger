namespace SimpleMessenger
{
    partial class FormMessenger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMessenger));
            this.label2 = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.listClients = new System.Windows.Forms.ListBox();
            this.btnNewsFeed = new System.Windows.Forms.Button();
            this.btnHide = new System.Windows.Forms.Button();
            this.lblBuddyList = new System.Windows.Forms.Label();
            this.btnSendStatus = new System.Windows.Forms.Button();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.filwToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.messageSoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oFFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.richTxtNewsFeed = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelSelfIPPrompt = new System.Windows.Forms.Label();
            this.labelSelfIP = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label2.Name = "label2";
            // 
            // txtStatus
            // 
            this.txtStatus.BackColor = System.Drawing.SystemColors.HighlightText;
            this.txtStatus.ForeColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.txtStatus, "txtStatus");
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.TextChanged += new System.EventHandler(this.txtStatus_TextChanged);
            // 
            // listClients
            // 
            this.listClients.BackColor = System.Drawing.Color.MediumOrchid;
            this.listClients.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.listClients, "listClients");
            this.listClients.ForeColor = System.Drawing.Color.Black;
            this.listClients.FormattingEnabled = true;
            this.listClients.Name = "listClients";
            // 
            // btnNewsFeed
            // 
            this.btnNewsFeed.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.btnNewsFeed, "btnNewsFeed");
            this.btnNewsFeed.Name = "btnNewsFeed";
            this.btnNewsFeed.UseVisualStyleBackColor = false;
            this.btnNewsFeed.Click += new System.EventHandler(this.btnNewsFeed_Click);
            // 
            // btnHide
            // 
            resources.ApplyResources(this.btnHide, "btnHide");
            this.btnHide.ForeColor = System.Drawing.Color.Purple;
            this.btnHide.Name = "btnHide";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // lblBuddyList
            // 
            resources.ApplyResources(this.lblBuddyList, "lblBuddyList");
            this.lblBuddyList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblBuddyList.Name = "lblBuddyList";
            // 
            // btnSendStatus
            // 
            resources.ApplyResources(this.btnSendStatus, "btnSendStatus");
            this.btnSendStatus.Name = "btnSendStatus";
            this.btnSendStatus.UseVisualStyleBackColor = true;
            this.btnSendStatus.Click += new System.EventHandler(this.btnSendStatus_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.MediumOrchid;
            resources.ApplyResources(this.menuStrip, "menuStrip");
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filwToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Name = "menuStrip";
            // 
            // filwToolStripMenuItem
            // 
            this.filwToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.leaveToolStripMenuItem});
            this.filwToolStripMenuItem.Name = "filwToolStripMenuItem";
            resources.ApplyResources(this.filwToolStripMenuItem, "filwToolStripMenuItem");
            // 
            // leaveToolStripMenuItem
            // 
            this.leaveToolStripMenuItem.Name = "leaveToolStripMenuItem";
            resources.ApplyResources(this.leaveToolStripMenuItem, "leaveToolStripMenuItem");
            this.leaveToolStripMenuItem.Click += new System.EventHandler(this.LeaveToolStripMenuItem_Click_1);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.messageSoundToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            resources.ApplyResources(this.optionsToolStripMenuItem, "optionsToolStripMenuItem");
            // 
            // messageSoundToolStripMenuItem
            // 
            this.messageSoundToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oNToolStripMenuItem,
            this.oFFToolStripMenuItem});
            this.messageSoundToolStripMenuItem.Name = "messageSoundToolStripMenuItem";
            resources.ApplyResources(this.messageSoundToolStripMenuItem, "messageSoundToolStripMenuItem");
            // 
            // oNToolStripMenuItem
            // 
            this.oNToolStripMenuItem.Name = "oNToolStripMenuItem";
            resources.ApplyResources(this.oNToolStripMenuItem, "oNToolStripMenuItem");
            this.oNToolStripMenuItem.Click += new System.EventHandler(this.ONToolStripMenuItem_Click);
            // 
            // oFFToolStripMenuItem
            // 
            this.oFFToolStripMenuItem.Name = "oFFToolStripMenuItem";
            resources.ApplyResources(this.oFFToolStripMenuItem, "oFFToolStripMenuItem");
            this.oFFToolStripMenuItem.Click += new System.EventHandler(this.OFFToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manualToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            // 
            // manualToolStripMenuItem
            // 
            this.manualToolStripMenuItem.Name = "manualToolStripMenuItem";
            resources.ApplyResources(this.manualToolStripMenuItem, "manualToolStripMenuItem");
            this.manualToolStripMenuItem.Click += new System.EventHandler(this.ManualToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // richTxtNewsFeed
            // 
            this.richTxtNewsFeed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.richTxtNewsFeed, "richTxtNewsFeed");
            this.richTxtNewsFeed.Name = "richTxtNewsFeed";
            this.richTxtNewsFeed.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label1.Name = "label1";
            // 
            // labelSelfIPPrompt
            // 
            resources.ApplyResources(this.labelSelfIPPrompt, "labelSelfIPPrompt");
            this.labelSelfIPPrompt.Name = "labelSelfIPPrompt";
            // 
            // labelSelfIP
            // 
            resources.ApplyResources(this.labelSelfIP, "labelSelfIP");
            this.labelSelfIP.Name = "labelSelfIP";
            // 
            // FormMessenger
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Purple;
            this.Controls.Add(this.labelSelfIP);
            this.Controls.Add(this.labelSelfIPPrompt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTxtNewsFeed);
            this.Controls.Add(this.btnSendStatus);
            this.Controls.Add(this.lblBuddyList);
            this.Controls.Add(this.btnHide);
            this.Controls.Add(this.btnNewsFeed);
            this.Controls.Add(this.listClients);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.menuStrip);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMessenger";
            this.Load += new System.EventHandler(this.FormMessenger_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.ListBox listClients;
        private System.Windows.Forms.Button btnNewsFeed;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.Label lblBuddyList;
        private System.Windows.Forms.Button btnSendStatus;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem filwToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leaveToolStripMenuItem;
        private System.Windows.Forms.RichTextBox richTxtNewsFeed;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem messageSoundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oFFToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label labelSelfIPPrompt;
        private System.Windows.Forms.Label labelSelfIP;
    }
}