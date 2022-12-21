namespace ECommerce.App.Helpers.Interfaces
{
    public interface IImageHelper
    {
        Task<byte[]> UploadImageArrayAsync(IFormFile imageFile);
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);
        string UploadImage(byte[] pictureArray, string folder);
    }
}
