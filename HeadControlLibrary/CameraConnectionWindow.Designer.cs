namespace HeadControlLibrary
{
    partial class CameraConnectionWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraConnectionWindow));
            this.choices = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.localCameras = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ipAddress = new System.Windows.Forms.TextBox();
            this.ipLogin = new System.Windows.Forms.TextBox();
            this.ipPassword = new System.Windows.Forms.MaskedTextBox();
            this.buttonTest = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            this.progress = new System.Windows.Forms.ToolStripProgressBar();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // choices
            // 
            this.choices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.choices.FormattingEnabled = true;
            this.choices.Items.AddRange(new object[] {
            "Połączenie strumieniowe (MJPEG)",
            "Połączenie klatkowe (JPEG)"});
            this.choices.Location = new System.Drawing.Point(16, 29);
            this.choices.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.choices.Name = "choices";
            this.choices.Size = new System.Drawing.Size(395, 24);
            this.choices.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Rodzaj kamery:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Kamery lokalne:";
            // 
            // localCameras
            // 
            this.localCameras.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.localCameras.FormattingEnabled = true;
            this.localCameras.Location = new System.Drawing.Point(16, 89);
            this.localCameras.Name = "localCameras";
            this.localCameras.Size = new System.Drawing.Size(395, 24);
            this.localCameras.TabIndex = 3;
            this.localCameras.SelectedIndexChanged += new System.EventHandler(this.localCameras_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Adres kamery IP:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 254);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "Hasło kamery IP:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 188);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "Login kamery IP:";
            // 
            // ipAddress
            // 
            this.ipAddress.Location = new System.Drawing.Point(16, 152);
            this.ipAddress.Name = "ipAddress";
            this.ipAddress.Size = new System.Drawing.Size(395, 22);
            this.ipAddress.TabIndex = 7;
            this.ipAddress.TextChanged += new System.EventHandler(this.ipAddress_TextChanged);
            // 
            // ipLogin
            // 
            this.ipLogin.Location = new System.Drawing.Point(16, 207);
            this.ipLogin.Name = "ipLogin";
            this.ipLogin.Size = new System.Drawing.Size(395, 22);
            this.ipLogin.TabIndex = 8;
            this.ipLogin.TextChanged += new System.EventHandler(this.ipLogin_TextChanged);
            // 
            // ipPassword
            // 
            this.ipPassword.Location = new System.Drawing.Point(16, 273);
            this.ipPassword.Name = "ipPassword";
            this.ipPassword.Size = new System.Drawing.Size(395, 22);
            this.ipPassword.TabIndex = 9;
            this.ipPassword.TextChanged += new System.EventHandler(this.ipPassword_TextChanged);
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(16, 313);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(138, 23);
            this.buttonTest.TabIndex = 10;
            this.buttonTest.Text = "Testuj połączenie";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(160, 313);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(140, 23);
            this.buttonConnect.TabIndex = 11;
            this.buttonConnect.Text = "Połącz";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(306, 313);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(105, 23);
            this.buttonClose.TabIndex = 12;
            this.buttonClose.Text = "Zamknij okno";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status,
            this.progress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 361);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(430, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // status
            // 
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(0, 17);
            // 
            // progress
            // 
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(100, 16);
            this.progress.Visible = false;
            // 
            // CameraConnectionWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 383);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.ipPassword);
            this.Controls.Add(this.ipLogin);
            this.Controls.Add(this.ipAddress);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.localCameras);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.choices);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(446, 422);
            this.MinimumSize = new System.Drawing.Size(446, 422);
            this.Name = "CameraConnectionWindow";
            this.Text = "CameraConnectionWindow";
            this.Load += new System.EventHandler(this.CameraConnectionWindow_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox choices;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox localCameras;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ipAddress;
        private System.Windows.Forms.TextBox ipLogin;
        private System.Windows.Forms.MaskedTextBox ipPassword;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel status;
        private System.Windows.Forms.ToolStripProgressBar progress;
    }
}