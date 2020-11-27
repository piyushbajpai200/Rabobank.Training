namespace Rabobank.Training.ClassLibrary
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// This class used to access data.
    /// </summary>
    /// <seealso cref="Rabobank.Training.ClassLibrary.IPortfolioServices" />
    public class PortfolioServices : IPortfolioServices
    {
        /// <summary>
        /// This Method accepts xml file path and serialize data into  FundsOfMandatesData
        /// </summary>
        /// <param name="path">XML file path</param>
        /// <returns>
        /// Funds of Mandates data
        /// </returns>
        
        public FundsOfMandatesData GetFundOfMandates(string path)
        {
            XmlSerializer reader = new XmlSerializer(typeof(FundsOfMandatesData));
            StreamReader file = new StreamReader(path);
            return (FundsOfMandatesData)reader.Deserialize(file);
        }

        /// <summary>
        /// This Method accepts xml file path and returns PortfolioVM.
        /// Portfolio view model contains static position view model, calls fillMandate method which fills mandate object inside postion view model.
        /// </summary>
        /// <param name="path">XML file path</param>
        /// <returns> returns portfolioVM</returns>
        public PortfolioVM GetPortfolio(string path)
        {
            FundsOfMandatesData fundsOfMandatesData = this.GetFundOfMandates(path);
            PortfolioVM portfolioVM = new PortfolioVM();
            portfolioVM.Positions.Add(new PositionVM() { Code = "NL0000009165", Name = "Heineken", Value = 12345 });
            portfolioVM.Positions.Add(new PositionVM() { Code = "NL0000287100", Name = "Optimix Mix Fund", Value = 23456 });
            portfolioVM.Positions.Add(new PositionVM() { Code = "LU0035601805", Name = "DP Global Strategy L High", Value = 34567 });
            portfolioVM.Positions.Add(new PositionVM() { Code = "NL0000292332", Name = "Rabobank Core Aandelen Fonds T2", Value = 45678 });
            portfolioVM.Positions.Add(new PositionVM() { Code = "LU0042381250", Name = "Morgan Stanley Invest US Gr Fnd", Value = 56789 });
            return this.FillMandate(portfolioVM, fundsOfMandatesData);
        }

        /// <summary>
        /// Calculates Mandates of perticular postion, considering position code should match with instrument code of fundsofmandate data
        /// if position code doesn't match with any of instrument code then do nothing.
        /// </summary>
        /// <param name="positionCode">The position code.</param>
        /// <param name="positionValue">The position value.</param>
        /// <param name="fundsOfMandatesData">List of fundsofmandate</param>
        /// <returns>returns list of mandateVM</returns>
        public List<MandateVM> CalculateMandate(string positionCode, decimal positionValue, FundsOfMandatesData fundsOfMandatesData)
        {
            List<MandateVM> mandates = new List<MandateVM>();
            foreach (var fundOfMandates in fundsOfMandatesData?.FundsOfMandates)
            {
                if (fundOfMandates.InstrumentCode.Equals(positionCode))
                {
                    decimal liqudityAllocation = 1;
                    decimal liquidityValue = positionValue;
                    foreach (var mandate in fundOfMandates.Mandates)
                    {
                        mandates.Add(new MandateVM() { Name = mandate.MandateName, Allocation = mandate.Allocation / 100, Value = Math.Round((mandate.Allocation * positionValue) / 100) });
                        liqudityAllocation = liqudityAllocation - mandate.Allocation / 100;
                        liquidityValue = liquidityValue - Math.Round((mandate.Allocation * positionValue) / 100);
                    }
                    if (fundOfMandates.LiquidityAllocation > 0)
                    {
                        MandateVM liquidityMandateVM = new MandateVM() { Allocation = liqudityAllocation, Name = "Liquidity", Value = liquidityValue };
                        mandates.Add(liquidityMandateVM);
                    }
                }
            }
            return mandates;
        }

        /// <summary>
        /// This function calls calculateMandate and updates mandates object for each postion item.
        /// if fundsofMandateData or portfolioVM is null or empty then it is not doing anything, returning same portfoliovm object which was passed as argument.
        /// </summary>
        /// <param name="portfolioVM">The portfolio vm.</param>
        /// <param name="fundsOfMandatesData">The funds of mandates data.</param>
        /// <returns>returns portfolioVM</returns>
        public PortfolioVM FillMandate(PortfolioVM portfolioVM, FundsOfMandatesData fundsOfMandatesData)
        {
            if (fundsOfMandatesData?.FundsOfMandates?.Length > 0 && portfolioVM?.Positions?.Count > 0)
            {
                foreach (var position in portfolioVM.Positions)
                {
                    position.Mandates = CalculateMandate(position.Code, position.Value, fundsOfMandatesData);
                }
            }
            return portfolioVM;
        }
    }
}
