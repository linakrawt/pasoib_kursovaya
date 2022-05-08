using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
using System.Security.AccessControl;
using System.Windows.Forms;

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
            //long intName;

            //using (RegistryKey regKey4 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\DisallowRun", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))
            //{
            //    string stringName = String.Join("", regKey4.GetValueNames());
            //    MessageBox.Show(stringName);

            //    intName = Int64.Parse(stringName);
             
            //};

   //MessageBox.Show(intName.ToString());     
   //         ++intName;
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
