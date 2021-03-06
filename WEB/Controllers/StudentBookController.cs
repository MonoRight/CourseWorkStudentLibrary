﻿using System;
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
    public class StudentBookController : Controller
    {
        IStudentBookService studentBookService;
        IStudentService studentService;
        IBookService bookService;
        UnitOfWork uow;

        public StudentBookController()
        {
            uow = new UnitOfWork();
            studentBookService = new StudentBookService(uow);
            studentService = new StudentService(uow);
            bookService = new BookService(uow);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllRecordings()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<StudentBookDTO, StudentBookViewModel>());
            var mapper = new Mapper(config);
            IEnumerable<StudentBookViewModel> studentBooks = mapper.Map<IEnumerable<StudentBookDTO>, List<StudentBookViewModel>>(studentBookService.GetAllRecordings());

            return View(studentBooks);
        }

        public ActionResult UpdateRecording(StudentBookViewModel studentBookViewModel)
        {
            try
            {
                if (studentBookViewModel.BookId > 0 && studentBookViewModel.StudentId > 0 && studentBookViewModel.BookId > 0)
                {
                    var recording = studentBookService.GetRecordingById(studentBookViewModel.Id);
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<StudentBookViewModel, StudentBookDTO>());
                    var mapper = new Mapper(config);
                    studentBookService.UpdateRecording(mapper.Map<StudentBookViewModel, StudentBookDTO>(studentBookViewModel));
                    return View(studentBookViewModel);
                }
                else
                {
                    return View(studentBookViewModel);
                }
            }
            catch (ValidationException e)
            {
                ModelState.AddModelError(e.Message, e);
                return Content(e.Message);
            }
        }

        public ActionResult DeleteRecordingById(int? id)
        {
            try
            {
                studentBookService.DeleteRecordingById(id);
            }
            catch (ValidationException e)
            {
                return Content(e.Message);
            }

            return View();
        }

        [HttpPost]
        public ActionResult CreateRecording(StudentBookViewModel studentBookViewModel)
        {
            try
            {
                if (studentBookViewModel.BookId > 0 && studentBookViewModel.StudentId > 0 && studentBookViewModel.Id > 0)
                {
                    StudentBookDTO studentBookDTO = new StudentBookDTO()
                    {
                        BookId = studentBookViewModel.BookId,
                        StudentId = studentBookViewModel.StudentId
                    };
                    try
                    {
                        studentBookService.CreateRecording(studentBookDTO);
                        return Content("<h2>Замовлення додано</h2>");
                    }
                    catch(ValidationException e)
                    {
                        ModelState.AddModelError(e.Message, e);
                        return Content(e.Message);
                    }
                }
                else
                {
                    return View(studentBookViewModel);
                }
            }
            catch (ValidationException e)
            {
                ModelState.AddModelError(e.Message, e);
                return Content(e.Message);
            }
        }

        public ActionResult CreateRecording(int? id)
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<StudentBookDTO, StudentBookViewModel>());
                var mapper = new Mapper(config);
                var studentBook = mapper.Map<StudentBookDTO, StudentBookViewModel>(studentBookService.GetRecordingById(id));
                return View(studentBook);
            }
            catch (ValidationException e)
            {
                ModelState.AddModelError(e.Message, e);
                return Content(e.Message);
            }
        }

        public ActionResult GetRecordingById(int? id)
        {
            StudentBookViewModel studentBookViewModel;
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<StudentBookDTO, StudentBookViewModel>());
                var mapper = new Mapper(config);
                try
                {
                    studentBookViewModel = mapper.Map<StudentBookDTO, StudentBookViewModel>(studentBookService.GetRecordingById(id));

                    IEnumerable<BookDTO> books = bookService.GetAllBooks();
                    IEnumerable<StudentBookDTO> studentBooks = studentBookService.GetAllRecordings();
                    IEnumerable<StudentDTO> students = studentService.GetAllStudents();

                    var result = from book in books
                                 join studentBook in studentBooks on book.Id equals studentBook.BookId
                                 select new {Id = studentBook.Id, BookId = studentBook.BookId, StudentId = studentBook.StudentId, BookName = book.Name, Author = book.Author};

                    var result1 = from student in students
                                  join resul in result on student.Id equals resul.StudentId
                                  select new {Id = resul.Id, StudentId = resul.StudentId, BookId = resul.BookId, StudentName = student.Name, Surname = student.Surname, BookName = resul.BookName, Author = resul.Author };

                    var returnstudentbook = result1.FirstOrDefault(t => t.Id == id);

                    TakeStudentBookViewModel studentbookreturn = new TakeStudentBookViewModel();
                    if (returnstudentbook != null)
                    {
                        studentbookreturn.Id = returnstudentbook.Id;
                        studentbookreturn.StudentId = returnstudentbook.StudentId;
                        studentbookreturn.BookId = returnstudentbook.BookId;
                        studentbookreturn.StudentName = returnstudentbook.StudentName;
                        studentbookreturn.BookName = returnstudentbook.BookName;
                        studentbookreturn.Surname = returnstudentbook.Surname;
                        studentbookreturn.Author = returnstudentbook.Author;
                    }

                    ViewBag.StudentBook = studentbookreturn;
                }
                catch (ValidationException e)
                {
                    ModelState.AddModelError(e.Message, e);
                    return Content(e.Message);
                }
                return View();
            }
            catch (ValidationException e)
            {
                ModelState.AddModelError(e.Message, e);
                return Content(e.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            studentBookService.Dispose();
            base.Dispose(disposing);
        }
    }
}