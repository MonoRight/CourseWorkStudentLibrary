using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IBookService
    {
        IEnumerable<BookDTO> GetAllBooks();
        BookDTO GetBookById(int? id);
        IEnumerable<BookDTO> GetAllBooksSortedByName();
        IEnumerable<BookDTO> GetAllBooksSortedByAuthor();
        IEnumerable<BookDTO> GetBooksByName(string name);
        IEnumerable<BookDTO> GetBooksByAuthor(string author);
        void UpdateBook(BookDTO bookDTO);
        void DeleteBookById(int? id);
        void CreateBook(BookDTO bookDTO);
        void Dispose();
    }
}
