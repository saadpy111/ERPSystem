using Hr.Application.Features.JobFeatures.ActivateJob;
using Hr.Application.Features.JobFeatures.CreateJob;
using Hr.Application.Features.JobFeatures.DeactivateJob;
using Hr.Application.Features.JobFeatures.DeleteJob;
using Hr.Application.Features.JobFeatures.GetActiveJobs;
using Hr.Application.Features.JobFeatures.GetAllJobs;
using Hr.Application.Features.JobFeatures.GetJobApplicants;
using Hr.Application.Features.JobFeatures.GetJobById;
using Hr.Application.Features.JobFeatures.GetJobsByDepartment;
using Hr.Application.Features.JobFeatures.GetJobsPaged;
using Hr.Application.DTOs;
using Hr.Application.Features.JobFeatures.UpdateJob;
using Hr.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Hr")]
    public class JobsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJobRequest request)
        {
            var result = await _mediator.Send(request);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllJobsRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] GetJobsPagedRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetJobByIdRequest { Id = id };
            var result = await _mediator.Send(query);

            if (result.Job == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateJobRequest request)
        {
            request.Id = id;

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new DeleteJobRequest { Id = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Business Endpoints

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveJobs()
        {
            var query = new GetActiveJobsRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("department/{departmentId}")]
        public async Task<IActionResult> GetJobsByDepartment(int departmentId)
        {
            var query = new GetJobsByDepartmentRequest { DepartmentId = departmentId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{jobId}/applicants")]
        public async Task<IActionResult> GetJobApplicants(int jobId)
        {
            var query = new GetJobApplicantsRequest { JobId = jobId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPatch("{jobId}/activate")]
        public async Task<IActionResult> ActivateJob(int jobId)
        {
            var request = new ActivateJobRequest { JobId = jobId };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("{jobId}/deactivate")]
        public async Task<IActionResult> DeactivateJob(int jobId)
        {
            var request = new DeactivateJobRequest { JobId = jobId };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Enum Endpoints

        [HttpGet("enums/statuses")]
        public IActionResult GetJobStatuses()
        {
            var statuses = Enum.GetValues(typeof(JobStatus))
                .Cast<JobStatus>()
                .Select(s => new { Value = (int)s, Name = s.ToString() });
            return Ok(statuses);
        }

        [HttpGet("enums/work-types")]
        public IActionResult GetWorkTypes()
        {
            var workTypes = Enum.GetValues(typeof(WorkType))
                .Cast<WorkType>()
                .Select(w => new { Value = (int)w, Name = w.ToString() });
            return Ok(workTypes);
        }

        [HttpGet("JobMetadata")]
        public IActionResult GetMetadata()
        {
            var metadata = new EntityMetadataDto
            {
                EntityName = "Job",
                OrderableFields = new List<OrderableFieldDto>
                {
                    new OrderableFieldDto { Key = "Title", Label = "Title" },
                    new OrderableFieldDto { Key = "PublishedDate", Label = "Published Date" },
                    new OrderableFieldDto { Key = "Status", Label = "Status" }
                },
                FilterableFields = new List<FilterableFieldDto>
                {
                    new FilterableFieldDto { Key = "searchTerm", Type = "string" },
                    new FilterableFieldDto { Key = "departmentId", Type = "number" },
                    new FilterableFieldDto { Key = "status", Type = "enum", Values = Enum.GetNames(typeof(JobStatus)).ToList() },
                    new FilterableFieldDto { Key = "workType", Type = "enum", Values = Enum.GetNames(typeof(WorkType)).ToList() }
                },
                Pagination = new PaginationMetadataDto
                {
                    DefaultPageSize = 10,
                    MaxPageSize = 100
                }
            };

            return Ok(metadata);
        }
    }
}
