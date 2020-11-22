namespace Rabobank.Training.WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Rabobank.Training.ClassLibrary;

    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private IDataService DataService;
        private IConfiguration Configuration;
        public PortfolioController(IDataService dataService, IConfiguration configuration)
        {
            this.DataService = dataService;
            this.Configuration = configuration;
        }

        /// <summary>
        /// GetPortfolio : It returns list of positionVM, which contains list of mandates if instrumentcode & code matches between fundofmande and position. 
        /// </summary>
        /// <returns> PortfolioVM : list of PositionVM</returns>
        [HttpGet]
        public PortfolioVM GetPortfolio()
        {
            string path = Configuration.GetValue<string>("FilePath");
            return this.DataService.GetPortfolio(path);
        }
    }
}
