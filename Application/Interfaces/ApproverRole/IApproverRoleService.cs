﻿using Application.Responses;
using Domain.Entities;

namespace Application.Interfaces.ApproverRole
{
    public interface IApproverRoleService
    {
        Task<Domain.Entities.ApproverRole> GetApproverRoleByIdAsync(int id);
        Task<List<GenericResponse>> GetAllApproverRoles();
    }
}
