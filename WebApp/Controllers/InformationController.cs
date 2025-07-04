﻿using Application.Interfaces.ApprovalStatus;
using Application.Interfaces.ApproverRole;
using Application.Interfaces.Area;
using Application.Interfaces.ProjectType;
using Application.Interfaces.User;
using Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Route("api/")]
    [ApiController]
    public class InformationController : Controller
    {
        private readonly IUserService _userService;
        private readonly IApproverRoleService _approverRoleService;
        private readonly IApprovalStatusService _approvalStatusService;
        private readonly IProjectTypeService _projectTypeService;
        private readonly IAreaService _areaService;
        private readonly ILogger<InformationController> _logger;
       
        public InformationController(ILogger<InformationController> logger, IApproverRoleService approverRoleService, IUserService userService,
            IApprovalStatusService approvalStatusService, IProjectTypeService projectTypeService, IAreaService areaService)
        {
            _logger = logger;
            _approverRoleService = approverRoleService;
            _userService = userService;
            _approvalStatusService = approvalStatusService;
            _projectTypeService = projectTypeService;
            _areaService = areaService;
        }
        [HttpGet("Area")]
        [ProducesResponseType(typeof(List<GenericResponse>), 200)]
        public async Task<IActionResult> GetAreas()
        {
            return Ok(await _areaService.GetAllAreasAsync());
        }
        [HttpGet("ProjectType")]
        [ProducesResponseType(typeof(List<GenericResponse>), 200)]
        public async Task<IActionResult> GetProyectTypes()
        {
            return Ok(await _projectTypeService.GetAllProjectTypes());
        }
        [HttpGet("Role")]
        [ProducesResponseType(typeof(List<GenericResponse>), 200)]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _approverRoleService.GetAllApproverRoles());
        }
        [HttpGet("ApprovalStatus")]
        [ProducesResponseType(typeof(List<GenericResponse>), 200)]
        public async Task<IActionResult> GetApprovalStatuses()
        {
            return Ok(await _approvalStatusService.GetAllApprovalStatus());
        }
        [HttpGet("User")]
        [ProducesResponseType(typeof(List<Users>), 200)]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

    }
}
