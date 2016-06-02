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
using System.Security.Cryptography;

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
            ///////
            //This function is to Select a Foder
            ///////
            FolderBrowserDialog folderBrwsDlg = new FolderBrowserDialog();
            folderBrwsDlg.ShowDialog();
            textBox1.Text = folderBrwsDlg.SelectedPath; //put the path of the folder in the Textbox
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ///////
            //This function is to generate MAC for the content of the Folder´s Content
            ///////
            if (checkKeyValue())
            {
                string key = textBox2.Text; //The key used for hashing

                ASCIIEncoding encoding = new ASCIIEncoding();

                byte[] keyByte = encoding.GetBytes(key); //The key is transformed into array of Bytes

                HMACMD5 hmacmd5 = new HMACMD5(keyByte);

                if (checkFolderPath())  //To check if the folder is provided and not empty
                {

                    string[] files = Directory.GetFiles(textBox1.Text);
                    System.Windows.Forms.MessageBox.Show("يوجد: " + files.Length.ToString() + "ملف ", "عدد الملفات"); //Showing the No. of Files
                    StringBuilder strBuilder = new StringBuilder();
                    foreach (string currentFile in files) //Looping for each file of the folder
                    {
                        byte[] messageBytes = encoding.GetBytes(currentFile); //Transforming the file (string) to array of Bytes
                        byte[] hashMessage = hmacmd5.ComputeHash(messageBytes); // To compute the Hash
                        string macMessage = ByteToString(hashMessage); //Transform from byte to string using ByteToString Function
                        for (int i = 0; i < hashMessage.Length; i++) //loop to add all the folder content to one string
                        {
                            strBuilder.Append(hashMessage[i].ToString("X2")); //Add the string and trasnform to Hex
                        }
                    }
                    MessageBox.Show("MAC for the Folder: " + textBox1.Text + " is: \n" + strBuilder, "كود الوثوقية"); //This part is to show the MAC
                }
                ///////
                //This code for saving the MAC on Flash Memory
                ///////
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            ///////
            //This function is to generate MAC for the content of each file
            ///////
            if (checkKeyValue())
            {
                string key = textBox2.Text; //The key used for hashing

                ASCIIEncoding encoding = new ASCIIEncoding();

                byte[] keyByte = encoding.GetBytes(key); //The key is transformed into array of Bytes

                HMACMD5 hmacmd5 = new HMACMD5(keyByte);

                if (checkFolderPath())  //To check if the folder is provided and not empty
                {
                    string[] files = Directory.GetFiles(textBox1.Text);

                    System.Windows.Forms.MessageBox.Show("يوجد: " + files.Length.ToString() + "ملف ", "عدد الملفات"); //Showing the No. of Files

                    foreach (string currentFile in files) //Looping for each file of the folder
                    {
                        string text = File.ReadAllText(currentFile, Encoding.UTF8); //To read the file content
                        byte[] messageBytes = encoding.GetBytes(text); //Transforming the file (string) to array of Bytes
                        byte[] hashMessage = hmacmd5.ComputeHash(messageBytes); // To compute the Hash
                        string macMessage = ByteToString(hashMessage); //Transform from byte to string using ByteToString Function
                        MessageBox.Show("MAC for the file: " + currentFile + " content is: \n" + macMessage, "كود الوثوقية"); //To show the MAC of the content for each file
                    }
                }
                //This code for saving the MAC on Flash Memory
                ///////
                //
                ///////
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ///////
            //This function is to read the MAC of folder content which is stored on the Flash Memory
            ///////
            checkFolderPath();
            ////
            //reading from flash
        }


        private void button5_Click(object sender, EventArgs e)
        {
            ///////
            //This function is to read the MAC of the files which is stored on the Flash Memory
            ///////
            checkFolderPath();
            //reading from flash
        }

        private bool checkFolderPath()
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text)) //To check if the path is provided
            {
                string[] files = Directory.GetFiles(textBox1.Text);

                if (files.Length <= 0)
                {
                    MessageBox.Show("المجلد لا يحتوي أي ملف ولا يمكن توليد كود وثوقية");
                    return false;
                }
                return true;
            }
            else
            {
                MessageBox.Show("الرجاء تحديد المجلد أولاً", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private bool checkKeyValue()
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
                return true;
            else
            {
                MessageBox.Show("الرجاء إدخال مفتاح تشفير", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public static string ByteToString(byte[] buff)
        {
            ///////
            //This function is to Transform array of Bytes to string in Hexdecimal Format
            ///////
            string sbinary = "";
            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); //hex format
            }
            return sbinary;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

