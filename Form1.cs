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
using System.Management;
using System.Diagnostics;
using System.Threading;

namespace kursovaya_pasoib
{
    public partial class Form1 : Form
    {

        void userSH()
        {
            string[] sessionDetails = new string[3];
            string current = "";
            ProcessStartInfo startInfo = new ProcessStartInfo("cmd", "/c QUERY SESSION " + comboBox1.Text)
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            Process getsess = Process.Start(startInfo);

            getsess.OutputDataReceived += (x, y) => current += y.Data;
            getsess.BeginOutputReadLine();
            getsess.WaitForExit();

            int a = Process.GetCurrentProcess().SessionId;

            if (current.Length != 0)
            {
                if (a.ToString() != current.Substring(119, 4).Replace(" ", ""))
                {

                    sessionDetails[0] = current.Substring(119, 4);

                    Process logoff = new Process();
                    ProcessStartInfo startInfo2 = new ProcessStartInfo();
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C LOGOFF " + sessionDetails[0];
                    logoff.StartInfo = startInfo;
                    logoff.Start();
                }
            }
        }


        public static SQLiteConnection DB;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {




            string[] mas = { };

            try
            {
                ManagementObjectSearcher usersSearcher = new ManagementObjectSearcher(@"SELECT * FROM Win32_UserAccount");
                ManagementObjectCollection users = usersSearcher.Get();

                var localUsers = users.Cast<ManagementObject>().Where(
                    u => (bool)u["LocalAccount"] == true &&
                         (bool)u["Disabled"] == false &&
                         (bool)u["Lockout"] == false &&
                         int.Parse(u["SIDType"].ToString()) == 1 &&
                         u["Name"].ToString() != "HomeGroupUser$");



                foreach (ManagementObject user in localUsers)
                {

                    Array.Resize(ref mas, mas.Length + 1);
                    mas[mas.Length - 1] = user["Name"].ToString();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + ex.Message);
            }


            comboBox1.Items.AddRange(mas);


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


            }

            catch (Exception ex)
            {
                //  MessageBox.Show("Ошибка!");
                MessageBox.Show($"Ошибка! {ex.Message}");
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && comboBox1.SelectedIndex > -1)
            {


                userSH(); Thread.Sleep(3000);
                if (Environment.UserName != comboBox1.Text)
                {
                    RegistryWork b = new RegistryWork();
                    b.LoadKey(comboBox1.Text); b.LoadKey(comboBox1.Text);
                };

                

                DB = new SQLiteConnection("Data Source=pasoib_bd.db; Version=3");
                try
                {
                    DB.Open();
                    BD authorization = new BD();
                    bool check1 = authorization.userCheck(comboBox1.Text);
                    if (check1 == false)
                    {

                        BD userWrite = new BD();
                        bool check2 = authorization.userWrite(comboBox1.Text);
                    }



                    bool check3 = authorization.Check(filePath, comboBox1.Text);
                    if (check3 == false)
                    {
                        if (Environment.UserName != comboBox1.Text)
                        {
                            RegistryWork c = new RegistryWork();
                            c.CreateUserKey(comboBox1.Text);
                            c.CreateUserValue(textBox1.Text, textBox1.Text, comboBox1.Text);
                            BD writePath = new BD();
                            bool check4 = writePath.WritePath(filePath, comboBox1.Text);
                        }
                        else
                        {
                            RegistryWork c = new RegistryWork();
                            c.CreateKey();
                            c.CreateValue(textBox1.Text, textBox1.Text);
                            BD writePath = new BD();
                            bool check4 = writePath.WritePath(filePath, comboBox1.Text);
                        }
                    }
                    else MessageBox.Show("Данная программа уже в списке!");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }
            else MessageBox.Show("Вы не выбрали пользователя или не указали путь!");
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
                        

                        if (Environment.UserName != comboBox1.Text)
                        {
                            userSH(); Thread.Sleep(3000);


                            if (Environment.UserName != comboBox1.Text)
                            {
                                RegistryWork b = new RegistryWork();
                                b.LoadKey(comboBox1.Text); b.LoadKey(comboBox1.Text);
                            };

                            Thread.Sleep(2000);
                            RegistryWork c = new RegistryWork();
                            c.CreateUserKey(comboBox1.Text);
                            
                            c.DeleteUserValue(Path.GetFileName(pathName), comboBox1.Text);
                            com.ExecuteNonQuery();
                            MessageBox.Show("Приложение удалено из списка! Перезагрузите компьютер");
                            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                        }
                        else
                        {
                            RegistryWork a = new RegistryWork();
                            a.DeleteValue(Path.GetFileName(pathName));
                            com.ExecuteNonQuery();
                            MessageBox.Show("Приложение удалено из списка! Перезагрузите компьютер");
                            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                        }




                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Удалить не удалось!"+ ex.Message);
                }
            }
       }
            
            

            
 
        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

            DB = new SQLiteConnection("Data Source=pasoib_bd.db; Version=3");
            SQLiteCommand command = DB.CreateCommand();

            string userName = comboBox1.Text;



            SQLiteCommand command2 = DB.CreateCommand();
            command2.Parameters.Add("@userName", DbType.String).Value = userName;
            command2.CommandText = "SELECT Path FROM programs where name_User like @userName";

            SQLiteDataAdapter sqlda = new SQLiteDataAdapter();
            sqlda.SelectCommand = command2;
            DataTable dt = new DataTable();

            using (dt = new DataTable())
            {
                sqlda.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Width = 505;
            }
            DB.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {



        }

    

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            DB = new SQLiteConnection("Data Source=pasoib_bd.db; Version=3");
            SQLiteCommand command = DB.CreateCommand();

            string userName = comboBox1.Text;



            SQLiteCommand command2 = DB.CreateCommand();
            command2.Parameters.Add("@userName", DbType.String).Value = userName;
            command2.CommandText= "SELECT Path FROM programs where name_User like @userName";


            SQLiteDataAdapter sqlda = new SQLiteDataAdapter();
            sqlda.SelectCommand = command2;
            DataTable dt = new DataTable();
            
            using (dt = new DataTable())
            {
                sqlda.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Width = 505;
            }
            DB.Close();

        }

      
    }
    }

