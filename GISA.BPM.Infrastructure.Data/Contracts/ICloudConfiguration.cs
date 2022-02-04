namespace GISA.BPM.Infrastructure.Data.Contracts
{
    public interface ICloudConfiguration
    {
        string GetRegion();
        string GetAccessKey();
        string GetSecretKey();
    }
}
