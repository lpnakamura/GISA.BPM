using FluentValidation;
using GISA.BPM.Domain.Entities;

namespace GISA.BPM.Domain.Validators
{
    public class WorkflowValidator : AbstractValidator<Workflow>
    {
        public WorkflowValidator()
        {
            RuleFor(insertWorkflowValidator => insertWorkflowValidator.Id)
                .NotEmpty();

            RuleFor(insertWorkflowValidator => insertWorkflowValidator.Name)
                .NotEmpty();

            RuleFor(insertWorkflowValidator => insertWorkflowValidator.Description)
                .NotEmpty();

            RuleFor(insertWorkflowValidator => insertWorkflowValidator.FileUrl)
                .NotEmpty();
        }
    }
}
