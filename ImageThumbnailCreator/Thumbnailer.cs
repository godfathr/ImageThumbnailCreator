using ImageThumbnailCreator.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ImageThumbnailCreator
{
    //TODO: Convert to use .NET Standard https://stackoverflow.com/questions/46722409/cannot-find-bitmap-class-in-class-library-net-standard
    public class Thumbnailer : IFileManager, IThumbnailer
    {
        /// <summary>
        /// Create the directory for storing image files if it doesn't already exist.
        /// </summary>
        /// <param name="imageFolder"></param>
        public void CheckAndCreateDirectory(string imageFolderPath)
        {
            try
            {
                Directory.CreateDirectory(imageFolderPath);
            }
            catch (Exception ex)
            {
                throw new IOException(ex.Message);
            }
        }

        /// <summary>
        /// Create the thumbnail
        /// </summary>
        /// <param name="width"></param>
        /// <param name="imageFolder"></param>
        /// <param name="fullImagePath"></param>
        /// <returns></returns>
        public string Create(float width, string imageFolder, string fullImagePath)
        {
            if (width < 1) throw new ArgumentException($"The width parameter must be greater than 0.");
            if (string.IsNullOrEmpty(imageFolder)) throw new ArgumentNullException(nameof(imageFolder));
            if (string.IsNullOrEmpty(fullImagePath)) throw new ArgumentNullException(nameof(fullImagePath));
            if (!string.IsNullOrEmpty(imageFolder))
            {
                CheckAndCreateDirectory(imageFolder); //make sure the directory exists
            }

            Bitmap thumbnail;

            try
            {
                //read the bytes
                Bitmap image = new Bitmap(fullImagePath);
                var srcImage = Image.FromFile(fullImagePath);

                //Image image = Image.FromFile(imageFolder + fileName);
                float imageWidth = srcImage.Width;
                float imageHeight = srcImage.Height;

                if(width > imageWidth)
                {
                    throw new Exception("Thumbnail width can not be greater than the original image width.");
                }

                List<int> propertyIdList = new List<int>();
                if (srcImage.PropertyIdList != null)
                {
                    propertyIdList = srcImage.PropertyIdList.ToList();
                }

                RotateFlipType rotationFlipType = OrientUpright(propertyIdList, srcImage);

                Tuple<float, float> dimensions = SetDimensions(width, ref imageWidth, ref imageHeight);

                string thumbnailFileName = fullImagePath.Split('\\').Last().ToString();
                var newWidth = (int)dimensions.Item2;
                var newHeight = (int)dimensions.Item1;
                thumbnail = new Bitmap(newWidth, newHeight);
                var graphics = Graphics.FromImage(thumbnail);

                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.RotateTransform(0);

                // Now calculate the X,Y position of the upper-left corner 
                // (one of these will always be zero)
                int posX = Convert.ToInt32((imageWidth - (newWidth)) / 2);
                int posY = Convert.ToInt32((imageHeight - (newHeight)) / 2);

                graphics.DrawImage(srcImage,
                        new Rectangle(posX, posY, newWidth, newHeight),
                        new Rectangle(0, 0, srcImage.Width, srcImage.Height),
                        GraphicsUnit.Pixel);

                thumbnail.RotateFlip(rotationFlipType);

                //Save the thumbnail to the file system.
                string thumbPath = SaveThumbnail(thumbnail, imageFolder, thumbnailFileName);

                //clean up
                srcImage.Dispose();
                thumbnail.Dispose();
                graphics.Dispose();

                return thumbPath;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Calculates the dimensions of the thumbnail and returns them in a tuple with 
        /// height as the first item and width as the second item.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="imageWidth"></param>
        /// <param name="imageHeight"></param>
        /// <returns></returns>
        public Tuple<float, float> SetDimensions(float width, ref float imageWidth, ref float imageHeight)
        {
            if (width < 1) throw new ArgumentException($"The width parameter must be greater than 0.");

            try
            {
                //compute the thumbnail height
                if (imageWidth > imageHeight)
                {
                    //landscape images
                    imageHeight = imageHeight * ((width / imageWidth));
                    imageWidth = width;
                }
                else if (imageWidth < imageHeight)
                {
                    //portrait images
                    imageHeight = imageHeight * ((width / imageWidth));
                    imageWidth = width;
                }
                else
                {
                    imageHeight = width;
                    imageWidth = width;
                }

                return new Tuple<float, float>(imageHeight, imageWidth);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Make sure the image is rotated upright if it has an EXIF orientation set
        /// </summary>
        /// <param name="propertyIdList"></param>
        /// <param name="srcImage"></param>
        /// <returns></returns>
        public RotateFlipType OrientUpright(List<int> propertyIdList, Image srcImage)
        {
            if (srcImage == null) throw new ArgumentNullException(nameof(srcImage));

            try
            {
                int exifOrientationID = 0x112;

                PropertyItem prop = null;

                var ids = srcImage.PropertyIdList.ToList();

                if (ids.Contains(exifOrientationID))
                {
                    prop = srcImage.GetPropertyItem(exifOrientationID);
                }

                var rot = RotateFlipType.RotateNoneFlipNone;

                //determine the orientation of the image from the EXIF orientation ID
                if (prop != null)
                {
                    int val = BitConverter.ToUInt16(prop.Value, 0);

                    if (val == 3 || val == 4)
                        rot = RotateFlipType.Rotate180FlipNone;
                    else if (val == 5 || val == 6)
                        rot = RotateFlipType.Rotate90FlipNone;
                    else if (val == 7 || val == 8)
                        rot = RotateFlipType.Rotate270FlipNone;
                }

                return rot;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Save the thumbnail to a specified file path
        /// </summary>
        /// <param name="thumbnail"></param>
        /// <param name="imagePath"></param>
        /// <param name="thumbnailFileName"></param>
        public string SaveThumbnail(Bitmap thumbnail, string imagePath, string thumbnailFileName)
        {
            if (thumbnail == null) throw new ArgumentNullException(nameof(thumbnail));
            if (string.IsNullOrEmpty(imagePath)) throw new ArgumentNullException(nameof(imagePath));
            if (string.IsNullOrEmpty(thumbnailFileName)) throw new ArgumentNullException(nameof(thumbnailFileName));

            try
            {
                string thumbPath = Path.Combine(imagePath, $"thumb_{thumbnailFileName}");
                thumbnail.Save(thumbPath);
                return thumbPath;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Save the original image to a specified file path. Must be called explicitly from the calling application
        /// if it is desired to keep the original file in addition to the thumbnail.
        /// </summary>
        /// <param name="imageFolder"></param>
        /// <param name="photo"></param>
        /// <returns></returns>
        public string SaveOriginal(string imageFolder, HttpPostedFileBase photo)
        {
            try
            {
                string response = "";

                //check the file size is less than 8MB
                if (photo.ContentLength > (8388608))
                {
                    response = "File size is too large. Must be less than 8MB.";
                }
                else
                {
                    var imageType = photo.ContentType;
                    if (imageType == "image/jpeg"
                        || imageType == "image/jpe"
                        || imageType == "image/jpg"
                        || imageType == "image/tif"
                        || imageType == "image/tiff"
                        || imageType == "image/png"
                        || imageType == "image/bmp"
                        || imageType == "image/png")
                    {
                        string ticks = DateTime.Now.Ticks.ToString()
                        .Replace("/", "")
                        .Replace(":", "")
                        .Replace(".", "")
                        .Replace(" ", "");

                        var fileName = Path.GetFileName($"{ticks}_{photo.FileName}");
                        CheckAndCreateDirectory(imageFolder); //make sure the destination folder exists before attempting to save
                        photo.SaveAs(Path.Combine(imageFolder, fileName));
                        response = Path.Combine(imageFolder, fileName);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
