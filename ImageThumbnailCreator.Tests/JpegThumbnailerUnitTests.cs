using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
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

        [TestMethod]
        public void Create_CreatesThumbnailFromLargeImageFile_CreatesSinglePortraitThumbnail()
        {
            //setup
            string originalFileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestImages\largePortrait.jpg");

            //act
            _jpegThumbnailer.Create(100, ThumbnailFolder, originalFileLocation);

            string[] images = Directory.GetFiles(ThumbnailFolder);

            //assert
            Assert.IsTrue(images.Length == 1);
            Assert.AreEqual(images.Length, 1);
        }

        [TestMethod]
        public void Create_CreatesThumbnailFromLargeImageFile_CreatesSingleSquareThumbnail()
        {
            //setup
            string originalFileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestImages\largeSquare.jpg");

            //act
            _jpegThumbnailer.Create(100, ThumbnailFolder, originalFileLocation);

            string[] images = Directory.GetFiles(ThumbnailFolder);

            //assert
            Assert.IsTrue(images.Length == 1);
            Assert.AreEqual(images.Length, 1);
        }

        [TestMethod]
        public void SetDimensions_GetDimensionsOfLandscapeThumbnail_ReturnsExpectedDimensions()
        {
            //setup
            float width = 100;
            float imageWidth = 1000;
            float imageHeight = 500;

            //act
            Tuple<float, float> calculatedDimensions = _jpegThumbnailer.SetDimensions(width, ref imageWidth, ref imageHeight);

            //assert
            //expected height = 50, expected width = 100
            Assert.AreEqual(calculatedDimensions.Item1, (float)50); //height
            Assert.AreEqual(calculatedDimensions.Item2, (float)100); //width
        }

        [TestMethod]
        public void SetDimensions_GetDimensionsOfPortraitThumbnail_ReturnsExpectedDimensions()
        {
            //setup
            float width = 100;
            float imageWidth = 500;
            float imageHeight = 1000;

            //act
            Tuple<float, float> calculatedDimensions = _jpegThumbnailer.SetDimensions(width, ref imageWidth, ref imageHeight);

            //assert
            //expected height = 100, expected width = 200
            Assert.AreEqual(calculatedDimensions.Item1, (float)200); //height
            Assert.AreEqual(calculatedDimensions.Item2, (float)100); //width
        }

        [TestMethod]
        public void SetDimensions_GetDimensionsOfSquareThumbnail_ReturnsExpectedDimensions()
        {
            //setup
            float width = 100;
            float imageWidth = 500;
            float imageHeight = 500;

            //act
            Tuple<float, float> calculatedDimensions = _jpegThumbnailer.SetDimensions(width, ref imageWidth, ref imageHeight);

            //assert
            //expected height = 100, expected width = 100
            Assert.AreEqual(calculatedDimensions.Item1, (float)100); //height
            Assert.AreEqual(calculatedDimensions.Item2, (float)100); //width
        }

        [TestMethod]
        public void OrientUpright_GetRotateFlipType_ReturnsExpectedRotateFlipType()
        {
            //setup
            string originalFileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestImages\largeLandscape.jpg");
            Image srcImage = Image.FromFile(originalFileLocation);
            List<int> propertyIdList = new List<int>();
            if (srcImage.PropertyIdList != null)
            {
                propertyIdList = srcImage.PropertyIdList.ToList();
            }

            //act
            RotateFlipType rotationFlipType = _jpegThumbnailer.OrientUpright(propertyIdList, srcImage);

            //assert
            Assert.AreEqual(rotationFlipType, RotateFlipType.Rotate180FlipXY);
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
