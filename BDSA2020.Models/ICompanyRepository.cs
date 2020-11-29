using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BDSA2020.Entities;
using BDSA2020.Shared;

namespace BDSA2020.Models
{
    public interface ICompanyRepository
    {
        Task<Guid> CreateCompanyAsync(CreateCompanyDTO company);
        Task<CompanyDetailsDTO> GetCompanyAsync(Guid id);
        Task<ICollection<CompanyDetailsDTO>> GetCompaniesAsync();
        Task<bool> UpdateCompanyAsync(UpdateCompanyDTO company);
        Task<bool> DeleteCompanyAsync(Guid id);
    }
}