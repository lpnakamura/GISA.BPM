using AutoMapper;
using GISA.BPM.Application.Contracts;
using GISA.BPM.Application.ViewModels;
using GISA.BPM.Domain.Contracts;
using GISA.BPM.Domain.Entities;
using GISA.BPM.Infrastructure.Storage.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GISA.BPM.Application.Services
{
    public class WorkflowService : ServiceBase, IWorkflowService
    {
        private readonly IWorkflowRepository _workflowRepository;
        private readonly IStorageRepository _storageRepository;
        private readonly IMapper _mapper;

        public WorkflowService(IMapper mapper, INotificationContext notificationContext, IWorkflowRepository workflowRepository, IStorageRepository storageRepository) : base(mapper, notificationContext)
        {
            _mapper = mapper;
            _workflowRepository = workflowRepository;
            _storageRepository = storageRepository;
        }

        public async Task<WorkflowResponseViewModel> SaveAsync(IFormFile file, IFormCollection formData)
        {
            try
            {
                if (!CheckAndRegisterFileAttached(file))
                    return null;

                if (!CheckAndRegisterExtensionFileAllowed(file))
                    return null;

                var workflow = BuildMapper<Workflow>(formData);
                var fileResultPath = await _storageRepository.UploadFileAsync(file);

                workflow.SetFileIdentifier(fileResultPath.Item1);
                workflow.SetFileUrl(fileResultPath.Item2);

                workflow.Validate(workflow);
                CheckAndRegisterInvalidNotifications(workflow);
                if (workflow.Invalid) return null;

                await _workflowRepository.SaveAsync(workflow);
                return _mapper.Map<WorkflowResponseViewModel>(workflow);
            }
            catch (Exception exception)
            {
                AddNotification("WorkflowSaveValidator", exception.Message);
            }

            return null;
        }

        public async Task<IEnumerable<WorkflowResponseViewModel>> GetAllAsync()
        {
            var workflowList = await _workflowRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<WorkflowResponseViewModel>>(workflowList);
        }

        public async Task<WorkflowResponseViewModel> GetByIdAsync(Guid id)
        {
            var workflow = await _workflowRepository.GetByIdAsync(id);
            return _mapper.Map<WorkflowResponseViewModel>(workflow);
        }

        public async Task RemoveAsync(Guid id)
        {
            await _storageRepository.RemoveFileAsync(id);
            await _workflowRepository.RemoveAsync(id);
        }

        public async Task<WorkflowResponseViewModel> UpdateAsync(Guid id, IFormFile file, IFormCollection formData)
        {
            try
            {
                if (!CheckAndRegisterFileAttached(file))
                    return null;

                if (!CheckAndRegisterExtensionFileAllowed(file))
                    return null;

                var workflow = BuildMapper<Workflow>(formData).SetId(id);
                workflow.SetFileUrl(await _storageRepository.GetFileUrlAsync(workflow.FileIdentifier));

                workflow.Validate(workflow);
                CheckAndRegisterInvalidNotifications(workflow);
                if (workflow.Invalid) return null;

                if (file != null) await _storageRepository.ReplaceFileAsync(workflow.FileIdentifier, file);

                await _workflowRepository.UpdateAsync(workflow);
                return _mapper.Map<WorkflowResponseViewModel>(workflow);
            }
            catch (Exception exception)
            {
                AddNotification("WorkflowUpdateValidator", exception.Message);
            }

            return null;
        }

        private bool CheckAndRegisterFileAttached(IFormFile file)
        {
            if (file == null)
            {
                AddNotification("FileNotAttachedValidator", "File not attached");
                return false;
            }

            return true;
        }

        private bool CheckAndRegisterExtensionFileAllowed(IFormFile file)
        {
            if (file == null || !file.FileName.EndsWith(".bpmn"))
            {
                AddNotification("ExtensionFileNotAllowedValidator", "File must have the .bmpn extension");
                return false;
            }

            return true;
        }
    }
}
