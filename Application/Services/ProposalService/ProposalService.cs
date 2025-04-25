using Application.Interfaces.Area;
using Application.Interfaces.ApprovalStatus;
using Application.Interfaces.ProjectProposal;
using Application.Interfaces.ProjectType;
using Application.Services.ProposalService.ProposalCommands;
using MediatR;
using Application.Interfaces.ProjectApprovalStep;
using Domain.Entities;
using Application.Interfaces.ApprovalRule;
using Application.Services.ApprovalRuleService.ApprovalRuleDto;
using Application.Services.ProjectApprovalStepService.ProjectApproalStepDtos;
using Application.Services.ProposalService.ProposalQuerys;

namespace Application.Services.ProposalService
{
    public class ProposalService: IProjectProposalService
    {
        private readonly IMediator _mediator;
        private readonly IAreaService _areaService;
        private readonly IProjectTypeService _projectTypeService;
        private readonly IApprovalStatusService _approvalStatusService;
        private readonly IProjectApprovalStepService _projectApprovalStepService;
        private readonly IApprovalRuleService _approvalRuleService;

        public ProposalService(IMediator mediator, IAreaService areaService,
            IProjectTypeService projectTypeService, IApprovalStatusService approvalStatusService,
            IProjectApprovalStepService projectApprovalStepService, IApprovalRuleService approvalRuleService)
        {
            _mediator = mediator;
            _areaService = areaService;
            _projectTypeService = projectTypeService;
            _approvalStatusService = approvalStatusService;
            _projectApprovalStepService = projectApprovalStepService;
            _approvalRuleService = approvalRuleService;
        }

        public async Task<Guid> CreateProjectProposalAsync(CreateProjectProposalCommand command)
        {
            //Search the missing objects to finish the project proposal entity
            command.AreaObject = await _areaService.GetAreaByIdAsync(command.Area);
            command.ProjectTypeObject = await _projectTypeService.GetTypeByIdAsync(command.Type);
            command.ApprovalStatusObject = await _approvalStatusService.GetStatusByIdAsync(command.Status);

            //Save the new project proposal
            ProjectProposal proposalProject = await _mediator.Send(command);

            //Call _approvalRuleService to obtain StepOrder and ApproverRoleId
            ResponseApprovalRuleDto response = await _approvalRuleService.MatchProposalWithRuleAsync(proposalProject.EstimatedAmount
                , proposalProject.Area, proposalProject.Type);

            //Build a incompleted project approval steps meanwhile we are creating the project proposal
            IncompletedProjectDto incompletedDto = new()
            {
                ProjectProposalId = proposalProject.Id,
                ProjectProposalObject = proposalProject,
                ApproverRoleId = response.ApproverRoleId,
                Status = command.Status,
                ApprovalStatusObject = command.ApprovalStatusObject,
                StepOrder = response.StepOrder      
            };

            //Save the new project approval step
            _ = await _projectApprovalStepService.CreateProjectApprovalStepAsync(incompletedDto);

            return proposalProject.Id;
        }

        public async Task<List<ProjectProposal>> GetAllProposalByUser(int idUser)
        {
            return await _mediator.Send(new GetAllProposalByUserQuery(idUser));
        }

        public async Task<bool> UpdateProposalAsync(ProjectProposal project)
        {
            return await _mediator.Send(new UpdateProjectProposalCommand(project));
        }
    }
}
