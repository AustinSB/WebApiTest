using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace WebApiTest.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController(FileExtensionContentTypeProvider provider) : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _contentTypeProvider = provider ?? throw new ArgumentNullException(nameof(provider));

        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            var path = "getting-started-with-rest-slides.pdf";
            if (!System.IO.File.Exists(path)) { return NotFound(); }

            if (!_contentTypeProvider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, contentType, Path.GetFileName(path));
        }
    }
}
