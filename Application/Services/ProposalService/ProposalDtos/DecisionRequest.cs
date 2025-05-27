using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ProposalService.ProposalDtos
{
    public class DecisionRequest
    {
        public required long Id {  get; set; }
        public required int User {  get; set; }
        public required int Status { get; set; }
        public required string Observation { get; set; }
    }
}
