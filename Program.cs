﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DapperDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            DeleteMultipleAuthors();
            GetAllBooks();
            GetAllAuthors();
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

    }
}
