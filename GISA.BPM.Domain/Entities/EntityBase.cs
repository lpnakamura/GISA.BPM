using Amazon.DynamoDBv2.DataModel;
using FluentValidation.Results;
using System;

namespace GISA.BPM.Domain.Entities
{
    public abstract class EntityBase
    {
        [DynamoDBHashKey]
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        [DynamoDBIgnore]
        public bool Valid { get; private set; }
        [DynamoDBIgnore]
        public bool Invalid => !Valid;
        [DynamoDBIgnore]
        public ValidationResult ValidationResult { get; private set; }
        
        public abstract ValidationResult GetValidationResult<TModel>();

        public bool Validate<TModel>(TModel model)
        {
            ValidationResult = GetValidationResult<TModel>();
            return Valid = ValidationResult.IsValid;
        }


    }
}
