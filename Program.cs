using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDemo
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    //GetDynamicAuthors(1, 3);
        //    //InsertSingleAuthorUsingDynamicParameters();
        //    //GetAuthorAndTheirBooksSPUsingDynamicParameters(1);
        //    //GetAuthorAndTheirBooksSPUsingDynamicParameters(2);
        //    //DeleteMultipleAuthors();
        //    //GetAllBooks();
        //    //GetAllAuthors();

        //    //in oprator
        //    //GetAuthors(1, 3);

        //}


        //for async method
        static async Task Main(string[] args)
        {
            //InsertSingleAuthorAsync();
            var authors = await GetAllAuthorsAsync();

            foreach (var author in authors)
            {
                Console.WriteLine(author.FName + " " + author.LastName);
            }
        }




        /// <summary>
        /// Insert Single Row
        /// </summary>
        private static void InsertSingleAuthor()
        {
            var ConnectionString = @"Data Source=.;Initial Catalog=BookStoreContext;Integrated Security=True;";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                Author author = new Author()
                {
                    FName = "William",
                    LastName = "Shakespeare"
                };

                string sqlQuery = "INSERT INTO Authors (FirstName, LastName) VALUES(@FName, @LastName)";

                int rowsAffected = db.Execute(sqlQuery, author);
            }
        }


        /// <summary>
        /// Insert Multiple Row
        /// </summary>
        private static void InsertMultipleAuthors()
        {
            var ConnectionString = @"Data Source=.;Initial Catalog=BookStoreContext;Integrated Security=True;";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string sqlQuery = "INSERT INTO Authors (FirstName, LastName) VALUES(@FirstName, @LastName)";

                int rowsAffected = db.Execute(sqlQuery,
                    new[]
                    {
                new {FirstName = "Emily", LastName = "Dickinson"},
                new {FirstName = "Leo", LastName = "Tolstoy"},
                new {FirstName = "Rabindranath", LastName = "Tagore"}
                    }
                );
            }
        }

        /// <summary>
        /// Get All Data
        /// </summary>

        private static void GetAllAuthors()
        {
            var ConnectionString = @"Data Source=.;Initial Catalog=BookStoreContext;Integrated Security=True;";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                List<Author> authors = db.Query<Author>("SELECT Id, FirstName FName,  LastName FROM Authors").ToList();

                foreach (var author in authors)
                {
                    Console.WriteLine(author.FName + " " + author.LastName);
                }
            }
        }

        /// <summary>
        /// Get All Book Data
        /// </summary>
        private static void GetAllBooks()
        {
            var ConnectionString = @"Data Source=.;Initial Catalog=BookStoreContext;Integrated Security=True;";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                List<Book> books = db.Query<Book>("SELECT * FROM Books").ToList();

                foreach(var book in books)
                {
                    Console.WriteLine("Id: {0} \t Title: {1} \t Category: {2}", book.Id, book.Title, book.Category);
                }
            }

        }

        /// <summary>
        /// Single Update Method
        /// </summary>
        private static void UpdateSingleBook()
        {
            string sql = "UPDATE Books SET Category = @Category WHere Id = @Id;";

            var ConnectionString = @"Data Source=.;Initial Catalog=BookStoreContext;Integrated Security=True;";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                int rowsAffected = db.Execute(sql, new { Id = 3, Category = "Education" });
            }
        }

        private static void UpdateMultipleBooks()
        {
            string sql = "UPDATE Books SET Category = @Category Where Id =@Id;";
            var ConnectionString = @"Data Source=.;Initial Catalog=BookStoreContext;Integrated Security=True;";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                int rowsAffected = db.Execute(sql,
                    new[]
                    {
                        new {Id = 4, Category = "Math"},
                        new {Id = 3, Category = "Math"},
                    }
                    );
            }

        }


        /// <summary>
        /// Delete one row
        /// </summary>
        private static void DeleteSingleAuthor()
        {
            string sql = "DELETE FROM Authors WHERE Id = @Id;";
            var ConnectionString = @"Data Source=.;Initial Catalog=BookStoreContext;Integrated Security=True;";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                int rowsAffected = db.Execute(sql, new { Id = 4 });
            }
        }


        /// <summary>
        /// Delete Multiple Authors
        /// </summary>
        private static void DeleteMultipleAuthors()
        {
            string sql = "DELETE FROM Authors WHERE Id = @Id;";

            var ConnectionString = @"Data Source=.;Initial Catalog=BookStoreContext;Integrated Security=True;";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                int rowsAffected = db.Execute(sql,
                    new[]
                    {
                        new {Id = 5},
                        new {Id = 6},
                    }
                    );
            }
        }

        /// <summary>
        /// Get Author And Their Books
        /// </summary>
        /// <param name="id"></param>
        private static void GetAuthorAndTheirBooks(int id)
        {
            string sql = "SELECT Id, FirstName FName,  LastName FROM Authors WHERE Id = @Id;" + "SELECT * FROM Books WHERE AuthorId = @Id;";

            var ConnectionString = @"Data Source=.;Initial Catalog=BookStoreContext;Integrated Security=True;";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                using (var results = db.QueryMultiple(sql, new {Id = id }))
                {
                    var author = results.Read<Author>().SingleOrDefault();
                    var books = results.Read<Book>().ToList();

                    if (author != null && books != null)
                    {
                        author.Books = books;

                        Console.WriteLine(author.FName + " " + author.LastName);

                        foreach(var book in author.Books)
                        {
                            Console.WriteLine("\t Title: {0} \t Category: {1}", book.Title, book.Category);
                        }
                    }

                }
            }

        }

        /// <summary>
        /// GetAuthorAndThierBooks by Store PROCEDURE
        /// </summary>
        /// <param name="id"></param>

        private static void GetAuthorAndThierBooksSP(int id)
        {
            string sql = "GetAuthor";
            var ConnectionString = @"Data Source=.;Initial Catalog=BookStoreContext;Integrated Security=True;";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                using (var results = db.QueryMultiple(sql, new { Id = id }, commandType: CommandType.StoredProcedure))
                {
                    var author = results.Read<Author>().SingleOrDefault();
                    var books = results.Read<Book>().ToList();

                    if (author != null && books != null)
                    {
                        author.Books = books;

                        Console.WriteLine(author.FName + " " + author.LastName);

                        foreach (var book in author.Books)
                        {
                            Console.WriteLine("\t Title: {0} \t  Category: {1}", book.Title, book.Category);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get Author And Their Books SP Using Dynamic Parameters
        /// </summary>
        /// <param name="id"></param>
        private static void GetAuthorAndTheirBooksSPUsingDynamicParameters(int id)
        {
            string sql = "GetAuthor";

            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@Id", id);

            var ConnectionString = @"Data Source=.;Initial Catalog=BookStoreContext;Integrated Security=True;";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                using (var results = db.QueryMultiple(sql, parameter, commandType: CommandType.StoredProcedure))
                {
                    var author = results.Read<Author>().SingleOrDefault();
                    var books = results.Read<Book>().ToList();

                    if (author != null && books != null)
                    {
                        author.Books = books;

                        Console.WriteLine(author.FName + " " + author.LastName);

                        foreach (var book in author.Books)
                        {
                            Console.WriteLine("\t Title: {0} \t  Category: {1}", book.Title, book.Category);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// inserts an author into the database using dynamic parameters
        /// </summary>
        private static void InsertSingleAuthorUsingDynamicParameters()
        {
            var ConnectionString = @"Data Source=.;Initial Catalog=BookStoreContext;Integrated Security=True;";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                DynamicParameters parameter = new DynamicParameters();

                parameter.Add("@FirstName", "Raginee");
                parameter.Add("@LastName", "Gupta");

                string sqlQuery = "INSERT INTO Authors (FirstName, LastName) VALUES(@FirstName, @LastName)";

                int rowsAffected = db.Execute(sqlQuery, parameter);
            }
        }

        /// <summary>
        /// IN Operator Support
        /// </summary>
        /// <param name="ids"></param>
        private static void GetAuthors(params int[] ids)
        {
            var ConnectionString = @"Data Source=.;Initial Catalog=BookStoreContext;Integrated Security=True;";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                List<Author> authors =
                    db.Query<Author>("SELECT Id, FirstName FName,  LastName FROM Authors WHERE Id IN @Ids", new { Ids = ids })
                    .ToList();

                foreach (var author in authors)
                {
                    Console.WriteLine(author.FName + " " + author.LastName);
                }
            }
        }

        /// <summary>
        /// same functionality using dynamic objects instead of using the concrete Author class
        /// </summary>
        /// <param name="ids"></param>
        private static void GetDynamicAuthors(params int[] ids)
        {
            var ConnectionString = @"Data Source=.;Initial Catalog=BookStoreContext;Integrated Security=True;";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                List<dynamic> authors =
                    db.Query("SELECT * FROM Authors WHERE Id IN @Ids", new { Ids = ids })
                    .ToList();

                foreach (var author in authors)
                {
                    Console.WriteLine(author.FirstName + " " + author.LastName);
                }
            }
        }

        /// <summary>
        /// Multi Mapping
        /// </summary>
        private static void GetAuthorWithBooks()
        {
            string sql = "SELECT * FROM Authors A INNER JOIN Books B ON A.Id = B.AuthorId";

            var ConnectionString = @"Data Source=.;Initial Catalog=BookStoreContext;Integrated Security=True;";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                var authorDictionary = new Dictionary<int, Author>();

                var authors = db.Query<Author, Book, Author>(
                    sql,
                    (author, book) =>
                    {
                        Author authorEntry;

                        if (!authorDictionary.TryGetValue(author.Id, out authorEntry))
                        {
                            authorEntry = author;
                            authorEntry.Books = new List<Book>();
                            authorDictionary.Add(authorEntry.Id, authorEntry);
                        }

                        authorEntry.Books.Add(book);
                        return authorEntry;
                    },
                    splitOn: "Id")
                .Distinct()
                .ToList();

                foreach (var author in authors)
                {
                    Console.WriteLine(author.FName + " " + author.LastName);

                    foreach (var book in author.Books)
                    {
                        Console.WriteLine("\t Title: {0} \t  Category: {1}", book.Title, book.Category);
                    }
                }
            }
        }


        /// <summary>
        /// Async Operations
        /// </summary>
        /// <returns></returns>
        private async static Task<List<Author>> GetAllAuthorsAsync()
        {
            var ConnectionString = @"Data Source=.;Initial Catalog=BookStoreContext;Integrated Security=True;";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                IEnumerable<Author> results = await db.QueryAsync<Author>("SELECT Id, FirstName FName,  LastName FROM Authors");
                return results.ToList();
            }
        }


        /// <summary>
        /// inserts a single author asynchronously using ExecuteAsync
        /// </summary>
        private static async void InsertSingleAuthorAsync()
        {
            var ConnectionString = @"Data Source=.;Initial Catalog=BookStoreContext;Integrated Security=True;";
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                Author author = new Author()
                {
                    FName = "Gunjan",
                    LastName = "Shakespeare"
                };

                string sqlQuery = "INSERT INTO Authors (FirstName, LastName) VALUES(@FName, @LastName)";

                int rowsAffected = await db.ExecuteAsync(sqlQuery, author);
            }
        }

    }
}
