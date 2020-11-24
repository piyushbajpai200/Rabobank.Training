namespace Rabobank.Training.ClassLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rabobank.Training.ClassLibrary;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class DataServiceTest
    {
        private IDataService dataservice;
        private string path;

        public DataServiceTest()
        {
            dataservice = new DataService();
        }

        [TestInitialize]
        public void SetPath()
        {
            path = @"..\..\..\TestData\FundsOfMandatesData.xml";
        }

        [TestMethod]
        public void GetFundOfMandates_ShouldReturnFundsOfMandatesDataWhenXMLHasData()
        {
            //Act
            FundsOfMandatesData fundsOfMandatesData = dataservice.GetFundOfMandates(path);

            //Assert
            Assert.IsNotNull(fundsOfMandatesData);
        }

        [TestMethod]
        public void GetPortfolio_ShouldReturnPositionVM()
        {
            //Act
            PortfolioVM PortfolioVM = dataservice.GetPortfolio(path);

            //Assert
            Assert.IsNotNull(PortfolioVM);
        }

        [TestMethod]
        public void GetPortfolio_ShouldMatchPositionCount()
        {
            //Arrange
            int positionsCount = 5;
           
            //Act
            PortfolioVM actualPortfolio = dataservice.GetPortfolio(path);

            //Assert
            Assert.AreEqual(positionsCount, actualPortfolio.Positions.Count);
        }

        [TestMethod]
        public void CalculateMandate_ShouldNotReturnMandate_WhenInstrumentCodeDoesNotMatch()
        {
            //Arrange
            FundsOfMandatesData fundsOfMandatesData = dataservice.GetFundOfMandates(path);
            string positioncode = "NL0000009165";
            decimal positionValue = 12345;
            int expectedCount = 0;

            //Act
            List<MandateVM> result = dataservice.CalculateMandate(positioncode, positionValue, fundsOfMandatesData);

            //Assert
            Assert.AreEqual(result.Count, expectedCount);
        }

        [TestMethod]
        public void CalculateMandate_ShouldReturnMandate_WhenInstrumentCodeMatches()
        {
            //Arrange
            FundsOfMandatesData fundsOfMandatesData = dataservice.GetFundOfMandates(path);
            string positioncode = "NL0000287100";
            decimal positionValue = 23456;
            int expectedCount = 4;

            //Act
            List<MandateVM> result = dataservice.CalculateMandate(positioncode, positionValue, fundsOfMandatesData);

            //Assert
            Assert.AreEqual(result.Count, expectedCount);
        }

        [TestMethod]
        public void CalculateMandate_ShouldMatchMandateValue_WhenInstrumentCodeMatches()
        {
            //Arrange
            FundsOfMandatesData fundsOfMandatesData = dataservice.GetFundOfMandates(path);
            string positioncode = "NL0000287100";
            decimal positionValue = 23456;
            List<MandateVM> expectedMandateVM = new List<MandateVM>()
                                               { new MandateVM() { Name = "Robeco Factor Momentum Mandaat", Allocation = (decimal)35.5, Value = 8327 },
                                                 new MandateVM() { Name = "BNPP Factor Value Mandaat", Allocation = (decimal)38.3, Value = 8984 },
                                                 new MandateVM() { Name = "Robeco Factor Quality Mandaat", Allocation = (decimal)26.1, Value = 6122 },
                                                 new MandateVM() { Name = "Liquidity", Allocation = (decimal)0.1, Value = 23 },
                                                };
            //Act
            List<MandateVM> actualMandateVM = dataservice.CalculateMandate(positioncode, positionValue, fundsOfMandatesData);

            //Assert
            Assert.IsNotNull(actualMandateVM);
            Assert.IsTrue(expectedMandateVM.Count == actualMandateVM.Count);
            foreach (var mandate in actualMandateVM)
            {
                Assert.AreEqual(expectedMandateVM.Where(x => x.Name == mandate.Name).Select(x => x.Value).FirstOrDefault(), mandate.Value);
            }
        }

        [TestMethod]
        public void FillMandate_ShouldNotAddMandateForEmptyFundsOfMandateData_WhenFundsOfMandateIsEmpty()
        {
            //Arrange
            PortfolioVM portfolioVM = new PortfolioVM();
            portfolioVM.Positions.Add(new PositionVM() { Code = "NL0000009165", Name = "Heineken", Value = 12345 });
            portfolioVM.Positions.Add(new PositionVM() { Code = "NL0000287100", Name = "Optimix Mix Fund", Value = 23456 });
            portfolioVM.Positions.Add(new PositionVM() { Code = "LU0035601805", Name = "DP Global Strategy L High", Value = 34567 });
            portfolioVM.Positions.Add(new PositionVM() { Code = "NL0000292332", Name = "Rabobank Core Aandelen Fonds T2", Value = 45678 });
            portfolioVM.Positions.Add(new PositionVM() { Code = "LU0042381250", Name = "Morgan Stanley Invest US Gr Fnd", Value = 56789 });
            FundsOfMandatesData fundsOfMandatesData = new FundsOfMandatesData();

            //Act
            portfolioVM = dataservice.FillMandate(portfolioVM, fundsOfMandatesData);

            //Assert
            foreach (var position in portfolioVM.Positions)
            {
                Assert.AreEqual(position.Mandates.Count, 0);
            }
        }

        [TestMethod]
        public void FillMandate_ShouldAddMandate_WhenInstrumentCodeMatches()
        {
            //Arrange
            PortfolioVM portfolioVM = new PortfolioVM();
            portfolioVM.Positions.Add(new PositionVM() { Code = "NL0000287100", Name = "Optimix Mix Fund", Value = 23456 });
            FundsOfMandatesData fundsOfMandatesData = dataservice.GetFundOfMandates(path);
            int expectedMandateCount = 4;

            //Act
            portfolioVM = dataservice.FillMandate(portfolioVM, fundsOfMandatesData);

            //Assert
            Assert.AreEqual(portfolioVM.Positions[0].Mandates.Count, expectedMandateCount);
        }
    }
}
