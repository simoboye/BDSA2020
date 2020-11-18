using System.Collections.Generic;
using System.Threading.Tasks;
using BDSA2020.Entities;

namespace BDSA2020.Models
{
    public interface IStudentRepository
    {
        Task<int> CreateStudentAsync(Student student);
        Task<Student> GetStudentAsync(int id);
        Task<ICollection<Student>> GetStudentsAsync();
        Task<bool> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int id);
    }
}