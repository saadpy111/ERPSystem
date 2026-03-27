using Hr.Application.Features.ApplicantFeatures.AcceptApplicant;
using Hr.Application.Features.ApplicantFeatures.Commands.DeleteAttachmentApplicant;
using Hr.Application.Features.ApplicantFeatures.Commands.UploadAttachmentApplicant;
using Hr.Application.Features.ApplicantFeatures.CreateApplicant;
using Hr.Application.Features.ApplicantFeatures.DeleteApplicant;
using Hr.Application.Features.ApplicantFeatures.GetAllApplicants;
using Hr.Application.Features.ApplicantFeatures.GetApplicantById;
using Hr.Application.Features.ApplicantFeatures.GetApplicantsByJob;
using Hr.Application.Features.ApplicantFeatures.GetApplicantsByStage;
using Hr.Application.Features.ApplicantFeatures.GetApplicantsPaged;
using Hr.Application.Features.ApplicantFeatures.MoveApplicantToStage;
using Hr.Application.Features.ApplicantFeatures.RejectApplicant;
using Hr.Application.Features.ApplicantFeatures.ScheduleInterview;
using Hr.Application.DTOs;
using Hr.Application.Features.ApplicantFeatures.UpdateApplicant;
using Hr.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SharedKernel.Constants.Permissions;

namespace Hr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Hr")]
    public class ApplicantsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Permissions = HrPermissions.ApplicantsCreate)]
        public async Task<IActionResult> Create([FromForm] CreateApplicantRequest request)
        {
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Permissions = HrPermissions.ApplicantsView)]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllApplicantsRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("paged")]
        [Authorize(Permissions = HrPermissions.ApplicantsView)]
        public async Task<IActionResult> GetPaged([FromQuery] GetApplicantsPagedRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("job/{jobId}")]
        [Authorize(Permissions = HrPermissions.ApplicantsView)]
        public async Task<IActionResult> GetByJob(int jobId)
        {
            var query = new GetApplicantsByJobRequest { JobId = jobId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("stage/{stageId}")]
        [Authorize(Permissions = HrPermissions.ApplicantsView)]
        public async Task<IActionResult> GetByStage(int stageId)
        {
            var query = new GetApplicantsByStageRequest { StageId = stageId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Permissions = HrPermissions.ApplicantsView)]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetApplicantByIdRequest { Id = id };
            var result = await _mediator.Send(query);

            if (result.Applicant == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Permissions = HrPermissions.ApplicantsEdit)]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateApplicantRequest request)
        {
            request.ApplicantId = id;

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("{id}/move-to-stage")]
        [Authorize(Permissions = HrPermissions.ApplicantsMoveToStage)]
        public async Task<IActionResult> MoveToStage(int id, [FromBody] MoveApplicantToStageRequest request)
        {
            request.ApplicantId = id;

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("{id}/accept")]
        [Authorize(Permissions = HrPermissions.ApplicantsAccept)]
        public async Task<IActionResult> Accept(int id, [FromBody] AcceptApplicantRequest request)
        {
            request.ApplicantId = id;

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("{id}/reject")]
        [Authorize(Permissions = HrPermissions.ApplicantsReject)]
        public async Task<IActionResult> Reject(int id, [FromBody] RejectApplicantRequest request)
        {
            request.ApplicantId = id;

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("{id}/schedule-interview")]
        [Authorize(Permissions = HrPermissions.ApplicantsScheduleInterview)]
        public async Task<IActionResult> ScheduleInterview(int id, [FromBody] ScheduleInterviewRequest request)
        {
            request.ApplicantId = id;

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

            [HttpDelete("{id}")]
            [Authorize(Permissions = HrPermissions.ApplicantsDelete)]
            public async Task<IActionResult> Delete(int id)
            {
                var request = new DeleteApplicantRequest { ApplicantId = id };
                var result = await _mediator.Send(request);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }

            // Enum Endpoints

            [HttpGet("enums/statuses")]
            [Authorize(Permissions = HrPermissions.ApplicantsView)]
            public IActionResult GetApplicantStatuses()
            {
                var statuses = Enum.GetValues(typeof(ApplicantStatus))
                    .Cast<ApplicantStatus>()
                    .Select(s => new { Value = (int)s, Name = s.ToString() });
                return Ok(statuses);
            }

            [HttpPost("{id}/attachments")]
            [Authorize(Permissions = HrPermissions.ApplicantsManageAttachments)]
            public async Task<IActionResult> UploadAttachment(int id, [FromForm] UploadAttachmentApplicantRequest request)
            {
                request.ApplicantId = id;
                var result = await _mediator.Send(request);
                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }

            [HttpDelete("attachments/{attachmentId}")]
            [Authorize(Permissions = HrPermissions.ApplicantsManageAttachments)]
            public async Task<IActionResult> DeleteAttachment(int attachmentId)
            {
                var request = new DeleteAttachmentApplicantRequest { AttachmentId = attachmentId };
                var result = await _mediator.Send(request);
                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }

            [HttpGet("ApplicantMetadata")]
            [Authorize(Permissions = HrPermissions.ApplicantsView)]
            public IActionResult GetMetadata()
            {
                var metadata = new EntityMetadataDto
                {
                    EntityName = "Applicant",
                    OrderableFields = new List<OrderableFieldDto>
                    {
                        new OrderableFieldDto { Key = "FullName", Label = "Full Name" },
                        new OrderableFieldDto { Key = "ApplicationDate", Label = "Application Date" },
                        new OrderableFieldDto { Key = "Status", Label = "Status" }
                    },
                    FilterableFields = new List<FilterableFieldDto>
                    {
                        new FilterableFieldDto { Key = "searchTerm", Type = "string" },
                        new FilterableFieldDto { Key = "jobId", Type = "number" },
                        new FilterableFieldDto { Key = "currentStageId", Type = "number" },
                        new FilterableFieldDto { Key = "status", Type = "enum", Values = Enum.GetNames(typeof(ApplicantStatus)).ToList() }
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

    
}
