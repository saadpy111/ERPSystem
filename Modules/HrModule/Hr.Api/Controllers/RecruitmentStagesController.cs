using Hr.Application.Features.RecruitmentStageFeatures.ActivateRecruitmentStage;
using Hr.Application.Features.RecruitmentStageFeatures.CreateRecruitmentStage;
using Hr.Application.Features.RecruitmentStageFeatures.DeactivateRecruitmentStage;
using Hr.Application.Features.RecruitmentStageFeatures.DeleteRecruitmentStage;
using Hr.Application.Features.RecruitmentStageFeatures.GetActiveRecruitmentStages;
using Hr.Application.Features.RecruitmentStageFeatures.GetAllRecruitmentStages;
using Hr.Application.Features.RecruitmentStageFeatures.GetOrderedRecruitmentStages;
using Hr.Application.Features.RecruitmentStageFeatures.GetRecruitmentStageById;
using Hr.Application.Features.RecruitmentStageFeatures.GetRecruitmentStagesPaged;
using Hr.Application.Features.RecruitmentStageFeatures.ReorderRecruitmentStages;
using Hr.Application.Features.RecruitmentStageFeatures.UpdateRecruitmentStage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Hr")]
    public class RecruitmentStagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RecruitmentStagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRecruitmentStageRequest request)
        {
            var result = await _mediator.Send(request);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllRecruitmentStagesRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] GetRecruitmentStagesPagedRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var query = new GetActiveRecruitmentStagesRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("ordered")]
        public async Task<IActionResult> GetOrdered()
        {
            var query = new GetOrderedRecruitmentStagesRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetRecruitmentStageByIdRequest { Id = id };
            var result = await _mediator.Send(query);

            if (result.RecruitmentStage == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRecruitmentStageRequest request)
        {
            if (id != request.StageId)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            var request = new ActivateRecruitmentStageRequest { StageId = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var request = new DeactivateRecruitmentStageRequest { StageId = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("reorder")]
        public async Task<IActionResult> Reorder([FromBody] ReorderRecruitmentStagesRequest request)
        {
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new DeleteRecruitmentStageRequest { StageId = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
