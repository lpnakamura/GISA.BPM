using System;

namespace GISA.BPM.Application.ViewModels
{
    public class WorkflowResponseViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid FileIdentifier { get; set; }
        public string FileUrl { get; set; }
    }
}
