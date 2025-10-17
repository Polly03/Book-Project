using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Sqlite;
using System.Data;
using System.IO;
using Microsoft.Data.SqlClient;
using System.Windows.Controls;
using System.Windows;


namespace BookDatabase
{
    public class Database
    {
        public Database()
        {
            SetupDatabase();
        }

        public void SetupDatabase()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Books.db");

            using (var conn = new SqliteConnection($"Data Source={path}"))
            {
                conn.Open();
                string command = "CREATE TABLE IF NOT EXISTS Country(ID integer primary key autoincrement, Name varchar(64));";
                var createTable = new SqliteCommand(command, conn);
                createTable.ExecuteNonQuery();
                command = "CREATE TABLE IF NOT EXISTS Genre(ID integer primary key autoincrement, Name varchar(64));";
                createTable = new SqliteCommand(command, conn);
                createTable.ExecuteNonQuery();
                command = "CREATE TABLE IF NOT EXISTS Language(ID integer primary key autoincrement, Name varchar(64));";
                createTable = new SqliteCommand(command, conn);
                createTable.ExecuteNonQuery();
                command = "CREATE TABLE IF NOT EXISTS Publisher(ID integer primary key autoincrement, Name varchar(64));";
                createTable = new SqliteCommand(command, conn);
                createTable.ExecuteNonQuery();
                command = "CREATE TABLE IF NOT EXISTS Book_type(ID integer primary key autoincrement, Name varchar(32));";
                createTable = new SqliteCommand(command, conn);
                createTable.ExecuteNonQuery();

                command = "Select Count(*) from Genre";
                int n;
                using (var select = new SqliteCommand(command, conn))
                {
                    using (SqliteDataReader reader = select.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            n = reader.GetInt32(0);
                            if (n == 0)
                            {
                                conn.Close();
                                SetupData();
                            }
                        }
                    }


                }

                conn.Close();

            }

        }



        public List<string> SelectFromTable(string selectCom)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Books.db");
            List<string> selected = new List<string>();

            using (var conn = new SqliteConnection($"Data Source={path}"))
            {
                conn.Open();
                using (var select = new SqliteCommand(selectCom, conn))
                {
                    using (SqliteDataReader reader = select.ExecuteReader())
                    {
                        int n = reader.FieldCount;
                        while (reader.Read())
                        {
                            List<string> list = new List<string>();
                            for (int i = 0; i < n; i++)
                            {

                                list.Add(reader.GetString(i));
                            }
                            string s = string.Join(",", list.ToArray());
                            selected.Add(s);
                        }
                    }
                }
                conn.Close();
            }
            return selected;
        }

        public void SetupData()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Books.db");

            using (var conn = new SqliteConnection($"Data Source={path}"))
            {
                conn.Open();
                string command = "INSERT INTO Country (Name) VALUES('Česká republika'),('Slovensko'),('Německo'),('Rakousko'),('Francie'),('Itálie'),('Španělsko'),('Portugalsko'),('Polsko'),('Maďarsko'),('Nizozemsko'),('Belgie'),('Švýcarsko'),('Velká Británie'),('Irsko'),('Norsko'),('Švédsko'),('Dánsko'),('Finsko'),('Rusko'),('Ukrajina'),('USA'),('Kanada'),('Austrálie'),('Čína'),('Japonsko'),('Jižní Korea'),('Indie'),('Mexiko'),('Brazílie');";
                var createTable = new SqliteCommand(command, conn);
                createTable.ExecuteNonQuery();
                command = "INSERT INTO Genre (Name) VALUES ('Román'),('Detektivka'),('Fantasy'),('Sci-fi'),('Historický');";
                createTable = new SqliteCommand(command, conn);
                createTable.ExecuteNonQuery();
                command = "INSERT INTO Language (Name) VALUES('Čeština'),('Slovenština'),('Němčina'),('Angličtina'),('Francouzština'),('Italština'),('Španělština'),('Portugalština'),('Polština'),('Maďarština'),('Nizozemština'),('Vlámština'),('Švýcarská němčina'),('Norština'),('Švédština'),('Dánština'),('Finština'),('Ruština'),('Ukrajinština'),('Čínština'),('Japonština'),('Korejština'),('Hindština'),('Arabština'),('Hebrejština'),('Řečtina'),('Turečtina'),('Latina'),('Esperanto');";
                createTable = new SqliteCommand(command, conn);
                createTable.ExecuteNonQuery();
                command = "INSERT INTO Publisher (Name) VALUES ('Nakladatelství Albatros'),('Euromedia Group'),('Host'),('Fragment'),('XYZ vydavatelství');";
                createTable = new SqliteCommand(command, conn);
                createTable.ExecuteNonQuery();
                command = "Insert into Book_type(Name) VALUES('Měkká'),('Tvrdá'),('Polotvrdá'),('audio'),('elektronická');";
                createTable = new SqliteCommand(command, conn);
                createTable.ExecuteNonQuery();
                conn.Close();

            }

        }


    }
}

