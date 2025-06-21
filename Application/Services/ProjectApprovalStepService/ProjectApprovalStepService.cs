using Application.Exceptions;
using Application.Interfaces.ApprovalStatus;
using Application.Interfaces.ApproverRole;
using Application.Interfaces.ProjectApprovalStep;
using Application.Interfaces.User;
using Application.Services.ApprovalRuleService.ApprovalRuleDto;
using Application.Services.ProjectApprovalStepService.ProjectApproalStepDtos;
using Application.Services.ProjectApprovalStepService.ProjectApprovalStepCommands;
using Application.Services.ProjectApprovalStepService.ProjectApprovalStepQuerys;
using Application.Services.ProposalService.ProposalDtos;
using Application.Services.UserService.UserDtos;
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

        public async Task<List<ProjectStepResponse>> CreateProjectApprovalStepAsync(List<ResponseApprovalRuleDto> rules, ProjectProposal proposal)
        {
            List<ProjectApprovalStep> newList = [];

            List<ProjectStepResponse> responseList = [];

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
                ProjectStepResponse response = new()
                {
                    Id = step.Id,
                    StepOrder = step.StepOrder,
                    DecisionDate = step.DecisionDate,
                    Observations = step.Observations,
                    Status = step.ApprovalStatusObject,
                    ApproverRole = step.ApproverRoleObject,
                };
                responseList.Add(response);
            }
            return responseList;
        }

        public async Task<ProposalResponse> DecideStatus(Guid id, DecisionRequest request)
        {
            if (request.Id == 0 || request.Status == 0 || request.User == 0 || string.IsNullOrWhiteSpace(request.Observation))
                throw new ExceptionBadRequest("Please, complete all the fields.");

            List<ProjectApprovalStep> project = await _mediator.Send(new GetProjectStepsByIdQuery(id));

            List<ProjectApprovalStep> projectApprovalSteps = [];

            List<ProjectStepResponse> response = [];

            if (project.Count == 0)
                throw new ExceptionNotFound("Project not found, please enter an existing project.");

            foreach (ProjectApprovalStep step in project)
            {
                if (!(step.Id == request.Id))
                {
                    continue;
                }

                if(step.Status == 2 || step.Status == 3)
                    throw new ExceptionConflict("Cannot modify a rejected or approved project.");

                User user = await _userService.GetUserByIdAsync(request.User);

                ApprovalStatus status = await _approverStatusService.GetStatusByIdAsync(request.Status);

                user.ApproverRoleObject = await _approverRoleService.GetApproverRoleByIdAsync(user.Role);

                step.Status = request.Status;
                step.ApprovalStatusObject = status;
                step.ApproverUserId = request.User;
                step.UserObject = user;
                step.Observations = request.Observation;

                projectApprovalSteps = await _mediator.Send(new UpdateProjectApprovalStep(step));

                foreach (ProjectApprovalStep s in projectApprovalSteps)
                {
                    ProjectStepResponse stepResponse = new()
                    {
                        Id = s.Id,
                        StepOrder = s.StepOrder,
                        ApproverUser = new ApproverUserResponse()
                        {
                            Id = s.UserObject.Id,
                            Name = s.UserObject.Name,
                            Email = s.UserObject.Email,
                            Role = s.UserObject.ApproverRoleObject
                        },
                        ApproverRole = s.ApproverRoleObject,
                        DecisionDate = DateTime.Now,
                        Observations = request.Observation,
                        Status = s.ApprovalStatusObject,
                    };
                    response.Add(stepResponse);
                }

                if (request.Status == 3)
                {
                    //funcion para desaprobar el proyecto
                }
            }

            if (response.Count == 0)
                throw new ExceptionNotFound("Step not found, please enter a valid step.");

            if (projectApprovalSteps == null || projectApprovalSteps.Count == 0)
                throw new ExceptionNotFound("No project approval steps found.");

            var projectProposal = projectApprovalSteps[0].ProjectProposalObject;

            return new ProposalResponse()
            {
                Id = projectApprovalSteps[0].ProjectProposalId,
                Title = projectProposal.Title,
                Description = projectProposal.Description,
                Amount = projectProposal.EstimatedAmount,
                Duration = projectProposal.EstimatedDuration,
                Area = projectProposal.AreaObject,
                Status = projectProposal.ApprovalStatusObject,
                Type = projectProposal.ProjectTypeObject,
                User = new UserResponse()
                {
                    Id = projectProposal.UserObject.Id,
                    Name = projectProposal.UserObject.Name,
                    Email = projectProposal.UserObject.Email,
                    Role = await _approverRoleService.GetApproverRoleByIdAsync(projectProposal.UserObject.Role)
                },
                Steps = response,
            };
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

        public async Task<List<ProjectStepResponse>> GetProjectStepsById(Guid id)
        {
            List<ProjectApprovalStep> list = await _mediator.Send(new GetProjectStepsByIdQuery(id));
            List<ProjectStepResponse> responseList = [];

            foreach (ProjectApprovalStep step in list)
            {

                ApproverUserResponse? approver = null;

                if (step.UserObject != null)
                {
                    approver = new ApproverUserResponse
                    {
                        Id = step.UserObject.Id,
                        Name = step.UserObject.Name,
                        Email = step.UserObject.Email,
                        Role = step.UserObject.ApproverRoleObject
                    };
                }

                ProjectStepResponse response = new()
                {
                    Id = step.Id,
                    StepOrder = step.StepOrder,
                    DecisionDate = step.DecisionDate,
                    Observations = step.Observations,
                    ApproverRole = step.ApproverRoleObject,
                    Status = step.ApprovalStatusObject,
                    ApproverUser = approver
                };
                responseList.Add(response);
            }
            return responseList;
        }
    }
}
