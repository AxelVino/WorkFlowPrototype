using Application.Services.ProposalService.ProposalDtos;
using Domain.Entities;
using MediatR;

namespace Application.Services.ProjectApprovalStepService.ProjectApprovalStepQuerys
{
    public record GetListApprovalStepsQuery(ProposalFilterRequest request) : IRequest<List<ProjectApprovalStep>>;
}
