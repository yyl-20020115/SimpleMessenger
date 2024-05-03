namespace SimpleMessenger
{
    partial class FormChat
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
            this.SendMessageBox = new System.Windows.Forms.RichTextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnBuZZ = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBoxEnter = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // SendMessageBox
            // 
            this.SendMessageBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SendMessageBox.BackColor = System.Drawing.Color.Thistle;
            this.SendMessageBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SendMessageBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SendMessageBox.Location = new System.Drawing.Point(0, 321);
            this.SendMessageBox.Multiline = true;
            this.SendMessageBox.Name = "SendMessageBox";
            this.SendMessageBox.Size = new System.Drawing.Size(270, 81);
            this.SendMessageBox.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSend.Location = new System.Drawing.Point(183, 286);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(87, 29);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "发送消息(&S)";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // btnBuZZ
            // 
            this.btnBuZZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuZZ.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuZZ.Location = new System.Drawing.Point(104, 286);
            this.btnBuZZ.Name = "btnBuZZ";
            this.btnBuZZ.Size = new System.Drawing.Size(73, 29);
            this.btnBuZZ.TabIndex = 4;
            this.btnBuZZ.Tag = "Clip";
            this.btnBuZZ.Text = "震动(&V)";
            this.btnBuZZ.UseVisualStyleBackColor = true;
            this.btnBuZZ.Click += new System.EventHandler(this.btnBuZZ_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AllowDrop = true;
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(270, 280);
            this.flowLayoutPanel1.TabIndex = 5;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // checkBoxEnter
            // 
            this.checkBoxEnter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxEnter.AutoSize = true;
            this.checkBoxEnter.Checked = true;
            this.checkBoxEnter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkBoxEnter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.checkBoxEnter.Location = new System.Drawing.Point(0, 293);
            this.checkBoxEnter.Name = "checkBoxEnter";
            this.checkBoxEnter.Size = new System.Drawing.Size(102, 16);
            this.checkBoxEnter.TabIndex = 6;
            this.checkBoxEnter.Tag = "";
            this.checkBoxEnter.Text = "按回车发送(&E)";
            this.checkBoxEnter.UseVisualStyleBackColor = true;
            this.checkBoxEnter.CheckedChanged += new System.EventHandler(this.CheckBoxEnter_CheckedChanged);
            // 
            // FormChat
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Purple;
            this.ClientSize = new System.Drawing.Size(270, 399);
            this.Controls.Add(this.btnBuZZ);
            this.Controls.Add(this.checkBoxEnter);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.SendMessageBox);
            this.MinimumSize = new System.Drawing.Size(286, 438);
            this.Name = "FormChat";
            this.Text = "聊天窗口";
            this.Load += new System.EventHandler(this.FormChat_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox SendMessageBox;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnBuZZ;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox checkBoxEnter;
    }
}