using Application.Interfaces.ApprovalStatus;
using Application.Interfaces.ApproverRole;
using Application.Interfaces.ProjectApprovalStep;
using Application.Services.ProjectApprovalStepService.ProjectApproalStepDtos;
using Application.Services.ProjectApprovalStepService.ProjectApprovalStepCommands;
using Application.Services.ProjectApprovalStepService.ProjectApprovalStepQuerys;
using Application.Services.ProposalService.ProposalCommands;
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

        public ProjectApprovalStepService(IMediator mediator, IApproverRoleService approverRoleService, IApprovalStatusService approvalStatusService)
        {
            _mediator = mediator;
            _approverRoleService = approverRoleService;
            _approverStatusService = approvalStatusService;
        }

        public async Task<bool> CreateProjectApprovalStepAsync(IncompletedProjectDto incompleted)
        {
            List<ProjectApprovalStep> newList = [];
            while (incompleted.StepOrder > 0 )
            {
                ProjectApprovalStep project = new()
                {
                    ProjectProposalId = incompleted.ProjectProposalId,
                    ProjectProposalObject = incompleted.ProjectProposalObject,
                    ApproverRoleId = incompleted.ApproverRoleId,
                    ApproverRoleObject = await _approverRoleService.GetApproverRoleByIdAsync(incompleted.ApproverRoleId),
                    Status = incompleted.Status,
                    ApprovalStatusObject = incompleted.ApprovalStatusObject,
                    StepOrder = incompleted.StepOrder,
                };

                newList.Add(project);

                incompleted.StepOrder--;
                incompleted.ApproverRoleId--;
            }

            CreateProjectApprovalSteps command = new()
            { 
                Steps = newList
            };

            return await _mediator.Send(command);

        }

        public async Task<bool> ApproveProjectStepAsync(ProjectApprovalStep project)
        {
            bool result = false;

            project.Status = 2;
            project.ApprovalStatusObject = await _approverStatusService.GetStatusByIdAsync(project.Status);

            bool approved = await _mediator.Send(new UpdateProjectApprovalStep(project));

            if(approved)
            {
                result = await _mediator.Send(new VerifyStepsQuery(project.ProjectProposalId));
            }

            if (result)
            {
                project.ProjectProposalObject.Status = 2;
                project.ProjectProposalObject.ApprovalStatusObject = project.ApprovalStatusObject;
                approved = await _mediator.Send(new UpdateProjectProposalCommand(project.ProjectProposalObject));
            }

            return approved;
        }

        public async Task<bool> RejectProjectStepAsync(ProjectApprovalStep project)
        {
            bool result = false;

            project.Status = 3;
            project.ApprovalStatusObject = await _approverStatusService.GetStatusByIdAsync(project.Status);

            bool rejected = await _mediator.Send(new UpdateProjectApprovalStep(project));

            if (rejected)
            {
                project.ProjectProposalObject.Status = 3;
                project.ProjectProposalObject.ApprovalStatusObject = project.ApprovalStatusObject;

                result = await _mediator.Send(new UpdateProjectProposalCommand(project.ProjectProposalObject));
            }

            return result;
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
    }
}
