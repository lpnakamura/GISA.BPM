using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using GISA.BPM.Domain.Contracts;
using GISA.BPM.Domain.Entities;
using GISA.BPM.Domain.Extensions;
using GISA.BPM.HttpContext.Shared.Contracts;
using GISA.BPM.Infrastructure.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GISA.BPM.Infrastructure.Data.Repositories
{
    public class WorkflowRepository : IWorkflowRepository
    {
        private readonly AmazonDynamoDBClient _dynamoDBClient;
        private readonly DynamoDBContext _dynamoDBContext;
        private readonly IClaimContext _claimContext;

        public WorkflowRepository(ICloudConfiguration cloudConfiguration, IClaimContext claimContext)
        {
            _dynamoDBClient = new AmazonDynamoDBClient(cloudConfiguration.GetAccessKey(), 
                cloudConfiguration.GetSecretKey(), RegionEndpoint.GetBySystemName(cloudConfiguration.GetRegion()));
            _dynamoDBContext = new DynamoDBContext(_dynamoDBClient);
            _claimContext = claimContext;
        }

        public async Task SaveAsync(Workflow workflow)
        {
            await _dynamoDBContext.SaveAsync(workflow.ToInsert(_claimContext.GetUserName()) as Workflow);
        }

        public async Task<IEnumerable<Workflow>> GetAllAsync()
        {
            var table = _dynamoDBContext.GetTargetTable<Workflow>();
            var scanOps = new ScanOperationConfig();
            var results = table.Scan(scanOps);
            List<Document> data = await results.GetNextSetAsync();
            return _dynamoDBContext.FromDocuments<Workflow>(data);
        }

        public async Task<Workflow> GetByIdAsync(Guid id)
        {
            return await _dynamoDBContext.LoadAsync<Workflow>(id);
        }

        public async Task RemoveAsync(Guid id)
        {
            await _dynamoDBContext.DeleteAsync<Workflow>(id);
        }

        public async Task UpdateAsync(Workflow workflow)
        {
            var workflowEntity = await GetByIdAsync(workflow.Id);
            workflowEntity.SetDescription(workflow.Description)
                .SetName(workflow.Name).SetFileUrl(workflow.FileUrl).ToUpdate(_claimContext.GetUserName());

            await _dynamoDBContext.SaveAsync(workflowEntity);
        }
    }
}
