using System;

namespace CSR_Project
{
    partial class AuthFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuthFrm));
            this.FolderPathtxt = new System.Windows.Forms.TextBox();
            this.keytxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FlashMeorytxt = new System.Windows.Forms.TextBox();
            this.browseFldashMemoryBtn = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.VerificateFolderMacBtn = new System.Windows.Forms.Button();
            this.GenerateCodeBtn = new System.Windows.Forms.Button();
            this.SelectFolderBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // FolderPathtxt
            // 
            this.FolderPathtxt.Location = new System.Drawing.Point(12, 12);
            this.FolderPathtxt.Name = "FolderPathtxt";
            this.FolderPathtxt.Size = new System.Drawing.Size(345, 20);
            this.FolderPathtxt.TabIndex = 1;
            // 
            // keytxt
            // 
            this.keytxt.Location = new System.Drawing.Point(12, 50);
            this.keytxt.Name = "keytxt";
            this.keytxt.Size = new System.Drawing.Size(322, 20);
            this.keytxt.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(340, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "حدد قيمة مفتاح التشفير";
            // 
            // FlashMeorytxt
            // 
            this.FlashMeorytxt.Location = new System.Drawing.Point(12, 227);
            this.FlashMeorytxt.Name = "FlashMeorytxt";
            this.FlashMeorytxt.Size = new System.Drawing.Size(326, 20);
            this.FlashMeorytxt.TabIndex = 10;
            // 
            // browseFldashMemoryBtn
            // 
            this.browseFldashMemoryBtn.Image = global::CSR_Project.Properties.Resources.usb;
            this.browseFldashMemoryBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.browseFldashMemoryBtn.Location = new System.Drawing.Point(356, 220);
            this.browseFldashMemoryBtn.Name = "browseFldashMemoryBtn";
            this.browseFldashMemoryBtn.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.browseFldashMemoryBtn.Size = new System.Drawing.Size(134, 33);
            this.browseFldashMemoryBtn.TabIndex = 9;
            this.browseFldashMemoryBtn.Text = "تحديد الفلاشة";
            this.browseFldashMemoryBtn.UseVisualStyleBackColor = true;
            this.browseFldashMemoryBtn.Click += new System.EventHandler(this.browseFldashMemoryBtn_Click);
            // 
            // button5
            // 
            this.button5.Image = global::CSR_Project.Properties.Resources.shield;
            this.button5.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button5.Location = new System.Drawing.Point(35, 132);
            this.button5.Name = "button5";
            this.button5.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.button5.Size = new System.Drawing.Size(207, 33);
            this.button5.TabIndex = 5;
            this.button5.Text = "تحقق من كود الوثوقية للملفات";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Image = global::CSR_Project.Properties.Resources.folder_1;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button4.Location = new System.Drawing.Point(35, 93);
            this.button4.Name = "button4";
            this.button4.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.button4.Size = new System.Drawing.Size(207, 33);
            this.button4.TabIndex = 4;
            this.button4.Text = "توليد كود وثوقية للملفات";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // VerificateFolderMacBtn
            // 
            this.VerificateFolderMacBtn.Image = global::CSR_Project.Properties.Resources.shield;
            this.VerificateFolderMacBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.VerificateFolderMacBtn.Location = new System.Drawing.Point(269, 132);
            this.VerificateFolderMacBtn.Name = "VerificateFolderMacBtn";
            this.VerificateFolderMacBtn.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.VerificateFolderMacBtn.Size = new System.Drawing.Size(198, 33);
            this.VerificateFolderMacBtn.TabIndex = 3;
            this.VerificateFolderMacBtn.Text = "تحقق من كود الوثوقية للمجلد";
            this.VerificateFolderMacBtn.UseVisualStyleBackColor = true;
            this.VerificateFolderMacBtn.Click += new System.EventHandler(this.button3_Click);
            // 
            // GenerateCodeBtn
            // 
            this.GenerateCodeBtn.Image = global::CSR_Project.Properties.Resources.browser;
            this.GenerateCodeBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.GenerateCodeBtn.Location = new System.Drawing.Point(269, 93);
            this.GenerateCodeBtn.Name = "GenerateCodeBtn";
            this.GenerateCodeBtn.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.GenerateCodeBtn.Size = new System.Drawing.Size(198, 33);
            this.GenerateCodeBtn.TabIndex = 2;
            this.GenerateCodeBtn.Text = "توليد كود وثوقية للمجلد";
            this.GenerateCodeBtn.UseVisualStyleBackColor = true;
            this.GenerateCodeBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // SelectFolderBtn
            // 
            this.SelectFolderBtn.Image = global::CSR_Project.Properties.Resources.folders;
            this.SelectFolderBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SelectFolderBtn.Location = new System.Drawing.Point(374, 5);
            this.SelectFolderBtn.Name = "SelectFolderBtn";
            this.SelectFolderBtn.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.SelectFolderBtn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SelectFolderBtn.Size = new System.Drawing.Size(116, 33);
            this.SelectFolderBtn.TabIndex = 0;
            this.SelectFolderBtn.Text = "تحديد مجلد";
            this.SelectFolderBtn.UseVisualStyleBackColor = true;
            this.SelectFolderBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 171);
            this.label1.MaximumSize = new System.Drawing.Size(450, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(429, 26);
            this.label1.TabIndex = 11;
            this.label1.Text = "عند التحقق من سلامة المجلد أو محتوى الملفات تأكد أنك تستعمل نفس مفتاح التشفير الذ" +
    "ي استعملته عند توليد كود الوثوقية حتى تحصل على نتيجة صحيحة";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(12, 76);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox1.Size = new System.Drawing.Size(478, 130);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::CSR_Project.Properties.Resources.key;
            this.pictureBox2.Location = new System.Drawing.Point(465, 50);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(25, 20);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 13;
            this.pictureBox2.TabStop = false;
            // 
            // AuthFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 265);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FlashMeorytxt);
            this.Controls.Add(this.browseFldashMemoryBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.keytxt);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.VerificateFolderMacBtn);
            this.Controls.Add(this.GenerateCodeBtn);
            this.Controls.Add(this.FolderPathtxt);
            this.Controls.Add(this.SelectFolderBtn);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(518, 303);
            this.MinimumSize = new System.Drawing.Size(518, 303);
            this.Name = "AuthFrm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "تأمين سلامة مجلد";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        //private void textBox1_TextChanged(object sender, EventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        #endregion

        private System.Windows.Forms.Button SelectFolderBtn;
        private System.Windows.Forms.TextBox FolderPathtxt;
        private System.Windows.Forms.Button GenerateCodeBtn;
        private System.Windows.Forms.Button VerificateFolderMacBtn;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox keytxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox FlashMeorytxt;
        private System.Windows.Forms.Button browseFldashMemoryBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}

