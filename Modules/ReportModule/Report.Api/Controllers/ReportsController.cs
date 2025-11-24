using Microsoft.AspNetCore.Mvc;
using Report.Application.Contracts.Persistence.Repositories;
using Report.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsRepository _reportRepository;

        public ReportsController(IReportsRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        /// <summary>
        /// Gets all reports
        /// </summary>
        /// <returns>A list of all reports</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Report.Domain.Entities.Report>>> GetAllReports()
        {
            var reports = await _reportRepository.GetAllAsync();
            return Ok(reports);
        }

        /// <summary>
        /// Gets a report by its ID
        /// </summary>
        /// <param name="id">The ID of the report to retrieve</param>
        /// <returns>The report with the specified ID</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Report.Domain.Entities.Report>> GetReportById(int id)
        {
            var report = await _reportRepository.GetByIdAsync(id);
            
            if (report == null)
            {
                return NotFound($"Report with ID {id} not found.");
            }

            return Ok(report);
        }
    }
}