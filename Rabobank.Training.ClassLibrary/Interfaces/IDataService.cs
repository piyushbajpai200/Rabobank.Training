namespace Rabobank.Training.ClassLibrary
{
    using System.Collections.Generic;

    /// <summary>
    ///  Interface for loose coupling dataservice.
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// Gets the fund of mandates.
        /// </summary>
        /// <param name="path">path to read data.</param>
        /// <returns>FundsOfMandatesData</returns>
        public FundsOfMandatesData GetFundOfMandates(string path);

        /// <summary>
        /// Gets the portfolio.
        /// </summary>
        /// <param name="path">path to read data</param>
        /// <returns>PortfolioVM</returns>
        public PortfolioVM GetPortfolio(string path);

        /// <summary>
        /// Fills the mandate.
        /// </summary>
        /// <param name="portfolioVM">The portfolio vm.</param>
        /// <param name="fundsOfMandatesData">The funds of mandates data.</param>
        /// <returns> PortfolioVM</returns>
        public PortfolioVM FillMandate(PortfolioVM portfolioVM, FundsOfMandatesData fundsOfMandatesData);

        /// <summary>
        /// Calculates the mandate.
        /// </summary>
        /// <param name="positionCode">The position code.</param>
        /// <param name="positionValue">The position value.</param>
        /// <param name="fundsOfMandatesData">The funds of mandates data.</param>
        /// <returns> list of MandateVM</returns>
        public List<MandateVM> CalculateMandate(string positionCode, decimal positionValue, FundsOfMandatesData fundsOfMandatesData);
    }
}
