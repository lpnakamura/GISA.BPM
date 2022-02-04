using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace GISA.BPM.Infrastructure.Storage.Contracts
{
    public interface IStorageRepository
    {
        Task<Tuple<Guid, string>> UploadFileAsync(IFormFile file);
        Task<Tuple<Guid, string>> ReplaceFileAsync(Guid fileIdentifier, IFormFile file);
        Task RemoveFileAsync(Guid fileIdentifier);
        Task<string> GetFileUrlAsync(Guid fileIdentifier);
    }
}
