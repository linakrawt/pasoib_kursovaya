using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using Microsoft.Win32;
using System.Security.AccessControl;

namespace kursovaya_pasoib
{
    public partial class Form1 : Form
    {
        public static SQLiteConnection DB;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            DB = new SQLiteConnection("Data Source=pasoib_bd.db; Version=3");
            SQLiteCommand command = Form1.DB.CreateCommand();



            string CommandText = "SELECT * FROM programs";
            SQLiteDataAdapter sqlda = new SQLiteDataAdapter(CommandText, DB);
            DataTable dt = new DataTable();

            using (dt = new DataTable())
            {
                sqlda.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Width = 505;
            }
            DB.Close();

        }
        string filePath;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var fileContent = string.Empty;
               // var filePath = string.Empty;

                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = "c:\\";
                    //  openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.Filter = "All files (*.exe)|*.exe";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        filePath = openFileDialog.FileName;

                        //Read the contents of the file into a stream
                        var fileStream = openFileDialog.OpenFile();

                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            fileContent = reader.ReadToEnd();
                        }
                    }
                }
               // textBox1.Text = filePath;
                string fileName = Path.GetFileName(filePath);
                textBox1.Text = fileName;
               

                //if (Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer") != null) //если путь существует
                //{
                //    MessageBox.Show("Путь Существует!");
                //}
                //else
                //{
                //   // string nameSubKey = "Friends";
                //    string nameFriend = "Explorer";
                //         using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))
                //        // if (regKey != null)  {
                //          regKey.CreateSubKey(nameFriend);
                //        //   }
                //        MessageBox.Show("Раздел создан 1");
                //    using (RegistryKey regKey2 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))

                //    regKey2.SetValue("DisallowRun", "1", RegistryValueKind.DWord);
                //    using (RegistryKey regKey3 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))

                //        regKey3.CreateSubKey("DisallowRun");
                //    MessageBox.Show("Раздел создан 2");

                //    using (RegistryKey regKey4 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\DisallowRun", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))
                //        regKey4.SetValue("1", fileName);

                //    MessageBox.Show("Параметр добавлен");

                //}

                //RegistryWork a = new RegistryWork();
                //a.CreateKey();
                //a.CreateValue(fileName, fileName);

            }
           
            catch  (Exception ex)
            {
                //  MessageBox.Show("Ошибка!");
                MessageBox.Show($"Ошибка! {ex.Message}");
            }
        }

   

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {

                DB = new SQLiteConnection("Data Source=pasoib_bd.db; Version=3");
                try
                {
                    DB.Open();
                    BD authorization = new BD();
                    bool check = authorization.Check(filePath);
                    if (check == false)
                    {
                        RegistryWork a = new RegistryWork();
                        a.CreateKey();
                        a.CreateValue(textBox1.Text, textBox1.Text);
                        BD writePath = new BD();
                        bool check2 = writePath.WritePath(filePath);
                    }
                    else MessageBox.Show("Данная программа уже в списке!");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }
            else MessageBox.Show("Вы ничего не выбрали!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
    
            DB = new SQLiteConnection("Data Source=pasoib_bd.db; Version=3");

            using (DB)
            {
                DB.Open(); //Открываем подключение
                SQLiteCommand com = new SQLiteCommand("DELETE FROM programs WHERE Path=@path", DB);

                try
                {
                    if (dataGridView1.Rows.Count == 1)
                        MessageBox.Show("База данных пуста! Удалять нечего!");

                    else
                    {
                        string pathName = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();


                        com.Parameters.AddWithValue("@path", pathName);
                        com.ExecuteNonQuery();
                        RegistryWork a = new RegistryWork();
                        a.DeleteValue(Path.GetFileName(pathName));
                        MessageBox.Show("Приложение удалено из списка! Перезагрузите компьютер");
                        dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                    }
                }

                catch
                {
                    MessageBox.Show("Удалить не удалось!");
                }
            }
       }
            
            

            
 
        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            DB = new SQLiteConnection("Data Source=pasoib_bd.db; Version=3");
            SQLiteCommand command = Form1.DB.CreateCommand();



            string CommandText = "SELECT * FROM programs";
            SQLiteDataAdapter sqlda = new SQLiteDataAdapter(CommandText, DB);
            DataTable dt = new DataTable();

            using (dt = new DataTable())
            {


                sqlda.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            DB.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
