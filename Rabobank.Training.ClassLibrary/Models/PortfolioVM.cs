namespace Rabobank.Training.ClassLibrary
{
    using System.Collections.Generic;
    public class PortfolioVM
    {
        public List<PositionVM> Positions { get; set; }
        public PortfolioVM()
        {
            Positions = new List<PositionVM>();
        }
    }
}
