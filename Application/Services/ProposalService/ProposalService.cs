using Application.Interfaces.Area;
using Application.Interfaces.ApprovalStatus;
using Application.Interfaces.ProjectProposal;
using Application.Interfaces.ProjectType;
using Application.Services.ProposalService.ProposalCommands;
using MediatR;
using Application.Interfaces.ProjectApprovalStep;
using Domain.Entities;
using Application.Interfaces.ApprovalRule;
using Application.Services.ProposalService.ProposalQuerys;
using Application.Services.ProposalService.ProposalDtos;
using Application.Interfaces.User;
using Application.Exceptions;
using Application.Responses;
using Application.Request;
using Application.Interfaces.ApproverRole;
using Application.Services.ApprovalRuleService.ApprovalRuleDto;
using Application.Services.ProjectApprovalStepService.ProjectApprovalStepCommands;

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
        private readonly IApproverRoleService _approverRoleService;

        public ProposalService(IMediator mediator, IAreaService areaService,
            IProjectTypeService projectTypeService, IApprovalStatusService approvalStatusService,
            IProjectApprovalStepService projectApprovalStepService, IApprovalRuleService approvalRuleService,
            IUserService userService, IApproverRoleService approverRoleService)
        {
            _mediator = mediator;
            _areaService = areaService;
            _projectTypeService = projectTypeService;
            _approvalStatusService = approvalStatusService;
            _projectApprovalStepService = projectApprovalStepService;
            _approvalRuleService = approvalRuleService;
            _userService = userService;
            _approverRoleService = approverRoleService;
        }

        public async Task<Project> CreateProjectProposalAsync(ProjectCreate proposal)
        {

            if (string.IsNullOrWhiteSpace(proposal.Title) || !proposal.Title.Any(char.IsLetter))
                throw new ExceptionBadRequest("You must provide a valid title with at least one letter.");

            List<string> titles = await _mediator.Send(new GetAllTitleProjectInUse());

            foreach (string title in titles)
            {
                if (title == proposal.Title)
                {
                    throw new ExceptionBadRequest("The project title is already in use.");
                }
            }

            if (string.IsNullOrWhiteSpace(proposal.Description) || !proposal.Description.Any(char.IsLetter))
                throw new ExceptionBadRequest("You must provide a valid description with at least one letter.");
            if (proposal.Amount <= 0)
                throw new ExceptionBadRequest("Please enter an amount greater than 0.");
            if (proposal.Duration <= 0)
                throw new ExceptionBadRequest("Please enter a duration greater than 0.");
            if (proposal.Area <= 0)
                throw new ExceptionBadRequest("Please enter an area greater than 0.");
            if (proposal.Type <= 0)
                throw new ExceptionBadRequest("Please enter an type greater than 0.");
            if (proposal.User <= 0)
                throw new ExceptionBadRequest("Please enter an user greater than 0.");

            Area area = await _areaService.GetAreaByIdAsync(proposal.Area);
            User user = await _userService.GetUserByIdAsync(proposal.User);
            ProjectType type = await _projectTypeService.GetTypeByIdAsync(proposal.Type);

            CreateProjectProposalCommand command = new()
            {
                Title = proposal.Title,
                Description = proposal.Description,
                Area = area.Id,
                AreaObject = area,
                Type = type.Id,
                ProjectTypeObject = type,
                EstimatedAmount = proposal.Amount,
                EstimatedDuration = proposal.Duration,
                Status = 1,
                ApprovalStatusObject = await _approvalStatusService.GetStatusByIdAsync(1),
                CreateAt = DateTime.Now,
                CreateBy = user.Id,
                UserObject = user,
            };

            ProjectProposal proposalProject = await _mediator.Send(command);

            List<ResponseApprovalRuleDto> response = await _approvalRuleService.MatchProposalWithRuleAsync(proposalProject.EstimatedAmount
               , proposalProject.Area, proposalProject.Type);

            List<ApprovalStep> list = await _projectApprovalStepService.CreateProjectApprovalStepAsync(response, proposalProject);

            ApproverRole approver = await _approverRoleService.GetApproverRoleByIdAsync(proposalProject.UserObject.Role);

            return new Project()
            {
                Id = proposalProject.Id,
                Title = proposalProject.Title,
                Description = proposalProject.Description,
                Amount = (double)proposalProject.EstimatedAmount,
                Duration = proposalProject.EstimatedDuration,
                Area = new GenericResponse()
                {
                    Id = proposalProject.AreaObject.Id,
                    Name = proposalProject.AreaObject.Name
                },
                Status = new GenericResponse
                {
                    Id = proposalProject.ApprovalStatusObject.Id,
                    Name = proposalProject.ApprovalStatusObject.Name
                },
                Type = new GenericResponse()
                {
                    Id = proposalProject.ProjectTypeObject.Id,
                    Name = proposalProject.ProjectTypeObject.Name
                },
                User = new Users
                {
                    Id = proposalProject.CreateBy,
                    Email = proposalProject.UserObject.Email,
                    Name = proposalProject.UserObject.Name,
                    Role = new GenericResponse()
                    {
                        Id = approver.Id,
                        Name = approver.Name,
                    }
                },
                Steps = list,
            };
        }

        public async Task<Project> EvaluateProject(Guid id, DecisionRequest request)
        {
            ProjectProposal proposalResponse = await _mediator.Send(new GetProposalProjectByIdQuery(id));

            ProjectApprovalStep stepSelected = null;

            if (proposalResponse.Status == 2 || proposalResponse.Status == 3)
            {
                throw new ExceptionConflict("The project is in a state where it can no longer be modified.");
            }

            if (proposalResponse == null)
                throw new ExceptionNotFound("Proposal not found, please enter a existing proposal.");

            if (request.Id <= 0)
                throw new ExceptionBadRequest("Please enter an id greater than 0.");

            if (request.User <= 0)
                throw new ExceptionBadRequest("Please enter an user greater than 0.");
            User user = await _userService.GetUserByIdAsync(request.User);

            if (request.Status <= 1)
                throw new ExceptionBadRequest("Please enter an status greater than 0.");
            ApprovalStatus status = await _approvalStatusService.GetStatusByIdAsync(request.Status);

            if (string.IsNullOrWhiteSpace(request.Observation) || !request.Observation.Any(char.IsLetter))
                throw new ExceptionBadRequest("You must provide a valid observation with at least one letter.");

            List<ProjectApprovalStep> stepResponse = await _projectApprovalStepService.GetStepById(id);

            
            foreach (ProjectApprovalStep step in stepResponse)
            {
                if(step.Id == request.Id)
                {
                    if (step.ApprovalStatusObject.Id == 2 || step.ApprovalStatusObject.Id == 3)
                    {
                        throw new ExceptionConflict("The step is in a state where it can no longer be modified.");
                    }

                    stepSelected = step;
                    stepSelected.ApproverUserId = request.User;
                    stepSelected.UserObject = user;
                    stepSelected.Observations = request.Observation;
                    stepSelected.DecisionDate = DateTime.Now;
                    stepSelected.Status = status.Id;
                    stepSelected.ApprovalStatusObject = status;
                    stepSelected = step;
                    stepResponse = stepResponse
                        .Where(s => s.StepOrder != step.StepOrder)
                        .ToList();

                    stepResponse.Add(step);

                    break; 
                }
            }

            if (stepSelected == null)
            {
                throw new ExceptionNotFound("Step not found, please enter a existing step.");
            }

            if(user.Role != stepSelected.ApproverRoleId)
            {
                throw new ExceptionConflict("You do not have the indicated hierarchy to modify");
            }


            List<ApprovalStep> approvalSteps = new List<ApprovalStep>();

            foreach (ProjectApprovalStep step in stepResponse)
            {
                ApproverRole roleApprover = await _approverRoleService.GetApproverRoleByIdAsync(step.ApproverRoleId);
                ApprovalStep responseStep = new()
                {
                    Id = step.Id,
                    StepOrder = step.StepOrder,
                    ApproverUser = new Users()
                    {
                        Id = step.UserObject?.Id ?? 0,
                        Name = step.UserObject?.Name ?? "",
                        Email = step.UserObject?.Email ?? "",
                        Role = new GenericResponse()
                        {
                            Id = step.UserObject?.ApproverRoleObject?.Id ?? 0,
                            Name = step.UserObject?.ApproverRoleObject?.Name ?? ""
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
                    Observations = string.IsNullOrEmpty(step.Observations) ? "" : step.Observations,
                    DecisionDate = step.DecisionDate ?? DateTime.MinValue,
                };
                approvalSteps.Add(responseStep);
            }
            
            if (request.Status == 3)
            {
                proposalResponse.Status = 3;
                proposalResponse.ApprovalStatusObject = status;

            }

            if(request.Status == 2)
            {
                int count = 0;
                foreach (ProjectApprovalStep step in stepResponse)
                {
                    if (step.Status == 2)
                    {
                        count++;
                    }
                }
                if(count == stepResponse.Count)
                {
                    proposalResponse.Status = 2;
                    proposalResponse.ApprovalStatusObject = status;
                }
            }

            if (request.Status == 4)
            {
                proposalResponse.Status = 3;
                proposalResponse.ApprovalStatusObject = status;
            }

            _ = await _mediator.Send(new UpdateProjectProposalCommand(proposalResponse));
            _ = await _mediator.Send(new UpdateProjectApprovalStep(stepSelected));

            ApproverRole role = await _approverRoleService.GetApproverRoleByIdAsync(proposalResponse.UserObject.Role);
            Area area = await _areaService.GetAreaByIdAsync(proposalResponse.Area);
            ProjectType type = await _projectTypeService.GetTypeByIdAsync(proposalResponse.Type);

            return new Project()
            {
                Id = proposalResponse.Id,
                Title = proposalResponse.Title,
                Description = proposalResponse.Description,
                Amount = (double)proposalResponse.EstimatedAmount,
                Duration = proposalResponse.EstimatedDuration,
                User = new Users()
                {
                    Id = proposalResponse.UserObject.Id,
                    Name = proposalResponse.UserObject.Name,
                    Email = proposalResponse.UserObject.Email,
                    Role = new GenericResponse()
                    {
                        Id = role.Id,
                        Name = role.Name,
                    }
                },
                Area = new GenericResponse()
                {
                    Id = area.Id,
                    Name = area.Name
                },
                Type = new GenericResponse()
                {
                    Id = type.Id,
                    Name = type.Name
                },
                Status = new GenericResponse()
                {
                    Id = proposalResponse.Status,
                    Name = proposalResponse.ApprovalStatusObject.Name,
                },
                Steps = approvalSteps.OrderBy(step => step.StepOrder).ToList()
            };
        }

        public async Task<List<ProjectProposal>> GetAllProposalByUser(int idUser)
        {
            return await _mediator.Send(new GetAllProposalByUserQuery(idUser));
        }

        public async Task<Project> GetProjectById(Guid id)
        {
            ProjectProposal proposalResponse = await _mediator.Send(new GetProposalProjectByIdQuery(id));

            if (proposalResponse == null)
                throw new ExceptionNotFound("Proposal not found, please enter a existing proposal.");

            List<ApprovalStep> stepResponse = await _projectApprovalStepService.GetProjectStepsById(id);

            foreach (ApprovalStep step in stepResponse)
            {
                step.Observations = string.IsNullOrEmpty(step.Observations) ? "" : step.Observations;
                step.DecisionDate = step.DecisionDate ?? DateTime.MinValue;
                if (step.ApproverUser == null)
                {
                    step.ApproverUser = new Users()
                    {
                        Id = 0,
                        Name = "",
                        Email = "",
                        Role = new GenericResponse() { Id = 0, Name = "" }
                    };
                }
            }

            if (proposalResponse.Status == 1)
            {
                proposalResponse.Status = 4;
                proposalResponse.ApprovalStatusObject = await _approvalStatusService.GetStatusByIdAsync(proposalResponse.Status);
                _ = _mediator.Send(new UpdateProjectProposalCommand(proposalResponse));
            }

            return new Project()
            {
                Id = proposalResponse.Id,
                Title = proposalResponse.Title,
                Description = proposalResponse.Description,
                Amount = (double)proposalResponse.EstimatedAmount,
                Duration = proposalResponse.EstimatedDuration,
                Area = new GenericResponse() { Id = proposalResponse.AreaObject.Id, Name = proposalResponse.AreaObject.Name },
                Status = new GenericResponse() { Id = proposalResponse.ApprovalStatusObject.Id, Name = proposalResponse.ApprovalStatusObject.Name },
                Type = new GenericResponse() { Id = proposalResponse.ProjectTypeObject.Id, Name = proposalResponse.ProjectTypeObject.Name },
                User = new Users()
                {
                    Id = proposalResponse.UserObject.Id,
                    Name = proposalResponse.UserObject.Name,
                    Email = proposalResponse.UserObject.Email,
                    Role = new GenericResponse() { Id = proposalResponse.UserObject.ApproverRoleObject.Id, Name = proposalResponse.UserObject.ApproverRoleObject.Name }
                },
                Steps = stepResponse
            };
        }

        public async Task<Project> UpdateProposalAsync(Guid id, ProposalUpdateRequest request)
        {

            ProjectProposal project = await _mediator.Send(new GetProposalProjectByIdQuery(id));

            if (project == null)
                throw new ExceptionNotFound("Proposal not found, please enter a existing proposal.");

            if (project.Status != 4)
                throw new ExceptionConflict("The project is no longer in a state that allows modifications.");

            if (string.IsNullOrWhiteSpace(request.Title) || !request.Title.Any(char.IsLetter))
                throw new ExceptionBadRequest("You must provide a valid title with at least one letter.");

            List<string> titles = await _mediator.Send(new GetAllTitleProjectInUse());

            titles = titles.Where(title => title != project.Title).ToList();

            foreach (string title in titles)
            {
                if (title == request.Title)
                {
                    throw new ExceptionBadRequest("The project title is already in use.");
                }
            }

            if (string.IsNullOrWhiteSpace(request.Description) || !request.Description.Any(char.IsLetter))
                throw new ExceptionBadRequest("You must provide a valid description with at least one letter.");

            if( request.Duration <= 0)
                throw new ExceptionBadRequest("Please enter a duration greater than 0.");
            
                project.Title = request.Title;

                project.Description = request.Description;

                project.EstimatedDuration = request.Duration.Value;

            ProjectProposal response = await _mediator.Send(new UpdateProjectProposalCommand(project));

            List<ApprovalStep> list = await _projectApprovalStepService.GetProjectStepsById(project.Id);

            foreach (ApprovalStep step in list)
            {
                step.Observations = string.IsNullOrEmpty(step.Observations) ? "" : step.Observations;
                step.DecisionDate = step.DecisionDate ?? DateTime.MinValue;
                if (step.ApproverUser == null)
                {
                    step.ApproverUser = new Users()
                    {
                        Id = 0,
                        Name = "",
                        Email = "",
                        Role = new GenericResponse() { Id = 0, Name = "" }
                    };
                }
            }

            return new Project()
            {
                Id = response.Id,
                Title = response.Title,
                Description = response.Description,
                Amount = (double)response.EstimatedAmount,
                Duration = response.EstimatedDuration,
                Area = new GenericResponse() { Id = response.AreaObject.Id, Name = response.AreaObject.Name },
                Status = new GenericResponse() { Id = response.ApprovalStatusObject.Id, Name = response.ApprovalStatusObject.Name },
                Type = new GenericResponse() { Id = response.ProjectTypeObject.Id, Name = response.ProjectTypeObject.Name },
                User = new Users()
                {
                    Id = response.UserObject.Id,
                    Name = response.UserObject.Name,
                    Email = response.UserObject.Email,
                    Role = new GenericResponse() { Id = response.UserObject.ApproverRoleObject.Id, Name = response.UserObject.ApproverRoleObject.Name }
                },
                Steps = list,

            };
        }
    }
}
