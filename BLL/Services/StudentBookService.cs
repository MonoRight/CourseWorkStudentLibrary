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
    public class StudentBookService  :IStudentBookService
    {
        public IUnitOfWork Database { get; set; }
        public StudentBookService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public IEnumerable<StudentBookDTO> GetAllRecordings()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<StudentBook, StudentBookDTO>());
            var mapper = new Mapper(config);
            var studentBooks = mapper.Map<IEnumerable<StudentBook>, List<StudentBookDTO>>(Database.StudentBooks.GetAll());
            return studentBooks;
        }

        public StudentBookDTO GetRecordingById(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Замовлення відсутнє з вказаним ID");
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<StudentBook, StudentBookDTO>());
            var mapper = new Mapper(config);
            var studentBook = Database.StudentBooks.Get(id.Value);

            if (studentBook == null)
            {
                throw new ValidationException("Замовлення відсутнє з вказаним ID");
            }

            return mapper.Map<StudentBook, StudentBookDTO>(studentBook);
        }

        public void UpdateRecording(StudentBookDTO studentBookDTO)
        {
            if (BookLimit(studentBookDTO))
            {
                throw new ValidationException("Ліміт на кількість книг перевищенно! Одночасно студент може мати всього 4 книги!");
            }
            if (BookInLibrary(studentBookDTO))
            {
                throw new ValidationException("Книга вже видана, зачекайте поки її повернуть до бібліотеки!");
            }

            StudentBook studentBook = Database.StudentBooks.Get(studentBookDTO.Id);
            if (studentBook == null)
            {
                throw new ValidationException("Замовлення відсутнє з вказаним ID");
            }

            studentBook.BookId = studentBookDTO.BookId;
            studentBook.StudentId = studentBookDTO.StudentId;

            Database.StudentBooks.Update(studentBook);
        }

        public void DeleteRecordingById(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Замовлення відсутнє з вказаним ID");
            }
            Database.StudentBooks.Delete(id.Value);
            Database.Save();
        }

        public void CreateRecording(StudentBookDTO studentBookDTO)
        {
            if(BookLimit(studentBookDTO))
            {
                throw new ValidationException("Ліміт на кількість книг перевищенно! Одночасно студент може мати всього 4 книги!");
            }
            if(BookInLibrary(studentBookDTO))
            {
                throw new ValidationException("Книга вже видана, зачекайте поки її повернуть до бібліотеки!");
            }

            Student student = Database.Students.Get(studentBookDTO.StudentId);
            Book book = Database.Books.Get(studentBookDTO.BookId);

            if (student == null && book == null)
            {
                throw new ValidationException("Студента або Книги не знайдено в базі!");
            }

            StudentBook studentBook = new StudentBook()
            {
                BookId = book.Id,
                StudentId = student.Id
            };

            Database.StudentBooks.Create(studentBook);
            Database.Save();
        }

        private bool BookLimit(StudentBookDTO studentBookDTO)
        {
            bool flag = false;
            int studentId = studentBookDTO.StudentId;
            int count = 0;

            IEnumerable<Student> students = Database.Students.GetAll();
            IEnumerable<StudentBook> studentbooks = Database.StudentBooks.GetAll();

            var result = from student in students
                         join studentbook in studentbooks on student.Id equals studentbook.StudentId
                         select new { StudentId = studentbook.StudentId, Name = student.Name, Surname = student.Surname, Group = student.GroupName, BookId = studentbook.BookId };
            
            foreach(var c in result)
            {
                if (count < 4)
                {
                    if (c.StudentId == studentId)
                    {
                        count++;
                    }
                }
                else
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }

        private bool BookInLibrary(StudentBookDTO studentBookDTO)
        {
            bool flag = false;
            int bookId = studentBookDTO.BookId;

            IEnumerable<StudentBook> studentbooks = Database.StudentBooks.GetAll();

            foreach(var c in studentbooks)
            {
                if(c.BookId == bookId)
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }
        

        public void Dispose()
        {
            Database.Dispose();
        }

    }
}
