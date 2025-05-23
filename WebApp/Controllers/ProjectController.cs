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
                await _proposalService.CreateProjectProposalAsync(proposal);

                return Created();
            }
            catch (ExceptionBadRequest ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
