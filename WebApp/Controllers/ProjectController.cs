using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.ProjectProposal;
using Application.Services.ProposalService.ProposalDtos;
using Application.Exceptions;
using Application.Interfaces.ProjectApprovalStep;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectProposalService _proposalService;
        private readonly IProjectApprovalStepService _projectApprovalStepService;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(ILogger<ProjectController> logger, IProjectProposalService projectProposalService,
            IProjectApprovalStepService projectApprovalStepService)
        {
            _logger = logger;
            _proposalService = projectProposalService;
            _projectApprovalStepService = projectApprovalStepService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(List<ProjectProposalResponse>), 200)]
        [ProducesResponseType(typeof(ApiError), 400)]
        public async Task<IActionResult> GetProjects(string? title, int? status, int? applicant, int? approvalUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ExceptionBadRequest("Invalid parameters entered");
                }

                return Ok(await _projectApprovalStepService.GetAllProjectsFiltred(new ProposalFilterRequest()
                {
                    Title = title,
                    Applicant = applicant,
                    Status = status,
                    ApprovalUser = approvalUser
                }));
            }
            catch (ExceptionBadRequest ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ProducesResponseType(typeof(ProposalResponse), 201)]
        [ProducesResponseType(typeof(ApiError), 400)]
        public async Task<IActionResult> CreateProject(ProposalRequest proposal)
        {
            try
            {
                var result = await _proposalService.CreateProjectProposalAsync(proposal);

                //No olvidar crear la url consultando chatgpt

                return Created(string.Empty, result);
            }
            catch (ExceptionBadRequest ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}/decision")]
        [ProducesResponseType(typeof(ProposalResponse), 200)]
        [ProducesResponseType(typeof(ApiError), 400)]
        [ProducesResponseType(typeof(ApiError), 404)]
        [ProducesResponseType(typeof(ApiError), 409)]
        public async Task<IActionResult> Decide(Guid id, [FromBody] DecisionRequest request)
        {
            try
            {
                return Ok(await _projectApprovalStepService.DecideStatus(id ,request));
            }
            catch (ExceptionBadRequest ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ExceptionNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch (ExceptionConflict ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(ProposalResponse), 200)]
        [ProducesResponseType(typeof(ApiError), 400)]
        [ProducesResponseType(typeof(ApiError), 404)]
        [ProducesResponseType(typeof(ApiError), 409)]
        public async Task<IActionResult> GetProject(Guid id, [FromBody] ProposalUpdateRequest request)
        {
            try
            {
                return Ok(await _proposalService.UpdateProposalAsync(id,request));
            }
            catch (ExceptionBadRequest ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ExceptionNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch (ExceptionConflict ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProposalResponse), 200)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public async Task<IActionResult> GetProject(Guid id)
        {
            try
            {
                return Ok(await _proposalService.GetProjectById(id));
            }
            catch (ExceptionNotFound ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
