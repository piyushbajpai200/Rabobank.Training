namespace Rabobank.Training.WebApp.Tests
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rabobank.Training.ClassLibrary;
    using Rabobank.Training.WebApp.Controllers;

    [TestClass]
    public class PortfolioControllerTest
    {
        [TestMethod]
        public void GetPortfolio_ShouldReturnPortfolio()
        {
            //Arrange
            var dataServiceMock = new Mock<IDataService>();
            PortfolioVM portfolioVM = new PortfolioVM();
            dataServiceMock.Setup(x=>x.GetPortfolio(It.IsAny<string>())).Returns(portfolioVM);
            
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(x => x.GetSection(It.IsAny<string>())).Returns(new Mock<IConfigurationSection>().Object);
            var PortfolioController = new PortfolioController(dataServiceMock.Object, configurationMock.Object);

            //Act
            var result = PortfolioController.GetPortfolio();

            //Assert
            Assert.AreEqual(portfolioVM, result);
        }
    }
}
