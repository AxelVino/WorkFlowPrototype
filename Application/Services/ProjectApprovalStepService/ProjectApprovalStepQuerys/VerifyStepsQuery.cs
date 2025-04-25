using MediatR;

namespace Application.Services.ProjectApprovalStepService.ProjectApprovalStepQuerys
{
    public record VerifyStepsQuery(Guid Guid): IRequest<bool>;
}
