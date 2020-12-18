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
    public class StudentService :IStudentService
    {
        public IUnitOfWork Database { get; set; }
        public StudentService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public IEnumerable<StudentDTO> GetAllStudents()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>());
            var mapper = new Mapper(config);
            var students = mapper.Map<IEnumerable<Student>, List<StudentDTO>>(Database.Students.GetAll());
            return students;
        }

        public IEnumerable<StudentDTO> GetAllStudentsSortedByName()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>());
            var mapper = new Mapper(config);
            var students = mapper.Map<IEnumerable<Student>, List<StudentDTO>>(Database.Students.GetAll()).OrderBy(student => student.Name);
            return students;
        }

        public IEnumerable<StudentDTO> GetAllStudentsSortedBySurname()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>());
            var mapper = new Mapper(config);
            var students = mapper.Map<IEnumerable<Student>, List<StudentDTO>>(Database.Students.GetAll()).OrderBy(student => student.Surname);
            return students;
        }

        public IEnumerable<StudentDTO> GetAllStudentsSortedByGroup()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>());
            var mapper = new Mapper(config);
            var students = mapper.Map<IEnumerable<Student>, List<StudentDTO>>(Database.Students.GetAll()).OrderBy(student => student.GroupName);
            return students;
        }

        public IEnumerable<StudentDTO> GetStudentsByName(string name)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>());
            var mapper = new Mapper(config);
            var students = mapper.Map<IEnumerable<Student>, List<StudentDTO>>(Database.Students.Find(stud => stud.Name == name));
            return students;
        }
        public IEnumerable<StudentDTO> GetStudentsBySurname(string surname)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>());
            var mapper = new Mapper(config);
            var students = mapper.Map<IEnumerable<Student>, List<StudentDTO>>(Database.Students.Find(stud => stud.Surname == surname));
            return students;
        }

        public IEnumerable<StudentDTO> GetStudentsByGroup(string group)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>());
            var mapper = new Mapper(config);
            var students = mapper.Map<IEnumerable<Student>, List<StudentDTO>>(Database.Students.Find(stud => stud.GroupName == group));
            return students;
        }

        public StudentDTO GetStudentById(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Студент відсутній з вказаним ID");
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>());
            var mapper = new Mapper(config);
            var student = Database.Students.Get(id.Value);

            if (student == null)
            {
                throw new ValidationException("Студент відсутній з вказаним ID");
            }

            return mapper.Map<Student, StudentDTO>(student);
        }

        public void UpdateStudent(StudentDTO studentDTO)
        {
            Student student = Database.Students.Get(studentDTO.Id);
            if (student == null)
            {
                throw new ValidationException("Студент відсутній з вказаним ID");
            }

            student.Name = studentDTO.Name;
            student.Surname = studentDTO.Surname;
            student.GroupName = studentDTO.GroupName;

            Database.Students.Update(student);
        }

        public void DeleteStudentById(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Студент відсутній з вказаним ID");
            }
            Database.Students.Delete(id.Value);
            Database.Save();
        }

        public void CreateStudent(StudentDTO studentDTO)
        {
            Student student = new Student()
            {
                Name = studentDTO.Name,
                Surname = studentDTO.Surname,
                GroupName = studentDTO.GroupName
            };
            
            Database.Students.Create(student);
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
