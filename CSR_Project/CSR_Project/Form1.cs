using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSR_Project
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrwsDlg = new FolderBrowserDialog();

            DialogResult result = folderBrwsDlg.ShowDialog();

            if (!string.IsNullOrWhiteSpace(folderBrwsDlg.SelectedPath))
            {
                string[] files = Directory.GetFiles(folderBrwsDlg.SelectedPath);

                System.Windows.Forms.MessageBox.Show("يوجد: " + files.Length.ToString() + "ملف ", "عدد الملفات");

                foreach (string currentFile in files)
                {

                    MessageBox.Show(currentFile);

                }
                textBox1.Text = folderBrwsDlg.SelectedPath;

            }

        }


    }
}
