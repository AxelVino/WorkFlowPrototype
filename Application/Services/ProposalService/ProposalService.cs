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
using Application.Services.ProposalService.ProposalDtos;
using Application.Interfaces.User;
using Application.Services.UserService.UserDtos;
using Application.Exceptions;

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
        private readonly IUserService _userService;

        public ProposalService(IMediator mediator, IAreaService areaService,
            IProjectTypeService projectTypeService, IApprovalStatusService approvalStatusService,
            IProjectApprovalStepService projectApprovalStepService, IApprovalRuleService approvalRuleService,
            IUserService userService)
        {
            _mediator = mediator;
            _areaService = areaService;
            _projectTypeService = projectTypeService;
            _approvalStatusService = approvalStatusService;
            _projectApprovalStepService = projectApprovalStepService;
            _approvalRuleService = approvalRuleService;
            _userService = userService;
        }

        public async Task<ProposalResponse> CreateProjectProposalAsync(ProposalRequest proposal)
        {
            CreateProjectProposalCommand command = new()
            {
                Title = proposal.Title,
                Description = proposal.Description,
                Area = proposal.Area,
                AreaObject = await _areaService.GetAreaByIdAsync(proposal.Area),
                Type = proposal.Type,
                ProjectTypeObject = await _projectTypeService.GetTypeByIdAsync(proposal.Type),
                EstimatedAmount = proposal.Amount,
                EstimatedDuration = proposal.Duration,
                Status = 1,
                ApprovalStatusObject = await _approvalStatusService.GetStatusByIdAsync(1),
                CreateAt = DateTime.Now,
                CreateBy = proposal.User,
                UserObject = await _userService.GetUserByIdAsync(proposal.User),
            };

            ProjectProposal proposalProject = await _mediator.Send(command);

            List<ResponseApprovalRuleDto> response = await _approvalRuleService.MatchProposalWithRuleAsync(proposalProject.EstimatedAmount
                , proposalProject.Area, proposalProject.Type);

            List<ProjectStepResponse> list = await _projectApprovalStepService.CreateProjectApprovalStepAsync(response, proposalProject);

            ProposalResponse proposalResponse = new()
            {
                Id = proposalProject.Id,
                Title = proposalProject.Title,
                Description = proposalProject.Description,
                Amount = proposalProject.EstimatedAmount,
                Duration = proposalProject.EstimatedDuration,
                Area = proposalProject.AreaObject,
                Type = proposalProject.ProjectTypeObject,
                Status = proposalProject.ApprovalStatusObject,
                User = new UserResponse
                {
                    Id = proposalProject.CreateBy,
                    Email = proposalProject.UserObject.Email,
                    Name = proposalProject.UserObject.Name,
                    Role = proposalProject.UserObject.ApproverRoleObject!
                },
                Steps = list,
            };

            return proposalResponse;
        }

        public async Task<List<ProjectProposal>> GetAllProposalByUser(int idUser)
        {
            return await _mediator.Send(new GetAllProposalByUserQuery(idUser));
        }

        public async Task<ProposalResponse> GetProjectById(Guid id)
        {
            ProjectProposal proposalResponse = await _mediator.Send(new GetProposalProjectByIdQuery(id));

            if (proposalResponse == null)
                throw new ExceptionNotFound("Proposal not found, please enter a existing proposal.");

            List<ProjectStepResponse> stepResponse = await _projectApprovalStepService.GetProjectStepsById(id);

            return new ProposalResponse()
            {
                Id = proposalResponse.Id,
                Title = proposalResponse.Title,
                Description = proposalResponse.Description,
                Amount = proposalResponse.EstimatedAmount,
                Duration = proposalResponse.EstimatedDuration,
                Area = proposalResponse.AreaObject,
                Status = proposalResponse.ApprovalStatusObject,
                Type = proposalResponse.ProjectTypeObject,
                User = new UserResponse()
                {
                    Id = proposalResponse.UserObject.Id,
                    Name = proposalResponse.UserObject.Name,
                    Email = proposalResponse.UserObject.Email,
                    Role = proposalResponse.UserObject.ApproverRoleObject
                },
                Steps = stepResponse
            };
        }

        public async Task<ProposalResponse> UpdateProposalAsync(Guid id, ProposalUpdateRequest request)
        {
            if (request.Title == null && request.Description == null && request.Duration == null)
                throw new ExceptionBadRequest("You must enter at least one field to modify.");

            ProjectProposal project = await _mediator.Send(new GetProposalProjectByIdQuery(id));

            if (project == null)
                throw new ExceptionNotFound("Proposal not found, please enter a existing proposal.");

            if (project.Status == 2 || project.Status == 3)
                throw new ExceptionConflict("Cannot modify a rejected or approved project.");

            if (request.Title != null && request.Title != "string")
                project.Title = request.Title;

            if (request.Description != null && request.Description != "string" )
                project.Description = request.Description;

            if (request.Duration != null && request.Duration != 0)
                project.EstimatedDuration = request.Duration.Value;

            ProjectProposal response = await _mediator.Send(new UpdateProjectProposalCommand(project));

            return new ProposalResponse()
            {
                Id = response.Id,
                Title = response.Title,
                Description = response.Description,
                Amount = response.EstimatedAmount,
                Duration = response.EstimatedDuration,
                Area = response.AreaObject,
                Status = response.ApprovalStatusObject,
                Type = response.ProjectTypeObject,
                User = new UserResponse()
                {
                    Id = response.UserObject.Id,
                    Name = response.UserObject.Name,
                    Email = response.UserObject.Email,
                    Role = response.UserObject.ApproverRoleObject,
                },
                Steps = await _projectApprovalStepService.GetProjectStepsById(project.Id),

            };
        }
    }
}
