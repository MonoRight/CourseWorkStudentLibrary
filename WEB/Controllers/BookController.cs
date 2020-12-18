using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using BLL.Services;
using DAL;
using BLL.Infrastructure;
using WEB.Models;

namespace WEB.Controllers
{
    public class BookController : Controller
    {
        IStudentService studentService;
        IStudentBookService studentBookService;
        IBookService bookService;
        UnitOfWork uow;

        public BookController()
        {
            uow = new UnitOfWork();
            bookService = new BookService(uow);
            studentService = new StudentService(uow);
            studentBookService = new StudentBookService(uow);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllBooks()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<BookDTO, BookViewModel>());
            var mapper = new Mapper(config);
            IEnumerable<BookViewModel> books = mapper.Map<IEnumerable<BookDTO>, List<BookViewModel>>(bookService.GetAllBooks());

            return View(books);
        }

        public ActionResult GetAllBooksSortedByName()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<BookDTO, BookViewModel>());
            var mapper = new Mapper(config);
            IEnumerable<BookViewModel> books = mapper.Map<IEnumerable<BookDTO>, List<BookViewModel>>(bookService.GetAllBooksSortedByName());

            return View(books);
        }

        public ActionResult GetAllBooksSortedByAuthor()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<BookDTO, BookViewModel>());
            var mapper = new Mapper(config);
            IEnumerable<BookViewModel> books = mapper.Map<IEnumerable<BookDTO>, List<BookViewModel>>(bookService.GetAllBooksSortedByAuthor());

            return View(books);
        }

        public ActionResult GetBookById(int? id)
        {
            BookViewModel bookViewModel;
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<BookDTO, BookViewModel>());
                var mapper = new Mapper(config);
                try
                {
                    bookViewModel = mapper.Map<BookDTO, BookViewModel>(bookService.GetBookById(id));

                    IEnumerable<BookDTO> books = bookService.GetAllBooks();
                    IEnumerable<StudentBookDTO> studentBooks = studentBookService.GetAllRecordings();
                    IEnumerable<StudentDTO> students = studentService.GetAllStudents();

                    var result = from book in books
                                 join studentBook in studentBooks on book.Id equals studentBook.BookId
                                 select new { BookId = studentBook.BookId, StudentId = studentBook.StudentId };

                    var result1 = from student in students
                                  join resul in result on student.Id equals resul.StudentId
                                  select new { StudentId = resul.StudentId, Name = student.Name, Surname = student.Surname, GroupName = student.GroupName, BookId = resul.BookId };

                    var returnstudent = result1.FirstOrDefault(t => t.BookId == id);

                    TakeStudentViewModel studentreturn = new TakeStudentViewModel();
                    if (returnstudent != null)
                    {
                        studentreturn.BookId = returnstudent.BookId;
                        studentreturn.StudentId = returnstudent.StudentId;
                        studentreturn.Name = returnstudent.Name;
                        studentreturn.Surname = returnstudent.Surname;
                        studentreturn.GroupName = returnstudent.GroupName;
                    }

                    ViewBag.Student = studentreturn;
                }
                catch (ValidationException e)
                {
                    ModelState.AddModelError(e.Message, e);
                    return Content(e.Message);
                }
                return View(bookViewModel);
            }
            catch (ValidationException e)
            {
                ModelState.AddModelError(e.Message, e);
                return Content(e.Message);
            }
        }

        public ActionResult CreateBook(BookViewModel bookViewModel)
        {
            try
            {
                if (bookViewModel.Name != null && bookViewModel.Author != null)
                {
                    BookDTO book = new BookDTO()
                    {
                        Name = bookViewModel.Name,
                        Author = bookViewModel.Author
                    };

                    bookService.CreateBook(book);
                    return View(bookViewModel);
                }
                else
                {
                    return View(bookViewModel);
                }
            }
            catch (ValidationException e)
            {
                ModelState.AddModelError(e.Message, e);
                return Content(e.Message);
            }
        }

        public ActionResult Search(string name, string author)
        {
            IEnumerable<BookDTO> booksDTO;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<BookDTO, BookViewModel>());
            var mapper = new Mapper(config);
            if (!String.IsNullOrEmpty(name))
            {
                booksDTO = bookService.GetBooksByName(name);
                IEnumerable<BookViewModel> books = mapper.Map<IEnumerable<BookDTO>, List<BookViewModel>>(booksDTO);
                return View(books);
            }
            else if (!String.IsNullOrEmpty(author))
            {
                booksDTO = bookService.GetBooksByAuthor(author);
                IEnumerable<BookViewModel> books = mapper.Map<IEnumerable<BookDTO>, List<BookViewModel>>(booksDTO);
                return View(books);
            }
            else
            {
                booksDTO = bookService.GetAllBooks();
                IEnumerable<BookViewModel> books = mapper.Map<IEnumerable<BookDTO>, List<BookViewModel>>(booksDTO);
                return View(books);
            }
        }

        public ActionResult DeleteBookById(int? id)
        {
            try
            {
                bookService.DeleteBookById(id);
            }
            catch (ValidationException e)
            {
                return Content(e.Message);
            }

            return View();
        }

        public ActionResult UpdateBook(BookViewModel bookViewModel)
        {
            try
            {
                if (bookViewModel.Name != null && bookViewModel.Author != null && bookViewModel.Id > 0)
                {
                    var student = bookService.GetBookById(bookViewModel.Id);
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<BookViewModel, BookDTO>());
                    var mapper = new Mapper(config);
                    bookService.UpdateBook(mapper.Map<BookViewModel, BookDTO>(bookViewModel));
                    return View(bookViewModel);
                }
                else
                {
                    return View(bookViewModel);
                }
            }
            catch (ValidationException e)
            {
                ModelState.AddModelError(e.Message, e);
                return Content(e.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            bookService.Dispose();
            base.Dispose(disposing);
        }
    }
}