namespace Rabobank.Training.ClassLibrary
{
    using System.Collections.Generic;
    /// <summary>
    /// This Model holds PostionVM
    /// </summary>
    public class PositionVM
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public decimal Value { get; set; }
        /// <summary>
        /// Gets or sets the mandates.
        /// </summary>
        /// <value>
        /// The mandates.
        /// </value>
        public List<MandateVM> Mandates { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionVM"/> class.
        /// </summary>
        public PositionVM()
        {
            Mandates = new List<MandateVM>();
        }
    }
}
