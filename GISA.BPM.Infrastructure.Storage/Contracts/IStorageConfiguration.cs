namespace GISA.BPM.Infrastructure.Storage.Contracts
{
    public interface IStorageConfiguration
    {
        string GetBucketName();
        string GetBucketUrlBase();
        string GetRegion();
        string GetAccessKey();
        string GetSecretKey();
    }
}
