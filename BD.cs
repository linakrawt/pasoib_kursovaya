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
        public bool Check(string Data)
        {
            bool check;
            string path = Data;

            command.Parameters.Add("@Path", DbType.String).Value = path;
           
            command.CommandText = "select * from programs where Path like @Path";
            if ((command.ExecuteScalar() != null))
                check = true;
            else check = false;

            return check;
        }

        public bool WritePath(string Data)
        {
            SQLiteCommand command = Form1.DB.CreateCommand();
            bool check;
            string path = Data;

            command.Parameters.Add("@Path", DbType.String).Value = path;

       
                command.CommandText = "insert into programs(Path) values(@Path)";
                command.ExecuteNonQuery();
                check = true;

            
            check = false;
            return check;
        }
    }

}

