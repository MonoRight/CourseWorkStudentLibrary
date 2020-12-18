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
    public class StudentController : Controller
    {
        IStudentService studentService;
        IBookService bookService;
        IStudentBookService studentBookService;
        UnitOfWork uow;

        public StudentController()
        {
            uow = new UnitOfWork();
            studentService = new StudentService(uow);
            bookService = new BookService(uow);
            studentBookService = new StudentBookService(uow);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllStudents()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<StudentDTO, StudentViewModel>());
            var mapper = new Mapper(config);
            IEnumerable<StudentViewModel> students = mapper.Map<IEnumerable<StudentDTO>, List<StudentViewModel>>(studentService.GetAllStudents());

            return View(students);
        }

        public ActionResult GetAllStudentsSortedByName()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<StudentDTO, StudentViewModel>());
            var mapper = new Mapper(config);
            IEnumerable<StudentViewModel> students = mapper.Map<IEnumerable<StudentDTO>, List<StudentViewModel>>(studentService.GetAllStudentsSortedByName());

            return View(students);
        }

        public ActionResult GetAllStudentsSortedBySurname()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<StudentDTO, StudentViewModel>());
            var mapper = new Mapper(config);
            IEnumerable<StudentViewModel> students = mapper.Map<IEnumerable<StudentDTO>, List<StudentViewModel>>(studentService.GetAllStudentsSortedBySurname());

            return View(students);
        }

        public ActionResult GetAllStudentsSortedByGroup()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<StudentDTO, StudentViewModel>());
            var mapper = new Mapper(config);
            IEnumerable<StudentViewModel> students = mapper.Map<IEnumerable<StudentDTO>, List<StudentViewModel>>(studentService.GetAllStudentsSortedByGroup());

            return View(students);
        }

        public ActionResult GetStudentById(int? id)
        {
            StudentViewModel student;
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<StudentDTO, StudentViewModel>());
                var mapper = new Mapper(config);
                try
                {
                    student = mapper.Map<StudentDTO, StudentViewModel>(studentService.GetStudentById(id));

                    IEnumerable<BookDTO> books = bookService.GetAllBooks();
                    IEnumerable<StudentBookDTO> studentBooks = studentBookService.GetAllRecordings();
                    IEnumerable<StudentDTO> students = studentService.GetAllStudents();

                    var result = from stud in students
                                 join studentBook in studentBooks on stud.Id equals studentBook.StudentId
                                 select new { BookId = studentBook.BookId, StudentId = studentBook.StudentId };

                    var result1 = from book in books
                                  join resul in result on book.Id equals resul.BookId
                                  select new { BookId = resul.BookId, Name = book.Name, Author = book.Author, StudentId = resul.StudentId };

                    var returnbook = result1.Where(t => t.StudentId == id);

                    List<TakeBookViewModel> bookreturn = new List<TakeBookViewModel>();

                    foreach(var t in returnbook)
                    {
                        TakeBookViewModel data = new TakeBookViewModel();
                        data.BookId = t.BookId;
                        data.StudentId = t.StudentId;
                        data.Name = t.Name;
                        data.Author = t.Author;
                        bookreturn.Add(data);
                    }

                    ViewBag.Books = bookreturn;
                }
                catch (ValidationException e)
                {
                    ModelState.AddModelError(e.Message, e);
                    return Content(e.Message);
                }
                return View(student);
            }
            catch (ValidationException e)
            {
                ModelState.AddModelError(e.Message, e);
                return Content(e.Message);
            }
        }

        public ActionResult CreateStudent(StudentViewModel studentViewModel)
        {
            try
            {
                if (studentViewModel.Name != null && studentViewModel.Surname != null && studentViewModel.GroupName != null)
                {
                    StudentDTO student = new StudentDTO()
                    {
                        Name = studentViewModel.Name,
                        Surname = studentViewModel.Surname,
                        GroupName = studentViewModel.GroupName
                    };

                    studentService.CreateStudent(student);
                    return View(studentViewModel);
                }
                else
                {
                    return View(studentViewModel);
                }
            }
            catch (ValidationException e)
            {
                ModelState.AddModelError(e.Message, e);
                return Content(e.Message);
            }
        }

        public ActionResult DeleteStudentById(int? id)
        {
            try
            {
                studentService.DeleteStudentById(id);
            }
            catch (ValidationException e)
            {
                return Content(e.Message);
            }

            return View();
        }

        public ActionResult UpdateStudent(StudentViewModel studentViewModel)
        {
            try
            {
                if (studentViewModel.Name != null && studentViewModel.Surname != null && studentViewModel.GroupName != null && studentViewModel.Id > 0)
                {
                    StudentDTO student = studentService.GetStudentById(studentViewModel.Id);
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<StudentViewModel, StudentDTO>());
                    var mapper = new Mapper(config);
                    studentService.UpdateStudent(mapper.Map<StudentViewModel, StudentDTO>(studentViewModel));
                    return View(studentViewModel);
                }
                else
                {
                    return View(studentViewModel);
                }
            }
            catch (ValidationException e)
            {
                ModelState.AddModelError(e.Message, e);
                return Content(e.Message);
            }
        }

        public ActionResult Search(string name, string surname, string groupName)
        {
            IEnumerable<StudentDTO> studentsDTO;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<StudentDTO, StudentViewModel>());
            var mapper = new Mapper(config);
            if (!String.IsNullOrEmpty(name))
            {
                studentsDTO = studentService.GetStudentsByName(name); 
                IEnumerable<StudentViewModel> students = mapper.Map<IEnumerable<StudentDTO>, List<StudentViewModel>>(studentsDTO);
                return View(students);
            }
            else if(!String.IsNullOrEmpty(surname))
            {
                studentsDTO = studentService.GetStudentsBySurname(surname);
                IEnumerable<StudentViewModel> students = mapper.Map<IEnumerable<StudentDTO>, List<StudentViewModel>>(studentsDTO);
                return View(students);
            }
            else if(!String.IsNullOrEmpty(groupName))
            {
                studentsDTO = studentService.GetStudentsByGroup(groupName);
                IEnumerable<StudentViewModel> students = mapper.Map<IEnumerable<StudentDTO>, List<StudentViewModel>>(studentsDTO);
                return View(students);
            }
            else
            {
                studentsDTO = studentService.GetAllStudents();
                
                IEnumerable<StudentViewModel> students = mapper.Map<IEnumerable<StudentDTO>, List<StudentViewModel>>(studentsDTO);
                return View(students);
            }
        }

        protected override void Dispose(bool disposing)
        {
            studentService.Dispose();
            base.Dispose(disposing);
        }
    }
}