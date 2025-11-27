using Hr.Application.Features.EmployeeFeatures.ActivateEmployee;
using Hr.Application.Features.EmployeeFeatures.CreateEmployee;
using Hr.Application.Features.EmployeeFeatures.DeleteEmployee;
using Hr.Application.Features.EmployeeFeatures.GetActiveEmployees;
using Hr.Application.Features.EmployeeFeatures.GetAllEmployees;
using Hr.Application.Features.EmployeeFeatures.GetEmployeeAttendanceRecords;
using Hr.Application.Features.EmployeeFeatures.GetEmployeeById;
using Hr.Application.Features.EmployeeFeatures.GetEmployeeLeaveRequests;
using Hr.Application.Features.EmployeeFeatures.GetEmployeeSalaryDetails;
using Hr.Application.Features.EmployeeFeatures.GetEmployeesPaged;
using Hr.Application.Features.EmployeeFeatures.PromoteEmployee;
using Hr.Application.Features.EmployeeFeatures.TerminateEmployee;
using Hr.Application.DTOs;
using Hr.Application.Features.EmployeeFeatures.UpdateEmployee;
using Hr.Application.Features.EmployeeFeatures.Commands.DeleteAttachment;
using Hr.Application.Features.EmployeeFeatures.Commands.UploadAttachment;
using Hr.Application.Features.EmployeeFeatures.Queries.GetAttachmentsByEmployeeId;
using Hr.Application.Features.EmployeeFeatures.Queries.GetAttachmentById;
using Hr.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Hr")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateEmployeeRequest request)
        {
            var result = await _mediator.Send(request);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllEmployeesRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] GetEmployeesPagedRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetEmployeeByIdRequest { Id = id };
            var result = await _mediator.Send(query);

            if (result.Employee == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateEmployeeRequest request)
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
            var request = new DeleteEmployeeRequest { Id = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Business Endpoints

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveEmployees()
        {
            var query = new GetActiveEmployeesRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPatch("{employeeId}/activate")]
        public async Task<IActionResult> ActivateEmployee(int employeeId)
        {
            var request = new ActivateEmployeeRequest { EmployeeId = employeeId };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("{employeeId}/terminate")]
        public async Task<IActionResult> TerminateEmployee(int employeeId)
        {
            var request = new TerminateEmployeeRequest { EmployeeId = employeeId };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("{employeeId}/promote")]
        public async Task<IActionResult> PromoteEmployee(int employeeId, [FromBody] PromoteEmployeeRequest request)
        {
            if (employeeId != request.EmployeeId)
                return BadRequest("Employee ID mismatch");

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{employeeId}/salary-details")]
        public async Task<IActionResult> GetEmployeeSalaryDetails(int employeeId)
        {
            var query = new GetEmployeeSalaryDetailsRequest { EmployeeId = employeeId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{employeeId}/attendance-records")]
        public async Task<IActionResult> GetEmployeeAttendanceRecords(int employeeId)
        {
            var query = new GetEmployeeAttendanceRecordsRequest { EmployeeId = employeeId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{employeeId}/leave-requests")]
        public async Task<IActionResult> GetEmployeeLeaveRequests(int employeeId)
        {
            var query = new GetEmployeeLeaveRequestsRequest { EmployeeId = employeeId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // Attachment Endpoints

        [HttpPost("{employeeId}/attachments")]
        public async Task<IActionResult> UploadAttachment(int employeeId, [FromForm] UploadAttachmentRequest request)
        {
            if (employeeId != request.EmployeeId)
                return BadRequest("Employee ID mismatch");
                
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{employeeId}/attachments")]
        public async Task<IActionResult> GetAttachmentsByEmployeeId(int employeeId)
        {
            var query = new GetAttachmentsByEmployeeIdRequest { EmployeeId = employeeId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("attachments/{attachmentId}")]
        public async Task<IActionResult> GetAttachmentById(int attachmentId)
        {
            var query = new GetAttachmentByIdRequest { AttachmentId = attachmentId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpDelete("attachments/{attachmentId}")]
        public async Task<IActionResult> DeleteAttachment(int attachmentId)
        {
            var request = new DeleteAttachmentRequest { AttachmentId = attachmentId };
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        // Enum Endpoints

        [HttpGet("enums/statuses")]
        public IActionResult GetEmployeeStatuses()
        {
            var statuses = Enum.GetValues(typeof(EmployeeStatus))
                .Cast<EmployeeStatus>()
                .Select(s => new { Value = (int)s, Name = s.ToString() });
            return Ok(statuses);
        }

        [HttpGet("EmployeeMetadata")]
        public IActionResult GetMetadata()
        {
            var metadata = new EntityMetadataDto
            {
                EntityName = "Employee",
                OrderableFields = new List<OrderableFieldDto>
                {
                    new OrderableFieldDto { Key = "FullName", Label = "Full Name" },
                    new OrderableFieldDto { Key = "Email", Label = "Email" },
                    new OrderableFieldDto { Key = "Status", Label = "Status" }
                },
                FilterableFields = new List<FilterableFieldDto>
                {
                    new FilterableFieldDto { Key = "searchTerm", Type = "string" },
                    new FilterableFieldDto { Key = "status", Type = "enum", Values = Enum.GetNames(typeof(EmployeeStatus)).ToList() }
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
