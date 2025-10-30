using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;



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
            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
            string dbPath = Path.Combine(path, "db", "Book_db.fdb");

            this.connString = $"User=SYSDBA;Password=masterkey;Database={dbPath};DataSource=localhost;Port=3050;Dialect=3;Charset=UTF8;";
            AttachConsole(ATTACH_PARENT_PROCESS);

            SelectAuthorsByFilers(null, null);
            SelectBooksByFilters(null, null, null, null);
            SelectBooksWithSearch("");
            SelectAuthorWithSearch("");
            SelectBook("1984");
            SelectAuthor("Franz Kafka");
            SelectAllAuthors();
            SelectAllBooks();
            OrderBooks("books.name", "asc");

        }
        public List<Tuple<string, string, string>> SelectAllAuthors()
        {
            return SelectAuthorWithSearch("");
        }

        public List<Tuple<Byte[], string, string, string>> SelectAllBooks()
        {
           return SelectBooksWithSearch("");
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

        public List<Tuple<Byte[], string, string, string>> SelectBooksWithSearch(string search)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("select * from select_book_with_search(@search)", con))
                {
                    cmd.Parameters.AddWithValue("@search", (object?) search ?? DBNull.Value);

                    using(var reader = cmd.ExecuteReader())
                    {
                        List < Tuple<Byte[], string, string, string> > list = new List<Tuple<Byte[], string, string, string>> { };

                        while (reader.Read())
                        {
                            var photo = reader["Photo"] == DBNull.Value ? null : (byte[])reader["Photo"];
                            var names = (string)reader["Name"];
                            var authors = (string)reader["Author"];
                            var genre = (string)reader["Genre"];

                            Tuple<Byte[], string, string, string> tup = new Tuple<byte[], string, string, string>(photo, names, authors, genre);
                            list.Add(tup);


                        }
                        con.Close();
                        return list;
                    }
                }
            }
        }

        public List<Tuple<string, string, string>> SelectAuthorWithSearch(string search)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("select * from select_author_with_search(@search)", con))
                {
                    cmd.Parameters.AddWithValue("@search", (object?)search ?? DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<Tuple<string, string, string>> list = new List<Tuple<string, string, string>> { };
                        while (reader.Read())
                        {
                            var name = (string)reader["Name"];
                            var dateOfBirth = (string)reader["DateOfBirth"];
                            var country = (string)reader["Country"];

                            Tuple<string, string, string> tup = new Tuple<string, string, string> ( name, dateOfBirth, country );
                            list.Add(tup);
                            
                        }
                        con.Close();
                        return list;
                    }
                }
            }
        }

        public void SelectBook(string name)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("select * from select_book(@name)", con))
                {
                    cmd.Parameters.AddWithValue("@name", (object?)name ?? DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var rating = reader["rating"];
                            var type = reader["BOOK_type"];
                            var publisher = reader["publisher"];
                            var genre = reader["Genre"];
                            var author = reader["author"];
                            var language = reader["language"];
                            var ean = reader["ean"];
                            var isbn = reader["isbn"];
                            var photo = reader["photo"];
                            var description = reader["Description"];
                            var names = reader["Name"];

                            Console.WriteLine(
                                    $"Book info:\n" +
                                    $"Name: {names}\n" +
                                    $"Author: {author}\n" +
                                    $"Genre: {genre}\n" +
                                    $"Type: {type}\n" +
                                    $"Language: {language}\n " +
                                    $"Publisher: {publisher}\n" +
                                    $"EAN: {ean}\n" +
                                    $"ISBN: {isbn}\n" +
                                    $"Rating: {rating}\n" +
                                    $"Description: {description}\n" +
                                    $"Photo: {photo}\n");
                        }
                    }
                }
            }
        }

        public void SelectAuthor(string name)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();
                using (var cmd = new FbCommand("select * from select_author(@Name_input)", con))
                {
                    cmd.Parameters.AddWithValue("@Name_input", (object?)name ?? DBNull.Value);

                    using ( var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            var names = reader["Name"];
                            var dateOfBirth = reader["DATE_OF_BIRTH"];
                            var country = reader["Country"];
                            var aboutAuthor = reader["About_Author"];
                            Console.WriteLine("tohle je autor: " + names + " " + dateOfBirth + " " + country + " " + aboutAuthor + "\n");
                        }
                    }
                }
            }
        }

        public void OrderBooks(string argument, string way)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("select * from order_books(@way, @argument)", con))
                {
                    cmd.Parameters.AddWithValue("@argument", (object?)argument ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@way", (object?)way ?? DBNull.Value);

                    using( var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var photo = reader["photo"];
                            var name = reader["Name"];
                            var author = reader["author"];
                            var genre = reader["genre"];

                            Console.WriteLine("order: " + photo + " " + name + " " + genre + " " + author + "\n");
                        }
                    }

                }
                con.Close();
            }
        }

        public List<string> SelectTableByName(string tableName)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("SELECT * FROM SELECT_TABLE_BY_NAME(@tableName)", con))
                {
                    cmd.Parameters.AddWithValue("@tableName", (object?)tableName ?? DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<string> list = new List<string>();

                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            list.Add(name);
                        }
                        con.Close();
                        return list;
                    }

                }
                
                

            }
            
        }
    }
    
}

