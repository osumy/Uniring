using Microsoft.AspNetCore.Http;

namespace Uniring.Contracts.File
{
    public class FileRequest
    {
        public Guid? id { get; set; }

        public IFormFile? file { get; set; }
    }
}
