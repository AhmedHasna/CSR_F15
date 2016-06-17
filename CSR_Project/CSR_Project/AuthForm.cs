using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography; // مكتبة التعمية
using Newtonsoft.Json; // مكتبة التعامل مع json




namespace CSR_Project
{
    public partial class AuthFrm : Form
    {
        int FilesCount = 0;

        public AuthFrm()
        {
            InitializeComponent();
        }

        // زر الضغط على تحديد مجلد ------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            ///////
            //This function is to Select a Foder
            ///////
            FolderBrowserDialog folderBrwsDlg = new FolderBrowserDialog();
            folderBrwsDlg.ShowDialog();
            FolderPathtxt.Text = folderBrwsDlg.SelectedPath; //put the path of the folder in the Textbox
        }

        // توليد كود وثوقية للمجلد ------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            FilesCount = 0;
            /////// ==========================================
            //This code for saving the MAC on Flash Memory
            if (FolderMac() == null) return;
             UserFolder uf = new UserFolder
            {
                folderName = FolderPathtxt.Text, // اسم المجلد
                folderMac = FolderMac(), // الكود
                CountFiles = FilesCount, // عدد الملفات
                key = keytxt.Text // المفتاح السري
            };

            /*  convert the list of user folder to json: فيما يلي التحويل إلى جي سون */
            string json = JsonConvert.SerializeObject(uf);
            string JsonFilePath = FlashMeorytxt.Text + "\\csr.json";
            // MessageBox.Show(JsonFilePath);
            File.WriteAllText(JsonFilePath, json);
            MessageBox.Show("تم توليد كود الوثوقية على الفلاشة");
            ///////

        }

        // توليد كود وثوقية للملفات ------------------------------------------------
        private void button4_Click(object sender, EventArgs e)
        {
            ///////
            //This function is to generate MAC for the content of each file
            ///////
            if (checkKeyValue()) //To check of the encryption key is provided
            {
                string key = this.keytxt.Text; //The key used for hashing قيمة مفتاح التشفير

                ASCIIEncoding encoding = new ASCIIEncoding();

                byte[] keyByte = encoding.GetBytes(key); //The key is transformed into array of Bytes

                HMACMD5 hmacmd5 = new HMACMD5(keyByte);

                if (checkFolderPath())  //To check if the folder is provided and not empty
                {
                    string[] files = Directory.GetFiles(FolderPathtxt.Text);

                    System.Windows.Forms.MessageBox.Show("يوجد: " + files.Length.ToString() + "ملف ", "عدد الملفات"); //Showing the No. of Files

                    foreach (string currentFile in files) //Looping for each file of the folder
                    {
                        string text = File.ReadAllText(currentFile, Encoding.UTF8); //To read the file content
                        byte[] messageBytes = encoding.GetBytes(text); //Transforming the file (string) to array of Bytes
                        byte[] hashMessage = hmacmd5.ComputeHash(messageBytes); // To compute the Hash
                        string macMessage = ByteToString(hashMessage); //Transform from byte to string using ByteToString Function
                        // MessageBox.Show("MAC for the file: " + currentFile + " content is: \n" + macMessage, "كود الوثوقية"); //To show the MAC of the content for each file
                    }
                }
                //This code for saving the MAC on Flash Memory
                ///////
                //
                ///////
            }
        }

        // تحقق من كود الوثوقية للمجلد
        private void button3_Click(object sender, EventArgs e)
        {
            
            ///////
            //This function is to read the MAC of folder content which is stored on the Flash Memory
            ///////==================================
            if (!checkFolderPath()) return;


            // read object from the file قراء ملف جي سون
            string json_read = File.ReadAllText(FlashMeorytxt.Text + "\\csr.json");
            // deserialize the object 
            UserFolder data = JsonConvert.DeserializeObject<UserFolder>(json_read);

            // assign data to local variables:
            string FloderName = data.folderName;
            string FloderMac = data.folderMac;
            int CountFiles = data.CountFiles;
            string key1 = Convert.ToString(data.key);
            if (FloderMac == FolderMac())
                MessageBox.Show("تطابق, المجلد آمن ولم يحصل عليه أي تغيير");
            else MessageBox.Show("عدم تطابق, هناك تغيير في المجلد");
            ////
            //reading from flash
        }


        // تحقق من كود الوثوقية للملفات
        private void button5_Click(object sender, EventArgs e)
        {
            ///////
            //This function is to read the MAC of the files which is stored on the Flash Memory
            ///////
            checkFolderPath();
            //reading from flash
        }

        //  التحقق من وجود ملفات في لمجلد المختار
        private bool checkFolderPath()
        {
            if (!string.IsNullOrWhiteSpace(FolderPathtxt.Text)) //To check if the path is provided
            {
                string[] files = Directory.GetFiles(FolderPathtxt.Text);

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

        // التحقق من أنه تم إدخال مفتاح تشفير
        private bool checkKeyValue()
        {
            if (!string.IsNullOrEmpty(keytxt.Text))
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

        // استعراض المجلدات لتحديد الفلاشة
        private void browseFldashMemoryBtn_Click(object sender, EventArgs e)
        {
            // var drives = DriveInfo.GetDrives().Where(drive => drive.IsReady && drive.DriveType == DriveType.Removable);
            
            //This function is to Select a Foder            
            FolderBrowserDialog folderBrwsDlgFlashMemory = new FolderBrowserDialog();
            folderBrwsDlgFlashMemory.ShowDialog();
            FlashMeorytxt.Text = folderBrwsDlgFlashMemory.SelectedPath; //put the path of the folder in the Textbox
        }

        // تابع توليد كود الوثوقية للمجلد وإرجاعه مع إرجاع عدد الملفات داخله
        public string FolderMac()
        {
            ///////
            //This function is to generate MAC for the content of the Folder´s Content
            ///////
            if (!checkKeyValue()) return null; //To check of the encryption key is provided

            string flashMemoryPath = FlashMeorytxt.Text; // مسار الفلاشة من الحقل النصي
            if (!System.IO.Directory.Exists(flashMemoryPath)) // التحقق من أن المسار صحيح
            {
                MessageBox.Show("يرجى اختيار الفلاشة");
                return null;
            }

            string key = this.keytxt.Text; //The key used for hashing قيمة مفتاح التشفير

            ASCIIEncoding encoding = new ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(key); //The key is transformed into array of Bytes

            HMACMD5 hmacmd5 = new HMACMD5(keyByte);

            if (!checkFolderPath()) return null;  //To check if the folder is provided and not empty


            string[] files = Directory.GetFiles(FolderPathtxt.Text);
            int count = files.Length;
            FilesCount += count;
            // System.Windows.Forms.MessageBox.Show("يوجد: " + count.ToString() + "ملف ", "عدد الملفات"); //Showing the No. of Files
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
            // MessageBox.Show("MAC for the Folder: " + textBox1.Text + " is: \n" + strBuilder, "كود الوثوقية"); //This part is to show the MAC
            return Convert.ToString(strBuilder);
        }
    }
}

