using GISA.BPM.Application.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GISA.BPM.Application.Contracts
{
    public interface IWorkflowService
    {
        Task<IEnumerable<WorkflowResponseViewModel>> GetAllAsync();
        Task<WorkflowResponseViewModel> GetByIdAsync(Guid id);
        Task<WorkflowResponseViewModel> SaveAsync(IFormFile file, IFormCollection formData);
        Task<WorkflowResponseViewModel> UpdateAsync(Guid id, IFormFile file, IFormCollection formData);
        Task RemoveAsync(Guid id);
    }
}
