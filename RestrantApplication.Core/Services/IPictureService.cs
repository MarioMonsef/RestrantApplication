using Microsoft.AspNetCore.Http;
using RestrantApplication.Core.Models.Identity;

namespace RestrantApplication.Core.Services
{
    public interface IPictureService
    {
        /// <summary>
        /// Uploads a user's profile picture.
        /// </summary>
        /// <param name="Picture">The picture file to be uploaded.</param>
        /// <returns>The uploaded UserPicture entity containing metadata and path.</returns>
        Task<UserPicture> UploadPictureAsync(IFormFile Picture);

        /// <summary>
        /// Retrieves the name or path of a user's picture by its ID.
        /// </summary>
        /// <param name="PictureID">The ID of the picture to retrieve.</param>
        /// <returns>The file name or path of the picture as a string.</returns>
        Task<string> GetPictureNameAsync(int PictureID);
        /// <summary>
        /// Deletes a user's profile picture from the disk by its ID.
        /// </summary>
        /// <param name="pictureID">The ID of the picture to delete.</param>
        /// <returns>True if the picture was found and successfully deleted; otherwise, false.</returns>
        Task<bool> DeletePictureInDiskAsync(int pictureID);
    }
}
