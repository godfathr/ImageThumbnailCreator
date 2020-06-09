using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System;

namespace ImageThumbnailCreator.Tests
{
    [TestClass]
    public class GifThumbnailerUnitTests
    {
        private Thumbnailer _thumbnailer = new Thumbnailer();
        private string ThumbnailFolder = ConfigurationManager.AppSettings["TestDirectory"];

        [TestInitialize]
        public void Setup()
        {
            _thumbnailer.CheckAndCreateDirectory(ThumbnailFolder);
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
            string originalFileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestImages\largeLandscape.gif");

            //act
            _thumbnailer.Create(100, ThumbnailFolder, originalFileLocation);

            string[] images = Directory.GetFiles(ThumbnailFolder);

            //assert
            Assert.IsTrue(images.Length == 1);
            Assert.AreEqual(images.Length, 1);
        }

        [TestMethod]
        public void Create_CreatesThumbnailFromLargeImageFile_CreatesSinglePortraitThumbnail()
        {
            //setup
            string originalFileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestImages\largePortrait.gif");

            //act
            _thumbnailer.Create(100, ThumbnailFolder, originalFileLocation);

            string[] images = Directory.GetFiles(ThumbnailFolder);

            //assert
            Assert.IsTrue(images.Length == 1);
            Assert.AreEqual(images.Length, 1);
        }

        [TestMethod]
        public void Create_CreatesThumbnailFromLargeImageFile_CreatesSingleSquareThumbnail()
        {
            //setup
            string originalFileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestImages\largeSquare.gif");

            //act
            _thumbnailer.Create(100, ThumbnailFolder, originalFileLocation);

            string[] images = Directory.GetFiles(ThumbnailFolder);

            //assert
            Assert.IsTrue(images.Length == 1);
            Assert.AreEqual(images.Length, 1);
        }

        [TestMethod]
        public void Create_WidthGreaterThanOriginal_ReturnsWidthGreaterThanOriginalException()
        {
            //setup
            string originalFileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestImages\largeLandscape.gif");

            //act
            string expectedResponse = _thumbnailer.Create(2000, ThumbnailFolder, originalFileLocation);
            string[] images = Directory.GetFiles(ThumbnailFolder);

            //assert
            Assert.IsTrue(images.Length == 0);
            Assert.AreEqual(images.Length, 0);
            Assert.AreEqual("Thumbnail width can not be greater than the original image width.", expectedResponse);
        }

        [TestMethod]
        public void SetDimensions_GetDimensionsOfLandscapeThumbnail_ReturnsExpectedDimensions()
        {
            //setup
            float width = 100;
            float imageWidth = 1000;
            float imageHeight = 500;

            //act
            Tuple<float, float> calculatedDimensions = _thumbnailer.SetDimensions(width, ref imageWidth, ref imageHeight);

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
            Tuple<float, float> calculatedDimensions = _thumbnailer.SetDimensions(width, ref imageWidth, ref imageHeight);

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
            Tuple<float, float> calculatedDimensions = _thumbnailer.SetDimensions(width, ref imageWidth, ref imageHeight);

            //assert
            //expected height = 100, expected width = 100
            Assert.AreEqual(calculatedDimensions.Item1, (float)100); //height
            Assert.AreEqual(calculatedDimensions.Item2, (float)100); //width
        }

        [TestMethod]
        public void OrientUpright_GetRotateFlipType_ReturnsExpectedRotateFlipType()
        {
            //setup
            string originalFileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestImages\largeLandscape.gif");
            Image srcImage = Image.FromFile(originalFileLocation);
            List<int> propertyIdList = new List<int>();
            if (srcImage.PropertyIdList != null)
            {
                propertyIdList = srcImage.PropertyIdList.ToList();
            }

            //act
            RotateFlipType rotationFlipType = _thumbnailer.OrientUpright(propertyIdList, srcImage);

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
