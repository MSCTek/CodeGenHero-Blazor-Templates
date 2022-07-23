using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ArtistSite.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly string _azureBlobConnectionString;

        public UploadController(IConfiguration configuration)
        {
            _azureBlobConnectionString = configuration.GetConnectionString("AzureBlobConnection");
        }

        [HttpPost("UploadIcon")]
        public async Task<IActionResult> UploadIcon()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                string blobUri = await UploadToBlobStorageAsync($"icon/{file.FileName}", file);

                if (!string.IsNullOrWhiteSpace(blobUri))
                {
                    return Ok(blobUri);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                string blobUri = await UploadToBlobStorageAsync($"medium/{file.FileName}", file);

                if (!string.IsNullOrWhiteSpace(blobUri))
                {
                    return Ok(blobUri);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        private async Task<string> UploadToBlobStorageAsync(string filePath, Microsoft.AspNetCore.Http.IFormFile file)
        {
            string retVal = null;

            if (file.Length > 0)
            {
                var container = new BlobContainerClient(_azureBlobConnectionString, "webimages");
                var createResponse = await container.CreateIfNotExistsAsync();
                if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                    await container.SetAccessPolicyAsync(PublicAccessType.Blob);

                var blob = container.GetBlobClient(filePath);
                await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

                using (var fileStream = file.OpenReadStream())
                {
                    var blobContentInfo = await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.ContentType });
                }

                retVal = filePath;
            }

            return retVal;
        }
    }
}