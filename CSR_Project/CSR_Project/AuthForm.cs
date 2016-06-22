using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography; // مكتبة التعمية
using Newtonsoft.Json; // مكتبة التعامل مع json http://www.newtonsoft.com/json
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace CSR_Project
{
    public partial class AuthFrm : Form
    {
        int FilesCount = 0; // عدد الملفات في المجلد

        public AuthFrm()
        {
            InitializeComponent();
        }

        // زر الضغط على تحديد مجلد ------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            ///////
            //وظيفة تحديد مجلد
            ///////
            try
            {
                FolderBrowserDialog folderBrwsDlg = new FolderBrowserDialog();
                folderBrwsDlg.ShowDialog();
                FolderPathtxt.Text = folderBrwsDlg.SelectedPath; //put the path of the folder in the Textbox
            }
            catch (Exception ex)
            {
                MessageBox.Show("Source: " + ex.Source + "\n Message: " + ex.Message + "\n" + "حصل خطأ أثناء عمل البرنامج.. الرجاء الاتصال بمطوري النظام");
            }
        }

        // توليد كود وثوقية للمجلد ------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Source: " + ex.Source + "\n Message: " + ex.Message + "\n" + "حصل خطأ أثناء عمل البرنامج.. الرجاء الاتصال بمطوري النظام");
            }
        }

        // توليد كود وثوقية للملفات ------------------------------------------------
        private void button4_Click(object sender, EventArgs e)
        {
            ///////
            // This function is to generate MAC for the content of each file
            ///////
            try
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
                                           //   MessageBox.Show(macMessage);
                        }
                        //This code for saving the MAC on Flash Memory
                        ///////
                        //This code for saving the MAC on Flash Memory
                        string flashMemoryPath = FlashMeorytxt.Text; // مسار الفلاشة من الحقل النصي
                        if (!System.IO.Directory.Exists(flashMemoryPath)) // التحقق من أن المسار صحيح
                        {
                            MessageBox.Show("يرجى اختيار الفلاشة");
                            return;
                        }
                        /*  convert the list of user folder to json: فيما يلي التحويل إلى جي سون */
                        string json = JsonConvert.SerializeObject(listuf);
                        string JsonFilePath = FlashMeorytxt.Text + "\\csr_files.json";
                        // MessageBox.Show(JsonFilePath);
                        File.WriteAllText(JsonFilePath, json);
                        MessageBox.Show("عدد الملفات التي يحتويها هذا المجلد " + files.Length.ToString() + Environment.NewLine + "تم توليد كود الوثوقية على الفلاشة");

                        ///////
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Source: " + ex.Source + "\n Message: " + ex.Message + "\n" + "حصل خطأ أثناء عمل البرنامج.. الرجاء الاتصال بمطوري النظام");
            }
        } // end button4_Click

        // تحقق من كود الوثوقية للمجلد ------------------------------------------------
        private void button3_Click(object sender, EventArgs e)
        {
            ///////
            //This function is to read the MAC of folder content which is stored on the Flash Memory
            ///////==================================
            try
            {
                if (!checkFolderPath()) return;

                // read object from the file قراء ملف جي سون
                string JsonFilePath = FlashMeorytxt.Text + "\\csr.json";
                if (!System.IO.File.Exists(JsonFilePath)) // التحقق من وجود ملف جي سون
                {
                    MessageBox.Show("لم يتم توليد كود وثوقية لهذا المجلد من قبل");
                    return;
                }
                string json_read = File.ReadAllText(JsonFilePath); // قراءة ملف جي سون
                                                                   // deserialize the object 
                UserFolder data = JsonConvert.DeserializeObject<UserFolder>(json_read);

                // assign data to local variables:
                string FloderName = data.folderName;
                string FloderMac = data.folderMac;
                int CountFiles = data.CountFiles;
                string key1 = Convert.ToString(data.key);
                if (FloderMac == FolderMac())
                    MessageBox.Show("تطابق, لم يحصل أي تغيير في عدد الملفات وأسمائها, لكن ينصح بالتحقق من أنه لم يحصل تغيير في محتوى الملفات عن طريق زر التحقق من كود الوثوقية للملفات");
                else MessageBox.Show("عدم تطابق, هناك تغيير في المجلد");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Source: " + ex.Source + "\n Message: " + ex.Message + "\n" + "حصل خطأ أثناء عمل البرنامج.. الرجاء الاتصال بمطوري النظام");
            }
            ////
            // end reading from flash
        }

        // تحقق من كود الوثوقية للملفات ------------------------------------------------
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkKeyValue()) return; // إنهاء في حال عدم وجود مفتاح تشفير
                if (!System.IO.Directory.Exists(FlashMeorytxt.Text))
                {
                    MessageBox.Show("يرجى تحديد الفلاشة التي تم تخزين الملف بها");
                    return;
                }
                string JsonFilePath = FlashMeorytxt.Text + "\\csr_files.json"; // مسار ملف جي سون للملفات
                if (!System.IO.File.Exists(JsonFilePath)) // التحقق من وجود ملف جي سون
                {
                    MessageBox.Show("لم يتم توليد كود وثوقية لهذه الملفات من قبل");
                    return;
                }
                string key = this.keytxt.Text; //The key used for hashing قيمة مفتاح التشفير
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] keyByte = encoding.GetBytes(key); //The key is transformed into array of Bytes
                HMACMD5 hmacmd5 = new HMACMD5(keyByte);
                if (!checkFolderPath()) return;  //To check if the folder is provided and not empty

                string[] files = Directory.GetFiles(FolderPathtxt.Text);

                List<UserFile> listuf = new List<UserFile>(); // قائمة الملفات

                // جلب ملف جي سون من الفلاشة

                string json_read = File.ReadAllText(JsonFilePath); // قراءة ملف جي سون

                JArray v = JArray.Parse(json_read); // المصدر: http://stackoverflow.com/questions/19025174/get-length-of-array-json-net

                string filesChanged = ""; // الملفات التي تغيرت
                int CountFilesChanged = 0; // عدد الملفات التي تغيرت
                string FilesRemoved = ""; // الملفات المحذوفة
                int CountFilesRemoved = 0; // عدد الملفات المحذوفة

                foreach (string currentFile in files) //Looping for each file of the folder
                {
                    string text = File.ReadAllText(currentFile, Encoding.UTF8); //To read the file content
                    byte[] messageBytes = encoding.GetBytes(text); //Transforming the file (string) to array of Bytes
                    byte[] hashMessage = hmacmd5.ComputeHash(messageBytes); // To compute the Hash
                    string macMessage = ByteToString(hashMessage); //Transform from byte to string using ByteToString Function

                    for (int i = 0; i < v.Count; i++)
                    {
                        if (v[i]["FileName"].ToString() == currentFile) // مقارنة الملفات في المجلد وفي جي سون
                        {
                            if (v[i]["FileMac"].ToString() != macMessage) // إذا تغير الماك لملف
                            {
                                // عدم تطابق في هذا الملف
                                filesChanged += currentFile + Environment.NewLine; // تجميع أسماء الملفات التي تغيرت
                                CountFilesChanged++; // زيادة عدد الملفات التي تغيرات بمقدار واحد
                            }
                        } // end if;
                        if (!files.Contains(v[i]["FileName"].ToString()))
                        { // التحقق من وجود الملف
                            FilesRemoved += v[i]["FileName"].ToString() + Environment.NewLine; // اضافة الملف الغير موجود لقائمة الملفات المحذوفة
                            CountFilesRemoved++; // زيادة عدد الملفات المحذوفة بمقدار واحد
                        }
                    } // end for;
                } // end foreach;
                  // النتائج:
                if (CountFilesChanged == 0 && CountFilesRemoved == 0)
                {
                    MessageBox.Show("جميع الملفات آمنة ولم يطرأ عليها أي تغيير");
                    return;
                }

                if (CountFilesChanged > 0)
                    MessageBox.Show("تم التغيير في " + CountFilesChanged + " ملف :" + Environment.NewLine + filesChanged);
                if (CountFilesRemoved > 0)
                    MessageBox.Show("يوجد " + CountFilesRemoved + " ملف محذوف: " + Environment.NewLine + FilesRemoved);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Source: " + ex.Source + "\n Message: " + ex.Message + "\n" + "حصل خطأ أثناء عمل البرنامج.. الرجاء الاتصال بمطوري النظام");
            }
        } // end button 5 click

        //  التحقق من وجود ملفات في المجلد المختار
        private bool checkFolderPath()
        {
            if (!string.IsNullOrWhiteSpace(FolderPathtxt.Text)) //To check if the path is provided
            {
                try
                {

                    string[] files = Directory.GetFiles(FolderPathtxt.Text);
                    if (files.Length <= 0)
                    {
                        MessageBox.Show("المجلد لا يحتوي أي ملف ولا يمكن توليد كود وثوقية");
                        return false;
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Source: " + ex.Source + "\n Message: " + ex.Message + "\n"+ "حصل خطأ أثناء عمل البرنامج.. الرجاء الاتصال بالدعم الفني");
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
            try
            {
                FolderBrowserDialog folderBrwsDlgFlashMemory = new FolderBrowserDialog();
                folderBrwsDlgFlashMemory.ShowDialog();
                FlashMeorytxt.Text = folderBrwsDlgFlashMemory.SelectedPath; //put the path of the folder in the Textbox
            }
            catch (Exception ex)
            {
                MessageBox.Show("Source: " + ex.Source + "\n Message: " + ex.Message + "\n" + "حصل خطأ أثناء عمل البرنامج.. الرجاء الاتصال بمطوري النظام");
            }
        }

        // تابع توليد كود الوثوقية للمجلد وإرجاعه مع إرجاع عدد الملفات داخله
        public string FolderMac()
        {
            ///////
            //This function is to generate MAC for the content of the Folder´s Content
            ///////
            try
            {
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
                byte[] strBuildBytes = encoding.GetBytes(strBuilder.ToString());
                byte[] hasStr = hmacmd5.ComputeHash(strBuildBytes);
                string macFolderMessage = ByteToString(hasStr);
                //MessageBox.Show(macFolderMessage);
                return Convert.ToString(strBuilder);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Source: " + ex.Source + "\n Message: " + ex.Message + "\n" + "حصل خطأ أثناء عمل البرنامج.. الرجاء الاتصال بمطوري النظام");
                return null;
            }
        }

        // رابط حساب أحمد أبو حسنة على فيس بوك
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/ahmed.abuhasna");
        }

        // رابط حساب يوسف زيبق على فيس بوك
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/yousef.sos.16");
        }

        // رابط حساب زهير طه على فيس بوك
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/zuhairtaha");
        }
    }
}

