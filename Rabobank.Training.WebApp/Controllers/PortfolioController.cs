namespace Rabobank.Training.WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Rabobank.Training.ClassLibrary;
    using System;

    /// <summary>
    /// Portfolio api controller 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        /// <summary>
        /// The portfolio service
        /// </summary>
        private IPortfolioServices PortfolioServices;

        /// <summary>
        /// The configuration
        /// </summary>
        private IConfiguration Configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="PortfolioController"/> class.
        /// </summary>
        /// <param name="portfolioServices">The portfolio service.</param>
        /// <param name="configuration">The configuration.</param>
        public PortfolioController(IPortfolioServices portfolioServices, IConfiguration configuration)
        {
            this.PortfolioServices = portfolioServices;
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the portfolio.
        /// </summary>
        /// <returns>returns portfolio view model</returns>
        [HttpGet]
        public IActionResult GetPortfolio()
        {
            try
            {
                string path = Configuration.GetValue<string>("FilePath");
                return Ok(this.PortfolioServices.GetPortfolio(path));
            }
            catch
            {
                throw new Exception("something went wrong, please try after sometime");
            }
            
        }
    }
}
