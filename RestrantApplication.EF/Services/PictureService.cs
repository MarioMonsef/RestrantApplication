using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using RestrantApplication.Core.Interfaces;
using RestrantApplication.Core.Models.Identity;
using RestrantApplication.Core.Services;

namespace RestrantApplication.EF.Services
{
    /// <summary>
    /// Handles saving and retrieving user profile pictures.
    /// </summary>
    public class PictureService : IPictureService
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly string _imageFolderPath;

        #endregion

        #region Constructors

        public PictureService(IUnitOfWork unitOfWork, IHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;

            // Define the directory path to store uploaded user pictures
            _imageFolderPath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images", "UserPicturs");

            // Create directory if it does not exist
            if (!Directory.Exists(_imageFolderPath))
            {
                Directory.CreateDirectory(_imageFolderPath);
            }
        }

        #endregion

        #region Handle Private Functions

        /// <summary>
        /// Saves the uploaded image file to disk with a unique filename.
        /// </summary>
        /// <param name="picture">Image file uploaded by the user.</param>
        /// <returns>The unique file name of the saved image.</returns>
        private async Task<string> SaveImageToDiskAsync(IFormFile picture)
        {
            // Generate a unique filename using GUID and original file name
            var uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(picture.FileName);
            var filePath = Path.Combine(_imageFolderPath, uniqueFileName);

            // Save the image to disk
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await picture.CopyToAsync(fileStream);
                fileStream.Close();
            }

            return uniqueFileName;
        }

        /// <summary>
        /// Validates the uploaded image file extension and size.
        /// </summary>
        /// <param name="image">The uploaded image file.</param>
        /// <returns>True if image is valid; otherwise, false.</returns>
        private bool IsImageValid(IFormFile image)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            const long maxFileSize = 5 * 1024 * 1024; // 5MB max

            return allowedExtensions.Contains(extension) && image.Length <= maxFileSize;
        }

        #endregion

        #region Handle Public Functions

        /// <summary>
        /// Uploads a new picture for a user and stores it in the database and file system.
        /// </summary>
        /// <param name="picture">The image file to upload.</param>
        /// <returns>The saved <see cref="UserPicture"/> entity, or null if upload fails.</returns>
        public async Task<UserPicture> UploadPictureAsync(IFormFile picture)
        {
            if (picture == null || !IsImageValid(picture))
                return null;

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Save image file to disk
                string uniqueFileName = await SaveImageToDiskAsync(picture);

                if (uniqueFileName.IsNullOrEmpty())
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                // Create new picture record in database
                var addPicture = new UserPicture { PictureName = uniqueFileName };
                await _unitOfWork.PictureRepository.AddAsync(addPicture);
                await _unitOfWork.Complete();
                await transaction.CommitAsync();

                return addPicture;
            }
            catch
            {
                await transaction.RollbackAsync();
                return null;
            }
        }

        /// <summary>
        /// Retrieves the picture file name based on picture ID.
        /// </summary>
        /// <param name="pictureID">The ID of the picture.</param>
        /// <returns>The name of the picture file.</returns>
        public async Task<string> GetPictureNameAsync(int pictureID)
        {
            var picture = await _unitOfWork.PictureRepository.GetByIDAsync(pictureID);
            return picture.PictureName;
        }

        /// <summary>
        /// Deletes a user's profile picture from the disk by its ID.
        /// </summary>
        /// <param name="pictureID">The ID of the picture to delete.</param>
        /// <returns>True if the picture was found and successfully deleted; otherwise, false.</returns>
        public async Task<bool> DeletePictureInDiskAsync(int pictureID)
        {
            var picture = await _unitOfWork.PictureRepository.GetByIDAsync(pictureID);
            if (picture == null)
                return false;

            var fileName = Path.GetFileName(picture.PictureName);
            var filePath = Path.Combine(_imageFolderPath, fileName);

            if (!File.Exists(filePath))
                return false;

            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}
