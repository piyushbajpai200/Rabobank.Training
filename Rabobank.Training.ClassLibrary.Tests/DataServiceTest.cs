namespace Rabobank.Training.ClassLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rabobank.Training.ClassLibrary;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;

    /// <summary>
    /// DataService unit test
    /// </summary>
    [TestClass]
    public class DataServiceTest
    {
        /// <summary>
        /// The dataservice
        /// </summary>
        private IDataService dataservice;

        /// <summary>
        /// The path
        /// </summary>
        private string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataServiceTest"/> class.
        /// </summary>
        public DataServiceTest()
        {
            dataservice = new DataService();
        }

        /// <summary>
        /// Sets the path.
        /// </summary>
        [TestInitialize]
        public void SetPath()
        {
            path = @"..\..\..\TestData\FundsOfMandatesData.xml";
        }

        /// <summary>
        /// Gets the fund of mandates should return funds of mandates data when XML has data.
        /// </summary>
        [TestMethod]
        public void GetFundOfMandates_ShouldReturnFundsOfMandatesDataWhenXMLHasData()
        {
            //Act
            FundsOfMandatesData fundsOfMandatesData = dataservice.GetFundOfMandates(path);

            //Assert
            fundsOfMandatesData.Should().NotBeNull();
        }

        /// <summary>
        /// Gets the portfolio should return position vm.
        /// </summary>
        [TestMethod]
        public void GetPortfolio_ShouldReturnPositionVM()
        {
            //Act
            PortfolioVM portfolioVM = dataservice.GetPortfolio(path);

            //Assert
            portfolioVM.Should().NotBeNull();
        }

        /// <summary>
        /// Gets the portfolio should match position count.
        /// </summary>
        [TestMethod]
        public void GetPortfolio_ShouldMatchPositionCount()
        {
            //Arrange
            int positionsCount = 5;

            //Act
            int actualPostionsCount = dataservice.GetPortfolio(path).Positions.Count;

            //Assert
            actualPostionsCount.Should().Be(positionsCount);
        }

        /// <summary>
        /// Calculates the mandate should not return mandate when instrument code does not match.
        /// </summary>
        [TestMethod]
        public void CalculateMandate_ShouldNotReturnMandate_WhenInstrumentCodeDoesNotMatch()
        {
            //Arrange
            FundsOfMandatesData fundsOfMandatesData = dataservice.GetFundOfMandates(path);
            string positioncode = "NL0000009165";
            decimal positionValue = 12345;
            int expectedCount = 0;

            //Act
            int actualMandateCount = dataservice.CalculateMandate(positioncode, positionValue, fundsOfMandatesData).Count;

            //Assert
            actualMandateCount.Should().Be(expectedCount);
        }

        /// <summary>
        /// Calculates the mandate should return mandate when instrument code matches.
        /// </summary>
        [TestMethod]
        public void CalculateMandate_ShouldReturnMandate_WhenInstrumentCodeMatches()
        {
            //Arrange
            FundsOfMandatesData fundsOfMandatesData = dataservice.GetFundOfMandates(path);
            string positioncode = "NL0000287100";
            decimal positionValue = 23456;
            int expectedCount = 4;

            //Act
            int actualMandateCount = dataservice.CalculateMandate(positioncode, positionValue, fundsOfMandatesData).Count;

            //Assert
            actualMandateCount.Should().Be(expectedCount);
        }

        /// <summary>
        /// Calculates the mandate should match mandate value when instrument code matches.
        /// </summary>
        [TestMethod]
        public void CalculateMandate_ShouldMatchMandateValue_WhenInstrumentCodeMatches()
        {
            //Arrange
            FundsOfMandatesData fundsOfMandatesData = dataservice.GetFundOfMandates(path);
            string positioncode = "NL0000287100";
            decimal positionValue = 23456;
            List<MandateVM> expectedMandateVM = new List<MandateVM>()
                                               { new MandateVM() { Name = "Robeco Factor Momentum Mandaat", Allocation = (decimal)0.355, Value = 8327 },
                                                 new MandateVM() { Name = "BNPP Factor Value Mandaat", Allocation = (decimal)0.383, Value = 8984 },
                                                 new MandateVM() { Name = "Robeco Factor Quality Mandaat", Allocation = (decimal)0.261, Value = 6122 },
                                                 new MandateVM() { Name = "Liquidity", Allocation = (decimal)0.001, Value = 23 },
                                                };
            //Act
            List<MandateVM> actualMandateVM = dataservice.CalculateMandate(positioncode, positionValue, fundsOfMandatesData);

            //Assert
            actualMandateVM.Should().NotBeNull();
            expectedMandateVM.Count.Should().Be(actualMandateVM.Count);
            foreach (var mandate in actualMandateVM)
            {
                expectedMandateVM.Where(x => x.Name == mandate.Name).Select(x => x.Value).FirstOrDefault().Should().Be(mandate.Value);
            }
        }

        /// <summary>
        /// Fills the mandate should not add mandate for empty funds of mandate data when funds of mandate is empty.
        /// </summary>
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
                position.Mandates.Count.Should().Be(0);
            }
        }

        /// <summary>
        /// Fills the mandate should add mandate when instrument code matches.
        /// </summary>
        [TestMethod]
        public void FillMandate_ShouldAddMandate_WhenInstrumentCodeMatches()
        {
            //Arrange
            PortfolioVM portfolioVM = new PortfolioVM();
            portfolioVM.Positions.Add(new PositionVM() { Code = "NL0000287100", Name = "Optimix Mix Fund", Value = 23456 });
            FundsOfMandatesData fundsOfMandatesData = dataservice.GetFundOfMandates(path);
            int expectedMandateCount = 4;

            //Act
            int actualMandateCount = dataservice.FillMandate(portfolioVM, fundsOfMandatesData).Positions[0].Mandates.Count;

            //Assert
            actualMandateCount.Should().Be(expectedMandateCount);
        }

        /// <summary>
        /// Fills the mandate should add mandate when instrument code matches.
        /// </summary>
        [TestMethod]
        public void FillMandate_ShouldMatchMandates_WhenInstrumentCodeMatches()
        {
            //Arrange
            PortfolioVM portfolioVM = new PortfolioVM();
            portfolioVM.Positions.Add(new PositionVM() { Code = "NL0000287100", Name = "Optimix Mix Fund", Value = 23456 });
            List<MandateVM> expectedListOfMandates = new List<MandateVM>()
                                               { new MandateVM() { Name = "Robeco Factor Momentum Mandaat", Allocation = (decimal)0.355, Value = 8327 },
                                                 new MandateVM() { Name = "BNPP Factor Value Mandaat", Allocation = (decimal)0.383, Value = 8984 },
                                                 new MandateVM() { Name = "Robeco Factor Quality Mandaat", Allocation = (decimal)0.261, Value = 6122 },
                                                 new MandateVM() { Name = "Liquidity", Allocation = (decimal)0.001, Value = 23 },
                                                };
            FundsOfMandatesData fundsOfMandatesData = dataservice.GetFundOfMandates(path);

            //Act
            List<MandateVM> actualListOfMandates = dataservice.FillMandate(portfolioVM, fundsOfMandatesData).Positions[0].Mandates;
            MandateVM actualMandateVM = actualListOfMandates.Where(x=>x.Name == "Robeco Factor Momentum Mandaat").FirstOrDefault();
            //Assert
            actualListOfMandates.Should().BeEquivalentTo(expectedListOfMandates);
            actualMandateVM.Should().BeEquivalentTo(new MandateVM() { Name = "Robeco Factor Momentum Mandaat", Allocation = (decimal)0.355, Value = 8327 });
        }
    }
}
