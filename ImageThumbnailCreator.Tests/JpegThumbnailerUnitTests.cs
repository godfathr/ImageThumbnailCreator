using System;
using System.Configuration;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageThumbnailCreator.Tests
{
    [TestClass]
    public class JpegThumbnailerUnitTests
    {
        private JpegThumbnailer _jpegThumbnailer = new JpegThumbnailer();
        private string ThumbnailFolder = ConfigurationSettings.AppSettings["TestDirectory"];

        [TestInitialize]
        public void Setup()
        {
            //TODO: Add any setup steps here
            _jpegThumbnailer.CheckAndCreateDirectory(ThumbnailFolder);
        }

        [TestMethod]
        public void CheckAndCreateDirectory_DirectoryDoesNotExist_SuccessfullyCreatesDirectory()
        {
            //setup

            //act            
            var createdFolder = Directory.Exists(ThumbnailFolder);

            //assert
            Assert.IsTrue(createdFolder);
        }

        [TestMethod]
        public void Create_CreatesThumbnailFromLargeImageFile_CreatesSingleLandscapeThumbnail()
        {
            //setup
            string originalFileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestImages\largeLandscape.jpg");

            //act
            _jpegThumbnailer.Create(100, ThumbnailFolder, originalFileLocation);

            string[] images = Directory.GetFiles(ThumbnailFolder);

            //assert
            Assert.IsTrue(images.Length == 1);
            Assert.AreEqual(images.Length, 1);
        }

        [TestCleanup]
        public void Cleanup()
        {
            DirectoryInfo di = new DirectoryInfo(ThumbnailFolder);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            Directory.Delete(ThumbnailFolder);
        }
    }
}
