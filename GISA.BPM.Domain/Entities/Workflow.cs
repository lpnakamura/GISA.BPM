using FluentValidation.Results;
using GISA.BPM.Domain.Validators;
using System;

namespace GISA.BPM.Domain.Entities
{
    public class Workflow : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public Guid FileIdentifier { get; set; }

        public Workflow SetId(Guid id) { Id = id; return this; }
        public Workflow SetName(string name) { Name = name; return this; }
        public Workflow SetDescription(string description) { Description = description; return this; }
        public Workflow SetFileUrl(string fileUrl) { FileUrl = fileUrl; return this; }
        public Workflow SetFileIdentifier(Guid fileIdentifier) { FileIdentifier = fileIdentifier; return this; }

        public override ValidationResult GetValidationResult<TModel>()
        {
            return new WorkflowValidator().Validate(this);
        }
    }
}
