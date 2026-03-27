using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using BookStoreService.Models;

namespace BookStoreService
{
    [ServiceContract]
    public interface IBookStoreService
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "books")]
        List<Book> GetAllBooks();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "books/{id}")]
        Book GetBookById(string id);

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "books")]
        Book AddBook(Book book);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "books/{id}")]
        Book UpdateBook(string id, Book book);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "books/{id}")]
        bool DeleteBook(string id);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "books/search?query={query}")]
        List<Book> SearchBooks(string query);
    }
}