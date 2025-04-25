using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ProposalService.ProposalQuerys
{
    public record GetAllProposalByUserQuery(int IdUser) : IRequest<List<ProjectProposal>>;
}
