using System.IO;
using System.Threading.Tasks;

namespace Services.Contracts.External
{
    public interface ICloudinary
    {
        public Task<string> UploadImageAsync(MemoryStream imageMemoryStream, string fileName);
    }
}
