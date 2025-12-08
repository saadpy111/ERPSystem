using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Report.Application.Contracts.Persistence.Repositories;
using Report.Application.DTOs;
using Report.Application.Features.ReportFeatures.RunReport;
using Report.Application.Features.ReportFeatures.RunReportWithAiPrompet;
using Report.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Report")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsRepository _reportRepository;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ReportsController(IReportsRepository reportRepository, IMediator mediator, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all reports
        /// </summary>
        /// <returns>A list of all reports</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetAllReports()
        {
            var reports = await _reportRepository.GetAllAsync();
            var reportDtos = _mapper.Map<IEnumerable<ReportDto>>(reports);
            return Ok(reportDtos);
        }

        /// <summary>
        /// Gets a report by its ID
        /// </summary>
        /// <param name="id">The ID of the report to retrieve</param>
        /// <returns>The report with the specified ID</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ReportDto>> GetReportById(int id)
        {
            var report = await _reportRepository.GetFullReportAsync(id);

            if (report == null)
            {
                return NotFound($"Report with ID {id} not found.");
            }

            var reportDto = _mapper.Map<ReportDto>(report);
            return Ok(reportDto);
        }

        [HttpPost("{id}/execute")]
        public async Task<IActionResult> Execute(int id, [FromBody] RunReportCommandRequest req)
        {
            req.ReportId = id;
            var result = await _mediator.Send(req);
            return Ok(result);
        }

        [HttpPost("{id}/aiExecute")]
        public async Task<IActionResult> ExecuteSql(int id, [FromBody] RunReportWithAiPromptRequest req)
        {
            req.ReportId = id;
            var result = await _mediator.Send(req);
            return Ok(result);
        }
    }
}