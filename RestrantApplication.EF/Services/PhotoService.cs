using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using RestrantApplication.Core.Interfaces;
using RestrantApplication.Core.Models.Product;
using RestrantApplication.Core.Services;
namespace RestrantApplication.EF.Services
{
    public class PhotoService : IPhotoService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly string _imageFolderPath;
        #endregion
        #region Constructors

        public PhotoService(IUnitOfWork unitOfWork, IHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;

            // Define the path where product images will be stored
            _imageFolderPath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images", "ProductImages");

            // Create the directory if it doesn't exist
            if (!Directory.Exists(_imageFolderPath))
            {
                Directory.CreateDirectory(_imageFolderPath);
            }
        }
        #endregion
        #region Handle Private Functions

        /// <summary>
        /// Saves the uploaded image to disk with a unique file name.
        /// </summary>
        /// <param name="image">The uploaded image file.</param>
        /// <returns>The generated unique file name as a string.</returns>
        private async Task<string> SaveImageToDiskAsync(IFormFile image)
        {
            string uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(image.FileName);
            string filePath = Path.Combine(_imageFolderPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
                stream.Close();
            }

            return uniqueFileName;
        }

        /// <summary>
        /// Validates image file extension and size.
        /// </summary>
        /// <param name="image">The image file to validate.</param>
        /// <returns>True if the image is valid; otherwise, false.</returns>
        private bool IsImageValid(IFormFile image)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            const long maxFileSize = 5 * 1024 * 1024;

            if (!allowedExtensions.Contains(extension) || image.Length > maxFileSize)
                return false;
            return true;
        }
        #endregion
        #region Handle Public Functions

        /// <summary>
        /// Uploads an image and saves its record in the database.
        /// </summary>
        /// <param name="image">The image to upload.</param>
        /// <returns>The created <see cref="Photo"/> entity if successful; otherwise, null.</returns>
        public async Task<Photo> UploadImage(IFormFile image)
        {
            if (image == null) return null;

            if (!IsImageValid(image))
                return null;

            try
            {
                // Save image file to disk
                string uniqueFileName = await SaveImageToDiskAsync(image);
                if (uniqueFileName.IsNullOrEmpty())
                    return null;
                // Save metadata in the database
                var photo = new Photo { PhotoName = uniqueFileName };
                await _unitOfWork.PhotoRepository.AddAsync(photo);
                await _unitOfWork.Complete();

                return photo;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Deletes an image both from disk and database using its ID.
        /// </summary>
        /// <param name="imageId">The ID of the image to delete.</param>
        /// <returns>True if the image was deleted successfully; otherwise, false.</returns>
        public async Task<bool> DeleteImageByID(int imageId)
        {
            if (imageId <= 0) return false;

            try
            {
                var photo = await _unitOfWork.PhotoRepository.GetByIDAsync(imageId);
                if (photo == null || string.IsNullOrEmpty(photo.PhotoName)) return false;

                // Delete image file from disk
                var imagePath = Path.Combine(_imageFolderPath, photo.PhotoName);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }

                // Delete photo record from database
                await _unitOfWork.PhotoRepository.DeleteAsync(photo.ID);
                await _unitOfWork.Complete();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Updates an existing image: deletes the old one and replaces it with a new one.
        /// </summary>
        /// <param name="oldPhotoId">ID of the photo to replace.</param>
        /// <param name="newImage">New image file.</param>
        /// <returns>The updated <see cref="Photo"/> entity if successful; otherwise, null.</returns>
        public async Task<Photo> UpdatePhotoAsync(int oldPhotoId, IFormFile newImage)
        {
            if (newImage == null || oldPhotoId <= 0) return null;

            if (!IsImageValid(newImage))
                return null;

            try
            {
                var oldPhoto = await _unitOfWork.PhotoRepository.GetByIDAsync(oldPhotoId);
                if (oldPhoto == null || oldPhoto.PhotoName.IsNullOrEmpty()) return null;

                // Delete old image file
                var oldImagePath = Path.Combine(_imageFolderPath, oldPhoto.PhotoName);
                if (File.Exists(oldImagePath))
                {
                    File.Delete(oldImagePath);
                }

                // Save new image to disk
                string newFileName = await SaveImageToDiskAsync(newImage);
                if (newFileName.IsNullOrEmpty()) 
                    return null;
                
                // Update the photo record
                oldPhoto.PhotoName = newFileName;
                await _unitOfWork.Complete();

                return oldPhoto;
            }
            catch
            {
                return null;
            }


        }
        #endregion
    }
}