using FirebirdSql.Data.FirebirdClient;
using System.IO;




namespace BookDatabase
{

    public sealed class Database
    {
        // string for connection
        private string connString { get; set; }

        private static Database instance = null;
        public static Database Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Database();
                }
                return instance;
            }
        }
        private Database()
        {
            // getting path to database file
            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
            string dbPath = Path.Combine(path, "db", "Book_db.fdb");
            this.connString = $"User=SYSDBA;Password=masterkey;Database={dbPath};DataSource=localhost;Port=3050;Dialect=3;Charset=UTF8;";

        }

        // Methods for selecting procedures in SQL
        public List<Tuple<string, DateTime, string>> SelectAllAuthors()
        {
            return SelectAuthorWithSearch("");
        }

        public List<Tuple<byte[], string, string, string>> SelectAllBooks()
        {
           return SelectBooksWithSearch("");
        }

        public List<Tuple<byte[], string, string, string>> SelectBooksByFilters(string? author, string? genres, string? languages, string? publishers)
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
                        List<Tuple<byte[], string, string, string>> list = new List<Tuple<byte[], string, string, string>> { };

                        while (reader.Read())
                        {
                            var photo = reader["Photo"];
                            byte[]? photoByte = photo as byte[];
                            var names = (string)reader["Name"];
                            var authors = (string)reader["Fullname"];
                            var genre = (string)reader["Genre"];

                            Tuple<byte[], string, string, string> tup = new Tuple<byte[], string, string, string>(photoByte!, names, authors, genre);
                            list.Add(tup);
                        }
                        con.Close();
                        return list;
                    }
                }
            }
        }

        public void InsertAuthor(string name,string surname, string country, DateTime dateOfBirth, string aboutAuthor)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (FbCommand cmd = new FbCommand("INSERT_AUTHOR", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("ABOUTAUTHOR", (object?)aboutAuthor ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("COUNTRY", (object?)country ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("DATEOFBIRTH", dateOfBirth);
                    cmd.Parameters.AddWithValue("SURNAME", (object?)surname ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("NAME", (object?)name ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
        }

        public List<Tuple<string, DateTime, string>> SelectAuthorsByFilters(string? Countries)
        {

            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("select * from select_authors_with_filters(@Countries_list)", con))
                {

                    cmd.Parameters.AddWithValue("@Countries_list", (object?)Countries ?? DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<Tuple<string, DateTime, string>> list = new List<Tuple<string, DateTime, string>>();
                        while (reader.Read())
                        {
                            var name = (string)reader["Name"];
                            var surname = (string)reader["Surname"];
                            var dateOfBirth = (DateTime)reader["DateOfBirth"];
                            var country = (string)reader["Country"];

                            Tuple<string, DateTime, string> tup = new Tuple<string, DateTime, string>( name + surname, dateOfBirth, country );
                            list.Add(tup);
                        }
                        return list;
                    }
                }
            }
        }

        public List<Tuple<byte[], string, string, string>> SelectBooksWithSearch(string search)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("select * from select_book_with_search(@search)", con))
                {
                    cmd.Parameters.AddWithValue("@search", (object?) search ?? DBNull.Value);

                    using(var reader = cmd.ExecuteReader())
                    {
                        List < Tuple<byte[], string, string, string> > list = new List<Tuple<byte[], string, string, string>> { };

                        while (reader.Read())
                        {
                            var photo = reader["Photo"];
                            byte[]? photoByte = photo as byte[];
                            var names = (string)reader["Name"];
                            var authors = (string)reader["Fullname"];
                            var genre = (string)reader["Genre"];

                            Tuple<byte[], string, string, string> tup = new Tuple<byte[], string, string, string>(photoByte!, names, authors, genre);
                            list.Add(tup);


                        }
                        con.Close();
                        return list;
                    }
                }
            }
        }

        public List<Tuple<string, DateTime, string>> SelectAuthorWithSearch(string search)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("select * from select_author_with_search(@search)", con))
                {
                    cmd.Parameters.AddWithValue("@search", (object?)search ?? DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<Tuple<string, DateTime, string>> list = new List<Tuple<string, DateTime, string>> { };
                        while (reader.Read())
                        {
                            var names = (string)reader["Name"];
                            var surname = (string)reader["Surname"];
                            string name = names + " " + surname;
                            var dateOfBirth = (DateTime)reader["DateOfBirth"];
                            var country = (string)reader["Country"];

                            Tuple<string, DateTime, string> tup = new Tuple<string, DateTime, string> ( name, dateOfBirth, country );
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

        public void SelectAuthor(string authorName)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();
                using (var cmd = new FbCommand("select * from select_author(@Name_input)", con))
                {
                    cmd.Parameters.AddWithValue("@Name_input", (object?)authorName ?? DBNull.Value);

                    using ( var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            var names = reader["Name"];
                            var surname = reader["Surname"];
                            string name = names + " " + surname;
                            var dateOfBirth = reader["DATE_OF_BIRTH"];
                            var country = reader["Country"];
                            var aboutAuthor = reader["About_Author"];
                            Console.WriteLine("tohle je autor: " + name + " " + dateOfBirth + " " + country + " " + aboutAuthor + "\n");
                        }
                    }
                }
            }
        }

        public List<Tuple<byte[], string, string, string>> OrderBooks(string argument, string way)
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
                        List<Tuple<byte[], string, string, string>> list = new List<Tuple<byte[], string, string, string>>();
                        while (reader.Read())
                        {
                            var photo = reader["Photo"];
                            byte[]? photoByte = photo as byte[];
                            var name = (string)reader["Name"];
                            var author = (string)reader["Fullname"];
                            var genre = (string)reader["genre"];

                            Tuple<byte[], string, string, string> tup = new Tuple<byte[], string, string, string>(photoByte!, name, author, genre);
                            list.Add(tup);
                        }
                        con.Close();
                        return list;
                    }

                }
                
            }
        }

        public List<string> SelectTableByName(string tableName)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                if (tableName != "Authors")
                {
                    using (var cmd = new FbCommand("SELECT * FROM SELECT_TABLE_BY_NAME(@tableName)", con))
                    {
                        cmd.Parameters.AddWithValue("@tableName", (object?)tableName ?? DBNull.Value);

                        using (var reader = cmd.ExecuteReader())
                        {
                            List<string> list = new List<string>();

                            while (reader.Read())
                            {
                                var name = reader["Name"];
                                list.Add(name?.ToString() ?? "");
                            }
                            con.Close();
                            return list;
                        }

                    }
                }
                else
                {
                    using (var cmd = new FbCommand("SELECT * FROM SELECT_TABLE_BY_NAME_AUTHORS", con))
                    { 
                        using (var reader = cmd.ExecuteReader())
                        {
                            List<string> list = new List<string>();

                            while (reader.Read())
                            {
                                var name = reader["Authors_Name"];
                                var surname = reader["Authors_Surname"];
                                var fullname = name + " " + surname;
                                list.Add(fullname?.ToString() ?? "");
                            }
                            con.Close();
                            return list;
                        }

                    }
                }


            }
            
        }

        public void InsertBookOldAuthor(
            byte[]? photo,
            string? typeOfBook,
            short? lengthOfBook,
            string? ean,
            string? isbn,
            string? publisher,
            string? language,
            string? rating,
            string? description,
            string? genre,
            string? bookName,
            string? authorName)
        {
            string newName = authorName.Split(" ")[0];

            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (FbCommand cmd = new FbCommand("INSERT_BOOK_OLD_AUTHOR", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("PHOTO", (object?)photo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("TYPE_OF_BOOK", (object?)typeOfBook ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("LENGTH_OF_BOOK", (object?)lengthOfBook ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("EAN", (object?)ean ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("ISBN", (object?)isbn ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("PUBLISHER", (object?)publisher ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("LANGUAGE", (object?)language ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("RATING", (object?)rating ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("DESCRIPTION", (object?)description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("GENRE", (object?)genre ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("BOOK_NAME", (object?)bookName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("AUTHOR_NAME", (object?)newName ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
        }
    }
    
}

