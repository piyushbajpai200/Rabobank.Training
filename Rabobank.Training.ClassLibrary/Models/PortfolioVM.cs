namespace Rabobank.Training.ClassLibrary
{
    using System.Collections.Generic;
    /// <summary>
    /// This Model holds Portfolio
    /// </summary>
    public class PortfolioVM
    {
        /// <summary>
        /// Gets or sets the positions.
        /// </summary>
        /// <value>
        /// The positions.
        /// </value>
        public List<PositionVM> Positions { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PortfolioVM"/> class.
        /// </summary>
        public PortfolioVM()
        {
            Positions = new List<PositionVM>();
        }
    }
}
