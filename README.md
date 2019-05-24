# ImageThumbnailCreator
Create image thumbnails from uploaded image files to help downsize large resolution images that need to be displayed on your website in a smaller resolution.

Excellent addition for custom blog sites or any kind of site that allows image files to be uploaded. You can downsize uploaded images as well as keep the original image if you want!

*Original uploaded images can be saved to the file system. *Thumbnails will be saved to the file system automatically.

Example MVC application is located at https://github.com/godfathr/ThumbnailWebExample

#Version 2.0.0 Updates
Renamed "JpegThumbnailer" to "Thumbnailer".
Added tests for PNG, GIF, TIF and BMP image formats. 
Added validation that prevents upsizing. 
Removed validation that would break the resizing if the property list had 0 items. 

#Version 1.0.2 Updates 
Added unit tests.

Added parameter checking with meaningful exceptions.