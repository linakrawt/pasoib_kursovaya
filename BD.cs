using System;
using System.Data;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Data.SQLite;

namespace kursovaya_pasoib
{
    public class BD
    {

        SQLiteCommand command = Form1.DB.CreateCommand();

        public bool Check(string Path, string UserName)
        {
            bool check;
            string path = Path;
            string userName = UserName;


            command.Parameters.Add("@Path", DbType.String).Value = path;
            command.Parameters.Add("@userName", DbType.String).Value = userName;


            command.CommandText = "select * from programs where (Path like @Path) and (name_User like @userName)";
            if ((command.ExecuteScalar() != null))
                check = true;
            else check = false;

            return check;
        }


        public bool userCheck(string Data)
        {
            bool check;
            string userName = Data;

            command.Parameters.Add("@Name", DbType.String).Value = userName;

            command.CommandText = "select * from User where Name like @Name";
            if ((command.ExecuteScalar() != null))
                check = true;
            else check = false;

            return check;
        }

        public bool userWrite(string Data)
        {
            SQLiteCommand command = Form1.DB.CreateCommand();
            bool check;
            string userName = Data;

            command.Parameters.Add("@userName", DbType.String).Value = userName;


            command.CommandText = "insert into User(Name) values(@userName)";
            command.ExecuteNonQuery();
            check = true;


            check = false;
            return check;
        }

        public bool WritePath(string Path, string UserName)
        {
            SQLiteCommand command = Form1.DB.CreateCommand();
            bool check;
            string path = Path;
            string userName = UserName;

            command.Parameters.Add("@Path", DbType.String).Value = path;
            command.Parameters.Add("@userName", DbType.String).Value = userName;

            command.CommandText = "insert into programs values(@Path, @userName)";
                command.ExecuteNonQuery();
                check = true;

            
            check = false;
            return check;
        }
    }

}

