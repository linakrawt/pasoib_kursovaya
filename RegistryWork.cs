using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
using System.Security.AccessControl;
using System.Windows.Forms;
using System.Diagnostics;

namespace kursovaya_pasoib
{
    class RegistryWork
    {
        public bool isExist()
        {
            if (Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer") != null) //если путь существует
            {
                return true;
            }
            else return false;
        }


   

            public void LoadKey(string userName)
            {
                if (Registry.Users.OpenSubKey(userName) != null) //если путь существует
                {
                    return;
                }
                else
                {


                    Process load = new Process();
                    ProcessStartInfo startInfo2 = new ProcessStartInfo();
                    startInfo2.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo2.FileName = "cmd.exe";
                    startInfo2.Arguments = "/c reg load HKU" + @"\" + userName + @" C:\Users\" + userName + @"\ntuser.dat";
                    load.StartInfo = startInfo2;
                    load.Start();



                }
            }


        public void CreateUserKey(string userName)
        {
            if (Registry.Users.OpenSubKey(userName + @"\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\DisallowRun") != null)
            { return; }
            else
            {

               

                string nameKey = "Explorer";
                string a = userName + @"\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies";
                using (RegistryKey regKey = Registry.Users.OpenSubKey(userName + @"\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))

                    regKey.CreateSubKey(nameKey);

                MessageBox.Show("Раздел создан 1");
                using (RegistryKey regKey2 = Registry.Users.OpenSubKey(userName + @"\" + @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))

                    regKey2.SetValue("DisallowRun", "1", RegistryValueKind.DWord);
                using (RegistryKey regKey3 = Registry.Users.OpenSubKey(userName + @"\" + @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))

                    regKey3.CreateSubKey("DisallowRun");
                MessageBox.Show("Раздел создан 2");

            }
        }


        public void CreateUserValue(string fileName, string Name, string userName)
        {

            using (RegistryKey regKey5 = Registry.Users.OpenSubKey(userName + @"\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\DisallowRun", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))
            {

                regKey5.SetValue(Name, fileName);
            };

            MessageBox.Show("Программа добавлена. Перезагрузите компьютер");
        }

        public void DeleteUserValue(string fileName, string userName)
        {
            using (RegistryKey regKey6 = Registry.Users.OpenSubKey(userName + @"\" + @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\DisallowRun", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))
            {

                regKey6.DeleteValue(fileName, true);
            };

        }


        public void CreateKey()
        {
            if (isExist())
                return;
            else {

                string nameKey = "Explorer";
                using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))

                    regKey.CreateSubKey(nameKey);

                MessageBox.Show("Раздел создан 1");
                using (RegistryKey regKey2 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))

                    regKey2.SetValue("DisallowRun", "1", RegistryValueKind.DWord);
                using (RegistryKey regKey3 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))

                    regKey3.CreateSubKey("DisallowRun");
                MessageBox.Show("Раздел создан 2");

            }
        }

        public void CreateValue(string fileName, string Name)
        {
            string a = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\DisallowRun";
            using (RegistryKey regKey5 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\DisallowRun", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))
            {
           
                regKey5.SetValue(Name, fileName);
            };

            MessageBox.Show("Программа добавлена. Перезагрузите компьютер");
        }


        public void DeleteValue(string fileName)
        {
            using (RegistryKey regKey6 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\DisallowRun", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))
            {

                regKey6.DeleteValue(fileName, true);
            };

        }


    }
}
