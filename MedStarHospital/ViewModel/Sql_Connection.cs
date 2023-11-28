using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;

namespace MedStarHospital.ViewModel
{
    public class Sql_Connection
    {
        static SqlConnection connection;
        public static string ConnectionString = @"Data Source=DESKTOP-MCFC3SU;Initial Catalog=Hospital_mvvm; TrustServerCertificate=True ; Integrated Security = True";

        public static bool DBConnection()
        {
            try
            {

                connection = new SqlConnection(ConnectionString);
                connection.Open();
                //MessageBox.Show("Connected");
                connection.Close();
                return true;
            }
            catch
            {
                MessageBox.Show("SQL Connection Failed", "SQL connection", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        public static void sql_connection()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection = new SqlConnection(ConnectionString);
                    connection.Open();
                }

            }
            catch
            {
                MessageBox.Show("Can't Connect DataBase", "SQL connection", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        public static void close_connection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            { connection.Close(); }
        }
        public static SqlConnection getconnection()
        {
            return connection;
        }
        public static bool IsData(string table)
        {
            DBConnection();
            int value = 0;
            connection.Open();
            bool Retrunit = false;
            var SerchQuary = $"select count(*) from {table};";
            var Command = new SqlCommand(SerchQuary, connection);
            var reader = Command.ExecuteReader();

            while (reader.Read())
            {
                value = int.Parse(reader.GetValue(0).ToString());
                if (value != 0 && value != null)
                {
                    Retrunit = true;
                }

            }
            connection.Close();

            return Retrunit;
        }
        public static bool IsData(string table_name, string Column_name, string Check_value)
        {
            bool value = false;
            try
            {
                connection.Open();
                string Query = $"select * from {table_name} where {Column_name} = " + " '" + Check_value + "'  ";
                var command = new SqlCommand(Query, connection);
                var dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    value = true;
                }
                dataReader.Close();
                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                value = false;
            }
            return value;

        }
        public static string SpaficDataISINTable(string table, string Name, string IDColumnname)
        {
            DBConnection();
            string datavalue = "";
            connection.Open();
            bool Retrunit = false;
            var SerchQuary = $"select {table}.{Name} from {table};";
            var Command = new SqlCommand(SerchQuary, connection);
            var reader = Command.ExecuteReader();
            while (reader.Read())
            {

                datavalue = reader.GetValue(0).ToString();
            }
            connection.Close();

            return datavalue;
        }

        public static int DataCount(string table)
        {
            DBConnection();
            int value = 0;
            connection.Open();

            var findQuery = $"select count(*) from {table};";
            var Cmd = new SqlCommand(findQuery, connection);
            var reader = Cmd.ExecuteReader();

            while (reader.Read())
            {
                value = int.Parse(reader.GetValue(0).ToString());
                if (value != 0 && value != null)
                {
                    value = int.Parse(reader.GetValue(0).ToString());
                }

            }

            close_connection();
            return value;


        }

        public static string SpaficDataISINTable(string table, string Name, string IDColumnname, string IDvalue)
        {
            DBConnection();
            string datavalue = "";
            connection.Open();
            bool Retrunit = false;
            var SerchQuary = $"select {table}.{Name} from {table} where {IDColumnname} ={IDvalue}";
            var Command = new SqlCommand(SerchQuary, connection);
            var reader = Command.ExecuteReader();
            while (reader.Read())
            {

                datavalue = reader.GetValue(0).ToString();
            }
            connection.Close();

            return datavalue;

        }

        public static string SpaficDataISINTable1(string table, string Name, string IDColumnname, string IDvalue)
        {
            DBConnection();
            string datavalue = "";
            connection.Open();
            bool Retrunit = false;
            var SerchQuary = $"select {table}.{Name} from {table} where {IDColumnname} ='{IDvalue}'";
            var Command = new SqlCommand(SerchQuary, connection);
            var reader = Command.ExecuteReader();
            while (reader.Read())
            {

                datavalue = reader.GetValue(0).ToString();
            }
            connection.Close();

            return datavalue;

        }


        public static int GetPK(string table_name, string column_name)
        {
            object value = null;
            if (!(connection.State == System.Data.ConnectionState.Open))
            {
                connection = new SqlConnection(ConnectionString);
                connection.Open();
            }
            string Query = $"select max ({column_name}) from {table_name}";
            var command = new SqlCommand(Query, connection);
            var datareader = command.ExecuteReader();
            while(datareader.Read())
            {
                value = datareader.GetValue(0);
            }
            datareader.Close();
            var val = value != DBNull.Value ? value : 0;
            return (int)val+1;
        }
    }
}
