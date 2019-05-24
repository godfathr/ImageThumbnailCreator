using System.Drawing;
using System.Web;

namespace ImageThumbnailCreator.Interfaces
{
    public interface IFileManager
    {
        void CheckAndCreateDirectory(string imageFolderPath);

        string SaveOriginal(string imageFolder, HttpPostedFileBase photo); //the photo should be an HttpPostedFileBase

        string SaveThumbnail(Bitmap thumbnail, string imagePath, string thumbnailFileName);
    }
}
