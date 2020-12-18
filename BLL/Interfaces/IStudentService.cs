using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IStudentService
    {
        IEnumerable<StudentDTO> GetAllStudents();
        StudentDTO GetStudentById(int? id);
        IEnumerable<StudentDTO> GetAllStudentsSortedByName();
        IEnumerable<StudentDTO> GetAllStudentsSortedBySurname();
        IEnumerable<StudentDTO> GetAllStudentsSortedByGroup();
        IEnumerable<StudentDTO> GetStudentsByName(string name);
        IEnumerable<StudentDTO> GetStudentsBySurname(string surname);
        IEnumerable<StudentDTO> GetStudentsByGroup(string group);
        void UpdateStudent(StudentDTO studentDTO);
        void DeleteStudentById(int? id);
        void CreateStudent(StudentDTO studentDTO);
        void Dispose();
    }
}
