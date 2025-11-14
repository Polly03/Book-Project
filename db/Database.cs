using BookDatabase.Models;
using FirebirdSql.Data.FirebirdClient;
using System.IO;
using System.Windows.Media.Imaging;





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
        public List<Authors> SelectAllAuthors()
        {
            return SelectAuthorWithSearch("");
        }

        public List<BooksData> SelectAllBooks()
        {
            return SelectBooksWithSearch("");
        }

        public List<BooksData> SelectBooksByFilters(string? author, string? genres, string? languages, string? publishers)
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
                        List<BooksData> list = new List<BooksData> { };

                        while (reader.Read())
                        {
                            var photo = reader["Photo"];
                            byte[]? photoByte = photo as byte[];
                            var names = (string)reader["Name"];
                            var authors = (string)reader["Fullname"];
                            var genre = (string)reader["Genre"];

                            list.Add(new BooksData(GetBM((byte[])photo), names, authors, genre));

                        }
                        con.Close();
                        return list;
                    }
                }
            }
        }

        public void InsertAuthor(string name, string surname, string country, DateTime dateOfBirth, string aboutAuthor)
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

        public List<Authors> SelectAuthorsByFilters(string? Countries)
        {

            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("select * from select_authors_with_filters(@Countries_list)", con))
                {

                    cmd.Parameters.AddWithValue("@Countries_list", (object?)Countries ?? DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<Authors> list = new List<Authors>();

                        while (reader.Read())
                        {
                            var nameT = (string)reader["Name"];
                            var surnameT = (string)reader["Surname"];
                            var dateOfBirthT = (DateTime)reader["DateOfBirth"];
                            var countryT = (string)reader["Country"];

                            list.Add(new Authors(nameT + " " + surnameT, dateOfBirthT.ToString(), countryT));

                        }

                        return list;
                    }
                }
            }
        }

        public List<BooksData> SelectBooksWithSearch(string search)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("select * from select_book_with_search(@search)", con))
                {
                    cmd.Parameters.AddWithValue("@search", (object?)search ?? DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<BooksData> list = new List<BooksData> { };

                        while (reader.Read())
                        {
                            var photo = reader["Photo"];
                            byte[]? photoByte = photo as byte[];
                            var names = (string)reader["Name"];
                            var authors = (string)reader["Fullname"];
                            var genre = (string)reader["Genre"];


                            list.Add(new BooksData(GetBM((byte[])photo), names, authors, genre));


                        }
                        con.Close();
                        return list;
                    }
                }
            }
        }

        public List<Authors> SelectAuthorWithSearch(string search)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("select * from select_author_with_search(@search)", con))
                {
                    cmd.Parameters.AddWithValue("@search", (object?)search ?? DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<Authors> list = new List<Authors> { };
                        while (reader.Read())
                        {
                            var names = (string)reader["Name"];
                            var surname = (string)reader["Surname"];
                            string name = names + " " + surname;
                            var dateOfBirth = (DateTime)reader["DateOfBirth"];
                            var country = (string)reader["Country"];

                            list.Add(new Authors(name, dateOfBirth.ToString(), country));

                        }
                        con.Close();
                        return list;
                    }
                }
            }
        }

        public Book SelectBook(string name, int size = 0)
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
                            Book book = new Book
                            {
                                Id = (int)reader["Id"],
                                Name = reader["Name"] as string,
                                Author = reader["author"] as string,
                                Genre = reader["Genre"] as string,
                                Type = reader["BOOK_type"] as string,
                                Langueage = reader["language"] as string,
                                Length = (short)reader["length_OF_BOOK"],
                                Publisher = reader["publisher"] as string,
                                Description = reader["Description"] as string,
                                EAN = reader["ean"] as string,
                                ISBN = reader["isbn"] as string,
                                Rating = reader["rating"] as string,
                                Image = GetBM((byte[])reader["Photo"], size)
                            };
                            return book;
                        }
                     
                    }
                }
            }
            return null;
 
        }

        public Authors SelectAuthor(string authorName)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();
                using (var cmd = new FbCommand("select * from select_author(@Name_input)", con))
                {
                    cmd.Parameters.AddWithValue("@Name_input", (object?)authorName ?? DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
    

                        while (reader.Read())
                        {
                            var id = (int)reader["Id"];
                            var names = reader["Name"];
                            var dateOfBirth = reader["DATE_OF_BIRTH"];
                            var country = reader["Country"];
                            var aboutAuthor = reader["About_Author"];

                            Authors author = new Authors((string)names, (string)dateOfBirth, (string)country);
                            author.Id = id;
                            author.AboutAuthor = (string)aboutAuthor;
                            return author;
                        }
                  
                    }
                }
            }
            return null;
        }

        public List<Authors> OrderAuthors(string argument, string way)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("select * from order_Authors(@way, @argument)", con))
                {
                    cmd.Parameters.AddWithValue("@argument", (object?)argument ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@way", (object?)way ?? DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<Authors> list = new List<Authors>();
                        while (reader.Read())
                        {
                            var fullname = (string)reader["Fullname"];
                            var DateOfBirth = (DateTime)reader["DateOfBirth"];
                            var Country = (string)reader["Country"];

                            list.Add(new Authors(fullname, DateOfBirth.ToString(), Country));
                        }
                        con.Close();
                        return list;
                    }

                }

            }
        }

        public List<BooksData> OrderBooks(string argument, string way)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("select * from order_books(@way, @argument)", con))
                {
                    cmd.Parameters.AddWithValue("@argument", (object?)argument ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@way", (object?)way ?? DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<BooksData> list = new List<BooksData>();
                        while (reader.Read())
                        {
                            var photo = reader["Photo"];
                            byte[]? photoByte = photo as byte[];
                            var name = (string)reader["Name"];
                            var author = (string)reader["Fullname"];
                            var genre = (string)reader["genre"];

                            list.Add(new BooksData(GetBM((byte[])photo), name, author, genre));
                        }
                        con.Close();
                        return list;
                    }

                }

            }
        }

        public List<GeneralModel> SelectNameByTableName(string tableName)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

              
                using (var cmd = new FbCommand("SELECT * FROM SELECT_NAME_BY_TABLE_NAME(@tableName)", con))
                {
                    cmd.Parameters.AddWithValue("@tableName", (object?)tableName ?? DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<GeneralModel> list = new List<GeneralModel>();

                        while (reader.Read())
                        {
                            var name = (string)reader["Name"];
                            list.Add(new GeneralModel(name));
                        }
                        con.Close();
                        return list;
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

        public void UpdateBook(
            int bookId,
            byte[] photo,
            string typeOfBook,
            short lengthOfBook,
            string ean,
            string isbn,
            string publisher,
            string language,
            string rating,
            string description,
            string genre,
            string bookName,
            string authorName)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("execute procedure UPDATE_BOOK(" +
                                               "@BOOK_ID, @PHOTO, @TYPE_OF_BOOK, @LENGTH_OF_BOOK, @EAN, @ISBN, @PUBLISHER, @LANGUAGE, " +
                                               "@RATING, @DESCRIPTION, @GENRE, @BOOK_NAME, @AUTHOR_NAME)", con))
                {
                    cmd.Parameters.AddWithValue("@BOOK_ID", bookId);
                    cmd.Parameters.AddWithValue("@PHOTO", (object?)photo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TYPE_OF_BOOK", (object?)typeOfBook ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LENGTH_OF_BOOK", lengthOfBook);
                    cmd.Parameters.AddWithValue("@EAN", (object?)ean ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ISBN", (object?)isbn ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PUBLISHER", (object?)publisher ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LANGUAGE", (object?)language ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@RATING", (object?)rating ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DESCRIPTION", (object?)description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@GENRE", (object?)genre ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@BOOK_NAME", (object?)bookName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@AUTHOR_NAME", (object?)authorName ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateAuthor(int authorId, string name, string surname, DateTime dateOfBirth, string countryName, string aboutAuthor)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand(
                    "execute procedure UPDATE_AUTHOR(" +
                    "@AUTHOR_ID, @NAME, @SURNAME, @DATE_OF_BIRTH, @COUNTRY_NAME, @ABOUT_AUTHOR)", con))
                {
                    cmd.Parameters.AddWithValue("@AUTHOR_ID", authorId);
                    cmd.Parameters.AddWithValue("@NAME", (object?)name ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SURNAME", (object?)surname ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DATE_OF_BIRTH", dateOfBirth);
                    cmd.Parameters.AddWithValue("@COUNTRY_NAME", (object?)countryName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ABOUT_AUTHOR", (object?)aboutAuthor ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public int DeleteAuthor(string authorName)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("select * from delete_author(@AUTHOR_NAME)", con))
                {
                    cmd.Parameters.AddWithValue("@AUTHOR_NAME", (object?)authorName ?? DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        int status = -1;

                        if (reader.Read())
                        {
                            status = Convert.ToInt32(reader["STATUS"]);
                        }

                        return status;
                    }
                }
            }
        }

        public void DeleteBook(string bookName, int bookId)
        {
            using (FbConnection con = new FbConnection(connString))
            {
                con.Open();

                using (var cmd = new FbCommand("execute procedure delete_book(@NAME_OF_BOOK, @ID_OF_BOOK)", con))
                {
                    cmd.Parameters.AddWithValue("@NAME_OF_BOOK", (object?)bookName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ID_OF_BOOK", bookId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // decoding byte[] data into bitmapImage
        private BitmapImage GetBM(byte[] data, int size = 0)
        {
            var bitmap = new BitmapImage();
            using (var ms = new MemoryStream(data))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;

                if (size == 0)
                {
                    bitmap.DecodePixelWidth = 125;
                    bitmap.DecodePixelHeight = 150;
                }
                else if (size == 1)
                {
                    bitmap.DecodePixelWidth = 520;
                    bitmap.DecodePixelHeight = 800;
                }

                bitmap.StreamSource = ms;
                bitmap.EndInit();
            }

            bitmap.Freeze();
            return bitmap;
        }
    }
}

