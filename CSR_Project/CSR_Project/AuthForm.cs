using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography; // مكتبة التعمية
using Newtonsoft.Json; // مكتبة التعامل مع json
using System.Collections.Generic;

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
            // This function is to generate MAC for the content of each file
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
                    List<UserFile> listuf = new List<UserFile>(); // قائمة الملفات

                    foreach (string currentFile in files) //Looping for each file of the folder
                    {
                        string text = File.ReadAllText(currentFile, Encoding.UTF8); //To read the file content
                        byte[] messageBytes = encoding.GetBytes(text); //Transforming the file (string) to array of Bytes
                        byte[] hashMessage = hmacmd5.ComputeHash(messageBytes); // To compute the Hash
                        string macMessage = ByteToString(hashMessage); //Transform from byte to string using ByteToString Function
                        // MessageBox.Show("MAC for the file: " + currentFile + " content is: \n" + macMessage, "كود الوثوقية"); //To show the MAC of the content for each file
                        UserFile f = new UserFile
                        {
                            FileName = currentFile,
                            FileMac = macMessage,
                            key = keytxt.Text
                        };
                        listuf.Add(f); // إضافة الملف إلى القائمة
                    }
                    //This code for saving the MAC on Flash Memory
                    ///////
                    //This code for saving the MAC on Flash Memory

                    /*  convert the list of user folder to json: فيما يلي التحويل إلى جي سون */
                    string json = JsonConvert.SerializeObject(listuf);
                    string JsonFilePath = FlashMeorytxt.Text + "\\csr_files.json";
                    // MessageBox.Show(JsonFilePath);
                    File.WriteAllText(JsonFilePath, json);
                    MessageBox.Show("تم توليد كود الوثوقية على الفلاشة");
                    ///////
                }

            }
        }

        // تحقق من كود الوثوقية للمجلد ------------------------------------------------
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
            // end reading from flash
        }


        // تحقق من كود الوثوقية للملفات ------------------------------------------------
        private void button5_Click(object sender, EventArgs e)
        {
            if (checkKeyValue()) //To check of the encryption key is provided
            {
                string key = this.keytxt.Text; //The key used for hashing قيمة مفتاح التشفير
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] keyByte = encoding.GetBytes(key); //The key is transformed into array of Bytes
                HMACMD5 hmacmd5 = new HMACMD5(keyByte);
                if (checkFolderPath())  //To check if the folder is provided and not empty
                {
                    string[] files = Directory.GetFiles(FolderPathtxt.Text);
                    // System.Windows.Forms.MessageBox.Show("يوجد: " + files.Length.ToString() + "ملف ", "عدد الملفات"); //Showing the No. of Files
                    List<UserFile> listuf = new List<UserFile>(); // قائمة الملفات

                    // string error = ""; // تخزين الأخطاء في هذه السلسلة

                    // read object from the file قراء ملف جي سون
                    string json_read = File.ReadAllText(FlashMeorytxt.Text + "\\csr_files.json");
                    // deserialize the object 
                    UserFile data = JsonConvert.DeserializeObject<UserFile>(json_read);

                    foreach (string currentFile in files) //Looping for each file of the folder
                    {
                        string text = File.ReadAllText(currentFile, Encoding.UTF8); //To read the file content
                        byte[] messageBytes = encoding.GetBytes(text); //Transforming the file (string) to array of Bytes
                        byte[] hashMessage = hmacmd5.ComputeHash(messageBytes); // To compute the Hash
                        string macMessage = ByteToString(hashMessage); //Transform from byte to string using ByteToString Function
                        // MessageBox.Show("MAC for the file: " + currentFile + " content is: \n" + macMessage, "كود الوثوقية"); //To show the MAC of the content for each file
                        UserFile f = new UserFile
                        {
                            FileName = currentFile,
                            FileMac = macMessage,
                            key = keytxt.Text
                        };
                        listuf.Add(f); // إضافة الملف إلى القائمة



                    } // end foreach currentfile
                    //This code for saving the MAC on Flash Memory
                    ///////
                    //This code for saving the MAC on Flash Memory

                    /*  convert the list of user folder to json: فيما يلي التحويل إلى جي سون */
                    string json = JsonConvert.SerializeObject(listuf);

                    ///////
                }
            }









            /*
             * if (FloderMac == FolderMac())
                MessageBox.Show("تطابق, المجلد آمن ولم يحصل عليه أي تغيير");
            else MessageBox.Show("عدم تطابق, هناك تغيير في المجلد");
            */




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

        // استعراض المجلدات لتحديد الفلاشة ------------------------------------------------
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            string jsonText = "[{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\1.jpg\",\"FileMac\":\"3ED1FF9025937D8FF9C1B4BD3FC35CBA\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\20160601_195553.jpg\",\"FileMac\":\"E0A1BDFF2C796A0AC49D9B38E80BFB44\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\Capture.PNG\",\"FileMac\":\"4F4D7111DDC2E3453BC11BBFB785C272\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\carsyria_db.rar\",\"FileMac\":\"E0EF9B6014E2BF8D46D93D8848D9785A\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\csr.json\",\"FileMac\":\"A62A61FDBE18276C4B7223242592D715\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\DataClustering.lnk\",\"FileMac\":\"D67D35FF2DE2C26211C23C47261CC464\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\desktop.ini\",\"FileMac\":\"E28AB3E6EA4940DD6A61A7B6C865EED9\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\F15 - Shortcut.lnk\",\"FileMac\":\"F28EE1297DD28B69294B87616AB104FF\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\FlashFXP 5.lnk\",\"FileMac\":\"C87F1A699601C7481EAECC94DA8C18C7\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\Format Factory.lnk\",\"FileMac\":\"D620696302DB639E1FB9BFD82DA5BDAB\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\Google Chrome.lnk\",\"FileMac\":\"5D30118910331539770398DD20F3ACE9\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\hand-touching-tablet-screen.psd\",\"FileMac\":\"18DD8E34E076CC078A409800F692B797\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\index.html\",\"FileMac\":\"4B10A8A4F63FB5559771E1BDB132F29C\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\index.jpg\",\"FileMac\":\"134E85037EB900F43328063BAC6B471D\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\Internet Download Manager.lnk\",\"FileMac\":\"2A475A7C4FEA43C1C946E0BCB927C134\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\ISE_SI_S14_C3_HW2modar_51325_zuhair_53957_.docx\",\"FileMac\":\"FBE6E64EBC7E9D5F87FC9D9151F5EFAE\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\ISE_SI_S14_C3_modar_51325_zuhair_53957.docx\",\"FileMac\":\"21711C0B0D70F1307FB9F13B345C6AF3\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\mysql-connector-java-5.0.4.zip\",\"FileMac\":\"D2E40D934B0E668D184AF6511E9239EE\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\mysql-connector-java-5.1.29.pom\",\"FileMac\":\"7B30E1BD3B964712B6EF52DEE023CE18\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\pingendo-win64.zip\",\"FileMac\":\"45C8377DDB7ECA318237DD1AD181A2CD\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\psiphon3.exe\",\"FileMac\":\"879D144A04E551DDE652788EE4BBF4DB\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\st.sql\",\"FileMac\":\"825FA31BFC18C254ED17D41F848C9AC6\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\style.css\",\"FileMac\":\"453A1B6318DB34D2E3C968CA7F957D14\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\style.less\",\"FileMac\":\"F60DB1F12F4EA92750E2AF44A81BCC8B\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\test.html\",\"FileMac\":\"297E1146EADD9642BB00E107763149AA\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\test.txt\",\"FileMac\":\"E910D487CAD4BDB27545318A1FE31EC5\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\ThinkstockPhotos-525494862.jpg\",\"FileMac\":\"D3C22DFB9086C5E31F7CE1373CB78068\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\u1504.exe\",\"FileMac\":\"07A2D4F0CB4829674AFAB6CCB78BC171\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\update_imgs.sql\",\"FileMac\":\"06E22F0D27DB6B45CDE8DC133E2250DC\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\VID-20160604-WA0006.mp4\",\"FileMac\":\"7BD4B9B4F40D341D33E4EB8DDABE5645\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\WhatsApp-Image-20160523.jpg\",\"FileMac\":\"C66E18E7D2CDCE0A01F7D784865757B7\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\WhatsApp.apk\",\"FileMac\":\"47903E449B327482E3FBE4DC4D7467CD\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\Windows 7 USB DVD Download Tool.lnk\",\"FileMac\":\"69260C5E57E73E9A7EB5F689C0593382\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\xampp-control - Shortcut.lnk\",\"FileMac\":\"B92BAB08B921839885C4CDE1AD707CCD\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\Zuhair Taha_ كل الحملات الإعلانية_ 1 مايو_ 2016 _ 31 مايو_ 2016.xls\",\"FileMac\":\"2058CA84D3AE7D2C855C05F3AC57A6E4\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\رجيم-رمضان-620x330.jpg\",\"FileMac\":\"8C0CAD82B333190E42128936AB627D80\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\رجيم-سريع-جدا.jpg\",\"FileMac\":\"522B91AFB4342188ED00E3995074D429\",\"key\":\"123\"},{\"FileName\":\"C:\\Users\\zuhair\\Desktop\\مراسلات ايمن.docx\",\"FileMac\":\"C46651B2B256BE99EA26E5D23E5164DB\",\"key\":\"123\"}]";

            using (var reader = new JsonTextReader(new StringReader(jsonText)))
            {
                while (reader.Read())
                {
                    MessageBox.Show(reader.TokenType + " " + reader.ValueType + " " + reader.Value);
                }
            }

        }
    }
}

