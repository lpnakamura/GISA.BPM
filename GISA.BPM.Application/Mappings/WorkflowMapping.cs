using AutoMapper;
using GISA.BPM.Application.ViewModels;
using GISA.BPM.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;

namespace GISA.BPM.Application.Mappings
{
    public class WorkflowMapping : Profile
    {
        public WorkflowMapping()
        {
            CreateMap<IFormCollection, Workflow>()
                .ForMember(destination => destination.Id, configuration => configuration.MapFrom(_ => Guid.NewGuid()))
                .BeforeMap(OnBeforeWorkflowMap);

            CreateMap<Workflow, WorkflowUpdateViewModel>().ReverseMap();
            CreateMap<Workflow, WorkflowResponseViewModel>().ReverseMap();
        }

        private void OnBeforeWorkflowMap(IFormCollection formData, Workflow workflow)
        {
            workflow.Id = Guid.NewGuid();

            if (formData.TryGetValue("fileIdentifier", out var fileIdentifier))
                workflow.SetFileIdentifier(Guid.Parse(fileIdentifier));

            if (formData.TryGetValue("description", out var description))
                workflow.SetDescription(description);

            if (formData.TryGetValue("name", out var name))
                workflow.SetName(name);
        }
    }
}
