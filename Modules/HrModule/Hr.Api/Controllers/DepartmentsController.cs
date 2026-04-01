using Hr.Application.Features.DepartmentFeatures.Commands.DeleteAttachment;
using Hr.Application.Features.DepartmentFeatures.Commands.UploadAttachment;
using Hr.Application.Features.DepartmentFeatures.CreateDepartment;
using Hr.Application.Features.DepartmentFeatures.DeleteDepartment;
using Hr.Application.Features.DepartmentFeatures.GetAllDepartments;
using Hr.Application.Features.DepartmentFeatures.GetDepartmentById;
using Hr.Application.Features.DepartmentFeatures.GetDepartmentsPaged;
using Hr.Application.Features.DepartmentFeatures.Queries.GetDepartmentTree;
using Hr.Application.DTOs;
using Hr.Application.Features.DepartmentFeatures.UpdateDepartment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SharedKernel.Constants.Permissions;
using SharedKernel.Authorization;

namespace Hr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Hr")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DepartmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [HasPermission( HrPermissions.DepartmentsCreate)]
        public async Task<IActionResult> Create([FromForm] CreateDepartmentRequest request)
        {
            var result = await _mediator.Send(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        [HasPermission( HrPermissions.DepartmentsView)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllDepartmentsRequest());
            return Ok(result);
        }

        [HttpGet("paged")]
        [HasPermission( HrPermissions.DepartmentsView)]
        public async Task<IActionResult> GetPaged([FromQuery] GetDepartmentsPagedRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("tree")]
        [HasPermission( HrPermissions.DepartmentsViewTree)]
        public async Task<IActionResult> GetTree()
        {
            var result = await _mediator.Send(new GetDepartmentTreeRequest());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [HasPermission( HrPermissions.DepartmentsView)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetDepartmentByIdRequest { Id = id });
            if (result.Department == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        [HasPermission( HrPermissions.DepartmentsEdit)]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateDepartmentRequest request)
        {
            request.Id = id;

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [HasPermission( HrPermissions.DepartmentsDelete)]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new DeleteDepartmentRequest { Id = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("{id}/attachments")]
        [HasPermission( HrPermissions.DepartmentsManageAttachments)]
        public async Task<IActionResult> UploadAttachment(int id, [FromForm] UploadDepartmentAttachmentRequest request)
        {
            request.DepartmentId = id;
            var result = await _mediator.Send(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("attachments/{attachmentId}")]
        [HasPermission( HrPermissions.DepartmentsManageAttachments)]
        public async Task<IActionResult> DeleteAttachment(int attachmentId)
        {
            var request = new DeleteDepartmentAttachmentRequest { AttachmentId = attachmentId };
            var result = await _mediator.Send(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("DepartmentMetadata")]
        [HasPermission( HrPermissions.DepartmentsView)]
        public IActionResult GetMetadata()
        {
            var metadata = new EntityMetadataDto
            {
                EntityName = "Department",
                OrderableFields = new List<OrderableFieldDto>
                {
                    new OrderableFieldDto { Key = "Name", Label = "Name" }
                },
                FilterableFields = new List<FilterableFieldDto>
                {
                    new FilterableFieldDto { Key = "searchTerm", Type = "string" }
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