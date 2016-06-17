using System;
using System.Windows.Forms;

namespace CSR_Project
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            // تسجيل الدخول عندما يكون اسم المستخدم وكلمة المرور كما في الشرط
            if (UserNameTextBox.Text == "admin" && PasswordTextBox.Text == "123")
            {
                AuthFrm af = new AuthFrm();
                this.Hide(); // إخفاء الحالي
                af.ShowDialog(); // إظهار فورم توليد الأكواد والتحقق
            }
            else
                MessageBox.Show("بيانات الدخول غير صحيحة");

        }
    }
}
