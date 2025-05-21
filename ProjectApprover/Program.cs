using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using MediatR;
using Application.Interfaces.Repository;
using Application.Interfaces.ProjectProposal;
using Application.Services.UserService.UserHandlers;
using Application.Services.ProposalService;
using Application.Interfaces.User;
using Application.Services.UserService;
using Application.Interfaces.Area;
using Application.Services.AreaService;
using Application.Interfaces.ApprovalStatus;
using Application.Interfaces.ProjectType;
using Application.Services.ApprovalStatusService;
using Application.Services.ProjectTypeService;
using Application.Interfaces.ApproverRole;
using Application.Services.ApproverRoleService;
using Application.Interfaces.ApprovalRule;
using Application.Services.ApprovalRuleService;
using Application.Interfaces.ProjectApprovalStep;
using Application.Services.ProjectApprovalStepService;
using Infrastructure.Persistence.Repositories;
using MyApp.Cli;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        var env = context.HostingEnvironment;
        config.SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<DbContextApprover>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<DbContext>(provider => provider.GetRequiredService<DbContextApprover>());
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped(typeof(IProjectApprovalStepRepository), typeof(ProjectApprovalStepRepository));
        services.AddScoped<IProjectProposalService, ProposalService>();
        services.AddScoped<IProjectProposalRepository, ProjectProposalRepository>();
        services.AddScoped<IApprovalRuleRepository, ApprovalRuleRepository>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAreaService, AreaService>();
        services.AddScoped<IApprovalStatusService, ApprovalStatusService>();
        services.AddScoped<IProjectTypeService, ProjectTypeService>();
        services.AddScoped<IApproverRoleService, ApproverRoleService>();
        services.AddScoped<IApprovalRuleService, ApprovalRuleService>();
        services.AddScoped<IProjectApprovalStepService, ProjectApprovalStepService>();

        services.AddMediatR(typeof(GetUserByIdHandler).Assembly);
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var userService = services.GetRequiredService<IUserService>();
    var proposalService = services.GetRequiredService<IProjectProposalService>();
    var approvalService = services.GetRequiredService<IProjectApprovalStepService>();

    var menu = new MenuService(userService, proposalService, approvalService);
    await menu.RunAsync();
}