using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSR_Project
{
    class UserFolder
    {

        // كلاس للمجلدات التي سنولد لها كود وثوقية
        
            public string folderName { get; set; } // اسم المجلد
            public string folderMac { get; set; } // كود الوثوقية mac
            public string key { get; set; } // المفتاح السري
            public int CountFiles { get; set; } // عدد الملفات التي يحتويها المجلد
        

    }
}
