using GISA.BPM.Infrastructure.Data.Contracts;
using Microsoft.Extensions.Configuration;

namespace GISA.BPM.Infrastructure.Data.Configurations
{
    public class DynamoDBConfiguration : ICloudConfiguration
    {
        private readonly IConfiguration _configuration;

        public DynamoDBConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetAccessKey()
        {
            return _configuration.GetSection("Aws").GetSection("AccessKey").Value;
        }

        public string GetRegion()
        {
            return _configuration.GetSection("Aws").GetSection("Region").Value;
        }

        public string GetSecretKey()
        {
            return _configuration.GetSection("Aws").GetSection("SecretKey").Value;
        }
    }
}
