namespace Rabobank.Training.WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Rabobank.Training.ClassLibrary;

    /// <summary>
    /// Portfolio api controller 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        /// <summary>
        /// The data service
        /// </summary>
        private IDataService DataService;

        /// <summary>
        /// The configuration
        /// </summary>
        private IConfiguration Configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="PortfolioController"/> class.
        /// </summary>
        /// <param name="dataService">The data service.</param>
        /// <param name="configuration">The configuration.</param>
        public PortfolioController(IDataService dataService, IConfiguration configuration)
        {
            this.DataService = dataService;
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the portfolio.
        /// </summary>
        /// <returns>returns portfolio view model</returns>
        [HttpGet]
        public PortfolioVM GetPortfolio()
        {
            string path = Configuration.GetValue<string>("FilePath");
            return this.DataService.GetPortfolio(path);
        }
    }
}
