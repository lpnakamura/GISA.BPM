using GISA.BPM.Application.Contracts;
using GISA.BPM.Application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GISA.BPM.Api.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WorkflowController : ApiBaseController
    {
        private readonly IWorkflowService _workflowService;

        public WorkflowController(IWorkflowService workflowService, INotificationContext notificationContext) : base(notificationContext)
        {
            _workflowService = workflowService;
        }

        [HttpGet("{id}")]
        public async Task<WorkflowResponseViewModel> GetByIdAsync(Guid id)
        {
            return await _workflowService.GetByIdAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<WorkflowResponseViewModel>> GetAllAsync()
        {
            return await _workflowService.GetAllAsync();
        }

        [HttpPost]
        public async Task<ActionResult> SaveAsync([FromForm(Name = "file")] IFormFile file, IFormCollection formData)
        {
            return CustomResponse(await _workflowService.SaveAsync(file, formData));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(Guid id, [FromForm(Name = "file")] IFormFile file, IFormCollection data)
        {
            return CustomResponse(await _workflowService.UpdateAsync(id, file, data));
        }

        [HttpDelete("{id}")]
        public async Task RemoveAsync(Guid id)
        {
            await _workflowService.RemoveAsync(id);
        }
    }
}
