using Gifter.Services.DTOs.Image;
using System.Threading.Tasks;

namespace Gifter.Services.Services
{
    public interface IUploadService
    {
        Task<bool> UploadAsync(UploadImageDTO uploadImageDTO, string userId);
    }
}