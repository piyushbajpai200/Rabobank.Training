namespace Rabobank.Training.ClassLibrary
{
    using System.Collections.Generic;
    public interface IDataService
    {
        public FundsOfMandatesData GetFundOfMandates(string path);
        public PortfolioVM GetPortfolio(string path);
        public PortfolioVM FillMandate(PortfolioVM portfolioVM, FundsOfMandatesData fundsOfMandatesData);
        public List<MandateVM> CalculateMandate(string positionCode, decimal positionValue, FundsOfMandatesData fundsOfMandatesData);
    }
}
