using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BDSA2020.Entities;
using BDSA2020.Shared;

namespace BDSA2020.Models
{
    public interface IStudentRepository
    {
        Task<Guid> CreateStudentAsync(CreateStudentDTO student);
        Task<StudentDetailsDTO> GetStudentAsync(Guid id);
        Task<ICollection<StudentDetailsDTO>> GetStudentsAsync();
        Task<bool> UpdateStudentAsync(UpdateStudentDTO student);
        Task<bool> DeleteStudentAsync(Guid id);
        Task<bool> SavePlacementDescription(Guid studentId, int descriptionId);
        Task<bool> UnSavePlacementDescription(Guid studentId, int descriptionId);
    }
}