using System.Collections.Generic;
using System.Threading.Tasks;
using BDSA2020.Entities;
using BDSA2020.Shared;

namespace BDSA2020.Models
{
    public interface IStudentRepository
    {
        Task<int> CreateStudentAsync(CreateStudentDTO student);
        Task<StudentDetailsDTO> GetStudentAsync(int id);
        Task<ICollection<StudentDetailsDTO>> GetStudentsAsync();
        Task<bool> UpdateStudentAsync(UpdateStudentDTO student);
        Task<bool> DeleteStudentAsync(int id);
    }
}