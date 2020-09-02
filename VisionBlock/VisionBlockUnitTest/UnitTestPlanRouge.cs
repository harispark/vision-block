//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using VisionBlockApplication.Models;
//using System.Windows.Media.Imaging;
//using VisionBlockApplication.Models.Block;

//namespace VisionBlockUnitTest
//{
//    [TestClass]
//    public class UnitTestPlanRouge
//    {
//        public readonly Model_Block_Plan blockPlan;
//        WriteableBitmap bmpIn = new WriteableBitmap(new BitmapImage(new Uri("pack://application:,,,/resources/images/imgtest/cervin.jpg", System.UriKind.Absolute)));
//        WriteableBitmap bmpResult = new WriteableBitmap(new BitmapImage(new Uri("pack://application:,,,/resources/images/imgtest/cervin-rouge.jpg", System.UriKind.Absolute)));
//        [TestMethod]
//        public void TestDoImageProcessing()
//        {
//            WriteableBitmap bmpToTest = blockPlan.DoImageProcessing(bmpIn);
//            Assert.Equals(bmpResult, bmpToTest);
//        }
//    }
//}
