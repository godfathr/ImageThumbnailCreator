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

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(ThumbnailFolder);
        }
    }
}
