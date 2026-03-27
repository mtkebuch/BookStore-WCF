using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using BookStoreService.Data;
using BookStoreService.Models;

namespace BookStoreService
{
    public class BookStoreService : IBookStoreService
    {
        public List<Book> GetAllBooks()
        {
            try
            {
                using (var context = new BookStoreContext())
                {
                    return context.Books.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(
                    "Failed to retrieve books: " + ex.Message,
                    HttpStatusCode.InternalServerError);
            }
        }

        public Book GetBookById(string id)
        {
            if (!int.TryParse(id, out int bookId))
                throw new WebFaultException<string>(
                    "Invalid ID format.",
                    HttpStatusCode.BadRequest);

            try
            {
                using (var context = new BookStoreContext())
                {
                    var book = context.Books.Find(bookId);
                    if (book == null)
                        throw new WebFaultException<string>(
                            "Book not found.",
                            HttpStatusCode.NotFound);
                    return book;
                }
            }
            catch (WebFaultException<string>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(
                    "Error: " + ex.Message,
                    HttpStatusCode.InternalServerError);
            }
        }

        public Book AddBook(Book book)
        {
            if (book == null)
                throw new WebFaultException<string>(
                    "Book data is empty.",
                    HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(book.Title))
                throw new WebFaultException<string>(
                    "Title is required.",
                    HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(book.Author))
                throw new WebFaultException<string>(
                    "Author is required.",
                    HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(book.ISBN))
                throw new WebFaultException<string>(
                    "ISBN is required.",
                    HttpStatusCode.BadRequest);

            if (book.Price <= 0)
                throw new WebFaultException<string>(
                    "Price must be greater than 0.",
                    HttpStatusCode.BadRequest);

            if (book.Stock < 0)
                throw new WebFaultException<string>(
                    "Stock cannot be negative.",
                    HttpStatusCode.BadRequest);

            try
            {
                using (var context = new BookStoreContext())
                {
                    bool isbnExists = context.Books
                        .Any(b => b.ISBN == book.ISBN);

                    if (isbnExists)
                        throw new WebFaultException<string>(
                            "A book with this ISBN already exists.",
                            HttpStatusCode.Conflict);

                    context.Books.Add(book);
                    context.SaveChanges();
                    return book;
                }
            }
            catch (WebFaultException<string>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(
                    "Failed to add book: " + ex.Message,
                    HttpStatusCode.InternalServerError);
            }
        }

        public Book UpdateBook(string id, Book book)
        {
            if (!int.TryParse(id, out int bookId))
                throw new WebFaultException<string>(
                    "Invalid ID format.",
                    HttpStatusCode.BadRequest);

            if (book == null)
                throw new WebFaultException<string>(
                    "Book data is empty.",
                    HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(book.Title))
                throw new WebFaultException<string>(
                    "Title is required.",
                    HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(book.Author))
                throw new WebFaultException<string>(
                    "Author is required.",
                    HttpStatusCode.BadRequest);

            // ✅ ეს დაემატა
            if (string.IsNullOrWhiteSpace(book.ISBN))
                throw new WebFaultException<string>(
                    "ISBN is required.",
                    HttpStatusCode.BadRequest);

            if (book.Price <= 0)
                throw new WebFaultException<string>(
                    "Price must be greater than 0.",
                    HttpStatusCode.BadRequest);

            try
            {
                using (var context = new BookStoreContext())
                {
                    var existing = context.Books.Find(bookId);
                    if (existing == null)
                        throw new WebFaultException<string>(
                            "Book not found.",
                            HttpStatusCode.NotFound);

                    existing.Title = book.Title;
                    existing.Author = book.Author;
                    existing.ISBN = book.ISBN;
                    existing.Price = book.Price;
                    existing.Stock = book.Stock;
                    existing.PublishedDate = book.PublishedDate;

                    context.SaveChanges();
                    return existing;
                }
            }
            catch (WebFaultException<string>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(
                    "Failed to update book: " + ex.Message,
                    HttpStatusCode.InternalServerError);
            }
        }

        public bool DeleteBook(string id)
        {
            if (!int.TryParse(id, out int bookId))
                throw new WebFaultException<string>(
                    "Invalid ID format.",
                    HttpStatusCode.BadRequest);

            try
            {
                using (var context = new BookStoreContext())
                {
                    var book = context.Books.Find(bookId);
                    if (book == null)
                        throw new WebFaultException<string>(
                            "Book not found.",
                            HttpStatusCode.NotFound);

                    context.Books.Remove(book);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (WebFaultException<string>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(
                    "Failed to delete book: " + ex.Message,
                    HttpStatusCode.InternalServerError);
            }
        }

        public List<Book> SearchBooks(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new WebFaultException<string>(
                    "Search query is empty.",
                    HttpStatusCode.BadRequest);

            try
            {
                using (var context = new BookStoreContext())
                {
                    string lowerQuery = query.ToLower();
                    return context.Books
                        .Where(b => b.Title.ToLower().Contains(lowerQuery) ||
                                    b.Author.ToLower().Contains(lowerQuery) ||
                                    b.ISBN.Contains(query))
                        .ToList();
                }
            }
            catch (WebFaultException<string>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(
                    "Search failed: " + ex.Message,
                    HttpStatusCode.InternalServerError);
            }
        }
    }
}