namespace Rabobank.Training.WebApp.Tests
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rabobank.Training.ClassLibrary;
    using Rabobank.Training.WebApp.Controllers;
    using FluentAssertions;
    using System;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Class used to test PortfolioController.
    /// </summary>
    [TestClass]
    public class PortfolioControllerTest
    {
        private Mock<IPortfolioServices> portfolioServicesMock;
        private Mock<IConfiguration> configurationMock;
        private PortfolioVM portfolioVM;
        /// <summary>
        /// Mock Portfolio service.
        /// </summary>
        [TestInitialize]
        public void MockPortfolioService()
        {
            portfolioServicesMock = new Mock<IPortfolioServices>();
            portfolioVM = new PortfolioVM();
            portfolioServicesMock.Setup(x => x.GetPortfolio(It.IsAny<string>())).Returns(portfolioVM);

            configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(x => x.GetSection(It.IsAny<string>())).Returns(new Mock<IConfigurationSection>().Object);
        }

        /// <summary>
        /// Test GetPortfolio : should return portfoliovm.
        /// </summary>
        [TestMethod]
        public void GetPortfolio_ShouldReturnOk()
        {
            //Arrange
            
            var PortfolioController = new PortfolioController(portfolioServicesMock.Object, configurationMock.Object);

            //Act
            var expectedPortfolioVM =  PortfolioController.GetPortfolio();
            //Assert
            expectedPortfolioVM.Should().NotBeNull();
            expectedPortfolioVM.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public void GetPortfolio_ShouldRetrunExceptionIFPathIsNotDefined()
        {
            //Arrange
            configurationMock.Setup(x => x.GetSection(It.IsAny<string>())).Throws(new Exception("Path not found"));
            var PortfolioController = new PortfolioController(portfolioServicesMock.Object, configurationMock.Object);

            //Act
            Func<IActionResult> expectedPortfolioVM = () => PortfolioController.GetPortfolio();

            //Assert
            expectedPortfolioVM.Should().Throw<Exception>("something went wrong, please try after sometime").WithMessage("something went wrong, please try after sometime")
                .And.Should().NotBeAssignableTo(typeof(IActionResult));
        }
    }
}
