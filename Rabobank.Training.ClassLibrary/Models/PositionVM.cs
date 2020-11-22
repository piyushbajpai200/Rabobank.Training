namespace Rabobank.Training.ClassLibrary
{
    using System.Collections.Generic;
    public class PositionVM
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public List<MandateVM> Mandates { get; set; }
        public PositionVM()
        {
            Mandates = new List<MandateVM>();
        }
    }
}
