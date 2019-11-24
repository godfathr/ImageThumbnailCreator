using System;
using System.Collections.Generic;
using System.Drawing;

namespace ImageThumbnailCreator.Interfaces
{
    public interface IThumbnailer
    {
        string Create(float width, string imageFolder, string fullImagePath, long compressionLevel);

        RotateFlipType OrientUpright(List<int> propertyIdList, Image srcImage);

        Tuple<float, float> SetDimensions(float width, ref float imageWidth, ref float imageHeight);
    }
}
