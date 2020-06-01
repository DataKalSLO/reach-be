using System;
using System.Threading.Tasks;
using HourglassServer.Custom.Constraints;
using HourglassServer.Data;
using HourglassServer.Data.DataManipulation.ImageOperations;
using HourglassServer.Images;
using HourglassServer.Models.Application.ImageModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using HourglassServer.Custom.Exception;
using Microsoft.AspNetCore.Http;
using HourglassServer.AmazonS3;

namespace HourglassServer.Controllers
{
    [DefaultControllerRoute]
    public class ImageController : Controller
    {
        readonly HourglassContext context;
        readonly IConfiguration config;

        const string FormImageKeyName = "image";

        public ImageController(HourglassContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }

        /*
         * Uploading Images
         */

        [HttpPost("profile/{user_id}")]
        public async Task<IActionResult> UploadProfileImage(string user_id)
        {
            //TODO: Delete old profile pic before uploading new one
            throw new NotImplementedException();
            //TODO: @Karson replicate code below with new enums for new bucket
        }

        [HttpPost("imageblock/{block_id}")]
        public async Task<IActionResult> UploadImageBlockImage(string block_id)
        {
            return await TryApiAction(async () =>
            {
                IFormFile imageFile = GetImageFileFromForm();
                FormImage image = new FormImage()
                {
                    ImageFile = imageFile,
                    Type = ImageUtility.ParseImageExtensionFromContentType(imageFile.ContentType),
                    Id = block_id
                };

                string imageUrl = await ImageManipulator.UploadFormImageToBucket(
                    image,
                    this.config,
                    Bucket.STORY_IMAGE_BLOCK
               );
                return new CreatedResult(imageUrl, new { id = block_id, imageUrl });
            }); 
        }

        private IFormFile GetImageFileFromForm()
        {
            AssertEditPermissions();

            IFormFileCollection files = HttpContext.Request.Form.Files;

            if (files.Count != 1)
                throw new HourglassException(
                    string.Format("Expected a single image. Received {0} images.", files.Count),
                    ExceptionTag.BadValue);

            IFormFile imageFile = files.GetFile(FormImageKeyName);

            if (imageFile.Length == 0)
                throw new HourglassException("Expected non-empty image", ExceptionTag.BadValue);

            return imageFile;
        }

        /*
         * Remove File
         */

        [HttpDelete("imageblock/{fileName}")]
        public async Task<IActionResult> RemoveImageBlockImage(string fileName)
        {
            return await TryApiAction(async () =>
            {
                AssertEditPermissions();

                if (fileName == null || fileName.Length == 0)
                    throw new HourglassException("Received empty file name", ExceptionTag.BadValue);

                await ImageManipulator.RemoveImageFromBucket(
                    this.config,
                    Bucket.STORY_IMAGE_BLOCK,
                    fileName
                    );
                return new NoContentResult();
            });
        }

        /*
         * Misc Helpers
         */

        private void AssertEditPermissions()
        {
            ConstraintChecker<FormImage> constraintChecker = new ConstraintChecker<FormImage>(
               new ConstraintEnvironment(this.HttpContext.User, context),
               null);
            constraintChecker.AssertConstraint(Constraints.HAS_USER_ACCOUNT);
            //TODO: Add Database table for storing owners of images so only owners can delete images.
        }

        private async Task<IActionResult> TryApiAction(Func<Task<IActionResult>> apiAction)
        {
            try
            {
                return await apiAction();
            }
            catch(HourglassException e)
            {
                return new BadRequestObjectResult(e);
            }
            catch(Exception e)
            {
                return new BadRequestObjectResult(new HourglassException(e.ToString(), ExceptionTag.BadValue));
            }
        }
    }
}
