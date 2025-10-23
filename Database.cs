using FirebirdSql.Data.FirebirdClient;
using System.IO;
using System.Runtime.InteropServices;



namespace BookDatabase
{

    public class Database
    {
        private string connString {  get; set; }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AttachConsole(uint dwProcessId);

        const uint ATTACH_PARENT_PROCESS = 0x0ffffffff;



        public Database()
        {

            string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string dbPath = Path.Combine(projectDir, "db", "mydatabase.fdb");

            this.connString = $"User=SYSDBA;Password=masterkey;Database={dbPath};DataSource=localhost;Port=3050;Dialect=3;Charset=UTF8;";
            AttachConsole(ATTACH_PARENT_PROCESS);

            SelectAuthorsByFilers(null, null);
            SelectBooksByFilters(null, null, null, null);
            SelectBooksWithSearch("");
            SelectAuthorWithSearch("");
        }

        public void SelectBooksByFilters(string? author, string? genres, string? languages, string? publishers)
        {

            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("select * from select_books_with_filters(@Author_list, @Genre_list, @Language_list, @Publishing_house_list)", con))
                {
                    cmd.Parameters.AddWithValue("@Author_list", (object?)author ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Genre_list", (object?)genres ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Language_list", (object?)languages ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Publishing_house_list", (object?)publishers ?? DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var photo = reader["Photo"];
                            var names = reader["Name"];
                            var authors = reader["Author"];
                            var genre = reader["Genre"];

                            Console.WriteLine("books: " + photo + " " + names + " " + genre + " " + authors + "\n");

                        }
                    }
                }
            }
        }

        public void SelectAuthorsByFilers(string? Names, string? Countries)
        {

            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("select * from select_authors_with_filters(@Name_list, @Countries_list)", con))
                {
                    cmd.Parameters.AddWithValue("@Name_list", (object?)Names ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Countries_list", (object?)Countries ?? DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var name = reader["Name"];
                            var dateOfBirth = reader["DateOfBirth"];
                            var country = reader["Country"];

                            Console.WriteLine("autoři: " + name + " " + dateOfBirth + " " + country + "\n");
                        }
                    }
                }
            }
        }

        public void SelectBooksWithSearch(string search)
        {


            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("select * from select_book_with_search(@search)", con))
                {
                    cmd.Parameters.AddWithValue("@search", (object?) search ?? DBNull.Value);

                    using(var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var photo = reader["Photo"];
                            var names = reader["Name"];
                            var authors = reader["Author"];
                            var genre = reader["Genre"];

                            Console.WriteLine("books ale search: " + photo + " " + names + " " + genre + " " + authors + "\n");
                        }
                    }
                }
            }
        }

        public void SelectAuthorWithSearch(string search)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("select * from select_author_with_search(@search)", con))
                {
                    cmd.Parameters.AddWithValue("@search", (object?)search ?? DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var name = reader["Name"];
                            var dateOfBirth = reader["DateOfBirth"];
                            var country = reader["Country"];

                            Console.WriteLine("autoři ale search: " + name + " " + dateOfBirth + " " + country + "\n");
                        }
                    }
                }
            }
        }
    }
}

