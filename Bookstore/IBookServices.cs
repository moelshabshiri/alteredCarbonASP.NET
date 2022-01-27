using System.Collections.Generic;

namespace Bookstore.Core
{
    public interface IBookServices
    {
        List<Book> GetBooks();
        Book AddBook(Book book);
        Book GetBook(string id);
        void DeleteBook(string id);
        Book UpdateBook(Book book);

    }
}
