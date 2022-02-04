using GISA.BPM.Infrastructure.Storage.Contracts;
using Microsoft.Extensions.Configuration;

namespace GISA.BPM.Infrastructure.Storage.Configurations
{
    public class S3Configuration : IStorageConfiguration
    {
        private readonly IConfiguration _configuration;
        private IConfigurationSection _configurationSection;

        public IConfigurationSection ConfigurationSection
        {
            get
            {
                if (_configurationSection == null) _configurationSection = _configuration.GetSection("Aws");
                return _configurationSection;
            }
        }

        public S3Configuration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetAccessKey()
        {
            return ConfigurationSection.GetSection("AccessKey").Value;
        }

        public string GetBucketName()
        {
            return ConfigurationSection.GetSection("BucketName").Value;
        }

        public string GetBucketUrlBase()
        {
            return ConfigurationSection.GetSection("BucketUrlBase").Value;
        }

        public string GetRegion()
        {
            return ConfigurationSection.GetSection("Region").Value;
        }

        public string GetSecretKey()
        {
            return ConfigurationSection.GetSection("SecretKey").Value;
        }
    }
}
