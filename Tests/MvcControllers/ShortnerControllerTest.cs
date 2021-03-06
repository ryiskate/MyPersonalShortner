﻿using System.Web.Mvc;
using NUnit.Framework;
using MyPersonalShortner.MvcApp.Controllers;
using Moq;
using MyPersonalShortner.Lib.Services;

namespace MyPersonalShortner.Tests.MvcControllers
{
    [TestFixture]
    public class ShortnerControllerTest
    {
        [Test]
        public void index_with_parameters_redirects_permanently()
        {
            var controller = GetController();
            var result = controller.Index("test");
            Assert.IsInstanceOf<RedirectResult>(result);
            Assert.IsTrue(((RedirectResult)result).Permanent);
        }

        private static ShortnerController GetController()
        {
            var mockService = new Mock<IShortnerService>();
            mockService.Setup(s => s.Expand(It.IsAny<string>())).Returns("http://github.com");

            var controller = new ShortnerController(mockService.Object);
            return controller;
        }
    }
}
