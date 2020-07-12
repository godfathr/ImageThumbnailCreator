# Warning
**This NuGet package will no longer be updated as of the latest commit.**

**A .NET Core Version will be available in lieu of the full ASP.NET Framework**

# ImageThumbnailCreator
Create image thumbnails from uploaded image files to help downsize large resolution images that need to be displayed on your website in a smaller resolution.

Excellent addition for custom blog sites or any kind of site that allows image files to be uploaded. You can downsize uploaded images as well as keep the original image if you want!

*Original uploaded images can be saved to the file system. *Thumbnails will be saved to the file system automatically.

Example MVC application is located at https://github.com/godfathr/ThumbnailWebExample

## Version 3.0.0 Updates
Changed .NET framework to 4.8. Updated license from Apache to MIT. Updated README.md.

No updates to existing code are needed. **Default value is set in NuGet package**

### How to use (example)
```
private Thumbnailer _thumbnailer = new Thumbnailer();
private string ThumbnailFolder = ConfigurationManager.AppSettings["TestDirectory"];
```

```
//Save the original photo at its original resolution
HttpPostedFileBase photo = Request.Files[0];
var originalImage = thumbnailer.SaveOriginal(uploadPath, photo);
```

```
//With specified compression level of 50
string thumbnailPath = _thumbnailer.Create(100, ThumbnailFolder, originalFileLocation, 50L); 
```

OR if a compression value is not provided, the default 85L will be used
```
string thumbnailPath = _thumbnailer.Create(100, ThumbnailFolder, originalFileLocation);
```

## Version 2.0.1 Updates
Added optional image compression parameter. Max value is 100. Min value is 0. Default value is 85. Parameter type is `long`.

No updates to existing code are needed. **Default value is set in NuGet package**
```
public string Create(float width, string imageFolder, string fullImagePath, long compressionLevel = 85L)
{
    //method content here...
}
```

```
private Thumbnailer _thumbnailer = new Thumbnailer();
private string ThumbnailFolder = ConfigurationSettings.AppSettings["TestDirectory"];

_thumbnailer.Create(100, ThumbnailFolder, originalFileLocation, 50L); //With specified compression level of 50
```

OR if a compression value is not provided, the default 85L will be used
```
_thumbnailer.Create(100, ThumbnailFolder, originalFileLocation);
```

## Version 2.0.0 Updates
Renamed "JpegThumbnailer" to "Thumbnailer".
Added tests for PNG, GIF, TIF and BMP image formats. 
Added validation that prevents upsizing. 
Removed validation that would break the resizing if the property list had 0 items. 

## Version 1.0.2 Updates 
Added unit tests.

Added parameter checking with meaningful exceptions.
