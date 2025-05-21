using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.ProjectProposal;
using Application.Services.ProposalService.ProposalDtos;
using Application.Exceptions;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectProposalService _proposalService;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(ILogger<ProjectController> logger, IProjectProposalService projectProposalService)
        {
            _logger = logger;
            _proposalService = projectProposalService;
        }
        [HttpPost("Create")]
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
