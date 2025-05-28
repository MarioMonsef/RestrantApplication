using Microsoft.AspNetCore.Http;
using RestrantApplication.Core.Models.Product;

namespace RestrantApplication.Core.Services
{
    public interface IPhotoService
    {
        /// <summary>
        /// Uploads an image and returns the corresponding Photo entity.
        /// </summary>
        /// <param name="Image">The image file to be uploaded.</param>
        /// <returns>The uploaded Photo object containing metadata and storage info.</returns>
        Task<Photo> UploadImage(IFormFile Image);

        /// <summary>
        /// Deletes an image by its ID.
        /// </summary>
        /// <param name="ImageID">The ID of the image to be deleted.</param>
        /// <returns>True if the image was deleted successfully; otherwise, false.</returns>
        Task<bool> DeleteImageByID(int ImageID);

        /// <summary>
        /// Updates an existing photo by replacing it with a new one.
        /// </summary>
        /// <param name="OldphotoID">The ID of the old photo to be replaced.</param>
        /// <param name="Image">The new image file.</param>
        /// <returns>The updated Photo object with new image information.</returns>
        Task<Photo> UpdatePhotoAsync(int OldphotoID, IFormFile Image);
    }
}
