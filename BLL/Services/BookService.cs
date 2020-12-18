using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Infrastructure;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using AutoMapper;

namespace BLL.Services
{
    public class BookService :IBookService
    {
        public IUnitOfWork Database { get; set; }
        public BookService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public IEnumerable<BookDTO> GetAllBooks()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookDTO>());
            var mapper = new Mapper(config);
            var books = mapper.Map<IEnumerable<Book>, List<BookDTO>>(Database.Books.GetAll());
            return books;
        }
        public IEnumerable<BookDTO> GetBooksByName(string name)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookDTO>());
            var mapper = new Mapper(config);
            var books = mapper.Map<IEnumerable<Book>, List<BookDTO>>(Database.Books.Find(book => book.Name == name));
            return books;
        }

        public IEnumerable<BookDTO> GetBooksByAuthor(string author)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookDTO>());
            var mapper = new Mapper(config);
            var books = mapper.Map<IEnumerable<Book>, List<BookDTO>>(Database.Books.Find(book => book.Author == author));
            return books;
        }

        public IEnumerable<BookDTO> GetAllBooksSortedByName()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookDTO>());
            var mapper = new Mapper(config);
            var books = mapper.Map<IEnumerable<Book>, List<BookDTO>>(Database.Books.GetAll()).OrderBy(book => book.Name);
            return books;
        }
        public IEnumerable<BookDTO> GetAllBooksSortedByAuthor()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookDTO>());
            var mapper = new Mapper(config);
            var books = mapper.Map<IEnumerable<Book>, List<BookDTO>>(Database.Books.GetAll()).OrderBy(book => book.Author);
            return books;
        }

        public BookDTO GetBookById(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Книга відсутня з вказаним ID");
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookDTO>());
            var mapper = new Mapper(config);
            var book = Database.Books.Get(id.Value);

            if (book == null)
            {
                throw new ValidationException("Книга відсутня з вказаним ID");
            }

            return mapper.Map<Book, BookDTO>(book);
        }

        public void UpdateBook(BookDTO bookDTO)
        {
            Book book = Database.Books.Get(bookDTO.Id);
            if (book == null)
            {
                throw new ValidationException("Книга відсутня з вказаним ID");
            }

            book.Name = bookDTO.Name;
            book.Author = bookDTO.Author;

            Database.Books.Update(book);
        }

        public void DeleteBookById(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Книга відсутня з вказаним ID");
            }
            Database.Books.Delete(id.Value);
            Database.Save();
        }

        public void CreateBook(BookDTO bookDTO)
        {
            Book book = new Book()
            {
                Name = bookDTO.Name,
                Author = bookDTO.Author,
 
            };

            Database.Books.Create(book);
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
