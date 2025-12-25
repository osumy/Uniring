using Microsoft.AspNetCore.Http;

namespace Uniring.Contracts.Media
{
    public class MediaRequest
    {
        public Guid? id { get; set; }

        public IFormFile? file { get; set; }
    }
}
