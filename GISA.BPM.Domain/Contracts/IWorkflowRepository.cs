using GISA.BPM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GISA.BPM.Domain.Contracts
{
    public interface IWorkflowRepository
    {
        Task<IEnumerable<Workflow>> GetAllAsync();
        Task<Workflow> GetByIdAsync(Guid id);
        Task SaveAsync(Workflow workflow);
        Task UpdateAsync(Workflow workflow);
        Task RemoveAsync(Guid id);
    }
}
