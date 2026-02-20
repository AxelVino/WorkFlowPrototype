using Application.Interfaces.ApprovalStatus;
using Application.Interfaces.ApproverRole;
using Application.Interfaces.ProjectApprovalStep;
using Application.Interfaces.User;
using Application.Responses;
using Application.Services.ApprovalRuleService.ApprovalRuleDto;
using Application.Services.ProjectApprovalStepService.ProjectApprovalStepCommands;
using Application.Services.ProjectApprovalStepService.ProjectApprovalStepQuerys;
using Application.Services.ProposalService.ProposalDtos;
using Domain.Entities;
using MediatR;

namespace Application.Services.ProjectApprovalStepService
{
    public class ProjectApprovalStepService : IProjectApprovalStepService
    {
        private readonly IMediator _mediator;
        private readonly IApproverRoleService _approverRoleService;
        private readonly IApprovalStatusService _approverStatusService;
        private readonly IUserService _userService;

        public ProjectApprovalStepService(IMediator mediator, IApproverRoleService approverRoleService, IApprovalStatusService approvalStatusService,
            IUserService userService)
        {
            _mediator = mediator;
            _approverRoleService = approverRoleService;
            _approverStatusService = approvalStatusService;
            _userService = userService;
        }

        public async Task<List<ApprovalStep>> CreateProjectApprovalStepAsync(List<ResponseApprovalRuleDto> rules, ProjectProposal proposal)
        {
            List<ProjectApprovalStep> newList = [];

            List<ApprovalStep> responseList = [];

            foreach (ResponseApprovalRuleDto rule in rules)
            {

                ProjectApprovalStep project = new()
                {
                    ProjectProposalId = proposal.Id,
                    ProjectProposalObject = proposal,
                    ApproverRoleId = rule.ApproverRoleId,
                    ApproverRoleObject = await _approverRoleService.GetApproverRoleByIdAsync(rule.ApproverRoleId),
                    Status = proposal.Status,
                    ApprovalStatusObject = proposal.ApprovalStatusObject,
                    StepOrder = rule.StepOrder,
                };
                newList.Add(project);
            }

            CreateProjectApprovalSteps command = new()
            {
                Steps = newList
            };

            _ = await _mediator.Send(command);

            foreach (ProjectApprovalStep step in newList)
            {
                ApprovalStep response = new()
                {
                    Id = step.Id,
                    StepOrder = step.StepOrder,
                    DecisionDate = DateTime.MinValue,
                    Observations = "",
                    ApproverUser = new Users 
                    { 
                        Id = 0,
                        Name = "", 
                        Email = "", 
                        Role = new GenericResponse() 
                        { 
                            Id = 0,
                            Name = "",
                        }
                    
                    },
                    ApproverRole = new GenericResponse() 
                    { 
                        Id = step.ApproverRoleObject.Id, 
                        Name = step.ApproverRoleObject.Name 
                    },
                    Status = new GenericResponse() 
                    { 
                        Id = step.ApprovalStatusObject.Id, 
                        Name = step.ApprovalStatusObject.Name
                    },
                };

                responseList.Add(response);
            }

            return responseList;
        }
        public async Task<List<ProjectProposalResponse>> GetAllProjectsFiltred(ProposalFilterRequest request)
        {

            List<ProjectApprovalStep> list = await _mediator.Send(new GetListApprovalStepsQuery(request));
            List<ProjectProposalResponse> listResponse = [];

            if (list == null)
            {
                return listResponse;
            }
            foreach (ProjectApprovalStep project in list)
            {
                ProjectProposalResponse response = new()
                {
                    Id = project.ProjectProposalId,
                    Title = project.ProjectProposalObject.Title,
                    Description = project.ProjectProposalObject.Description,
                    Amount = project.ProjectProposalObject.EstimatedAmount,
                    Duration = project.ProjectProposalObject.EstimatedDuration,
                    Area = project.ProjectProposalObject.AreaObject.Name,
                    Type = project.ProjectProposalObject.ProjectTypeObject.Name,
                    Status = project.ProjectProposalObject.ApprovalStatusObject.Name,
                };
                listResponse.Add(response);
            }
            return listResponse;
        }

        public async Task<List<ApprovalStep>> GetProjectStepsById(Guid id)
        {
            List<ProjectApprovalStep> list = await _mediator.Send(new GetProjectStepsByIdQuery(id));
            List<ApprovalStep> responseList = [];

            foreach (ProjectApprovalStep step in list)
            {

                Users approver = null;

                if (step.UserObject != null)
                {
                    approver = new Users
                    {
                        Id = step.UserObject.Id,
                        Name = step.UserObject.Name,
                        Email = step.UserObject.Email,
                        Role = new GenericResponse() 
                        { 
                            Id = step.UserObject.ApproverRoleObject!.Id,
                            Name = step.UserObject.ApproverRoleObject.Name,
                        },
                    };
                }

                ApprovalStep response = new()
                {
                    Id = step.Id,
                    StepOrder = step.StepOrder,
                    DecisionDate = step.DecisionDate,
                    Observations = step.Observations,
                    ApproverRole = new GenericResponse() 
                    { 
                        Id = step.ApproverRoleObject.Id, 
                        Name = step.ApproverRoleObject.Name
                    },
                    Status = new GenericResponse() 
                    { 
                        Id = step.ApprovalStatusObject.Id,
                        Name = step.ApprovalStatusObject.Name,
                    },
                    ApproverUser = approver!
                };
                responseList.Add(response);
            }
            return responseList;
        }

        public async Task<List<ProjectApprovalStep>> GetStepById(Guid id)
        {
            return await _mediator.Send(new GetProjectStepsByIdQuery(id));
        }
    }
}
