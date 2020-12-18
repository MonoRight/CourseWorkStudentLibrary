using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IStudentBookService
    {
        IEnumerable<StudentBookDTO> GetAllRecordings();
        StudentBookDTO GetRecordingById(int? id);
        void UpdateRecording(StudentBookDTO studentBookDTO);
        void DeleteRecordingById(int? id);
        void CreateRecording(StudentBookDTO studentBookDTO);
        void Dispose();
    }
}
