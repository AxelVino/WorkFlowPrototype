using Application.Interfaces.ApprovalRule;
using Application.Interfaces.ApprovalStatus;
using Application.Interfaces.ApproverRole;
using Application.Interfaces.Area;
using Application.Interfaces.ProjectApprovalStep;
using Application.Interfaces.ProjectProposal;
using Application.Interfaces.ProjectType;
using Application.Interfaces.Repository;
using Application.Interfaces.User;
using Application.Services.ApprovalRuleService;
using Application.Services.ApprovalStatusService;
using Application.Services.ApproverRoleService;
using Application.Services.AreaService;
using Application.Services.ProjectApprovalStepService;
using Application.Services.ProjectTypeService;
using Application.Services.ProposalService;
using Application.Services.UserService;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder => builder
            .WithOrigins("http://127.0.0.1:5500")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DbContextApprover>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DbContextApprover>());
builder.Services.AddScoped<IProjectTypeService, ProjectTypeService>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped(typeof(IProjectApprovalStepRepository), typeof(ProjectApprovalStepRepository));
builder.Services.AddScoped(typeof(IApprovalRuleRepository), typeof(ApprovalRuleRepository));
builder.Services.AddScoped(typeof(IProjectProposalRepository), typeof(ProjectProposalRepository));

builder.Services.AddScoped<IApproverRoleService, ApproverRoleService>();
builder.Services.AddScoped<IApprovalStatusService, ApprovalStatusService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectProposalService, ProposalService>();
builder.Services.AddScoped<IProjectApprovalStepService, ProjectApprovalStepService>();
builder.Services.AddScoped<IApprovalRuleService, ApprovalRuleService>();

builder.Services.AddMediatR(typeof(ApproverRoleService).Assembly);

var app = builder.Build();

app.UseCors("AllowLocalhost");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
