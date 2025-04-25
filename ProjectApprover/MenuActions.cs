using Application.Interfaces.ProjectApprovalStep;
using Application.Interfaces.ProjectProposal;
using Application.Services.ProposalService.ProposalCommands;
using Domain.Entities;
using Application.Interfaces.User;

namespace MyApp.Cli
{
        public class MenuService
        {
            private readonly IUserService _userService;
            private readonly IProjectProposalService _proposalService;
            private readonly IProjectApprovalStepService _projectApprovalService;

            public MenuService(
                IUserService userService,
                IProjectProposalService proposalService,
                IProjectApprovalStepService projectApprovalService)
            {
                _userService = userService;
                _proposalService = proposalService;
                _projectApprovalService = projectApprovalService;
            }

            public async Task RunAsync()
            {
                bool exit = false;

                while (!exit)
                {
                    Console.Clear();
                    Console.WriteLine("=== LOGIN SYSTEM ===\n");

                    try
                    {
                        Console.Write("Enter your user ID: ");
                        if (!int.TryParse(Console.ReadLine(), out int userId))
                        {
                            ShowError("Invalid ID. Please enter a number.");
                            continue;
                        }

                        var user = await _userService.GetUserByIdAsync(userId)
                                   ?? throw new Exception($"The user with ID {userId} was not found.");

                        bool inSession = true;
                        while (inSession)
                        {
                            Console.Clear();
                            Console.WriteLine($"Welcome user: {user.Name} (ID:{user.Id})\n");
                            Console.WriteLine("Select an option:");
                            Console.WriteLine("1. Create new project");
                            Console.WriteLine("2. View the state of my projects");
                            Console.WriteLine("3. View projects to approve");
                            Console.WriteLine("4. Change user");
                            Console.WriteLine("5. Exit");
                            Console.Write("Option: ");
                            string? option = Console.ReadLine();
                            try
                            {
                                switch (option)
                                {
                                    case "1":
                                        await CreateNewProjectAsync(user);
                                        break;

                                    case "2":
                                        await ShowUserProjectsAsync(user);
                                        break;

                                    case "3":
                                        await ReviewPendingProjectsAsync(user);
                                        break;

                                    case "4":
                                        inSession = false;
                                        break;

                                    case "5":
                                        inSession = false;
                                        exit = true;
                                        break;

                                    default:
                                        ShowError("Invalid option.");
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                ShowError(ex.Message);
                            }

                            if (inSession)
                            {
                                Console.WriteLine("\nPress any key to continue...");
                                Console.ReadKey();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex.Message);
                    }
                }
            }

            private static void ShowError(string message)
            {
                Console.Clear();
                Console.WriteLine("*** ERROR ***");
                Console.WriteLine(message);
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }

            private async Task CreateNewProjectAsync(User user)
            {
                Console.Clear();
                Console.Write("Title: ");
                string? title = Console.ReadLine();
                Console.Write("Description: ");
                string? description = Console.ReadLine();
                Console.Write("Area: ");
                int area = int.Parse(Console.ReadLine());
                Console.Write("Type: ");
                int type = int.Parse(Console.ReadLine());
                Console.Write("Estimated amount: ");
                decimal amount = decimal.Parse(Console.ReadLine());
                Console.Write("Estimated duration (days): ");
                int duration = int.Parse(Console.ReadLine());

                CreateProjectProposalCommand command = new()
                {
                    Title = title,
                    Description = description,
                    Area = area,
                    Type = type,
                    EstimatedAmount = amount,
                    EstimatedDuration = duration,
                    Status = 1,
                    CreateAt = DateTime.Now,
                    CreateBy = user.Id,
                    UserObject = user
                };

                await _proposalService.CreateProjectProposalAsync(command);
                Console.WriteLine("");
                Console.WriteLine("Project created successfully!");
            }

            private async Task ShowUserProjectsAsync(User user)
            {
                Console.Clear();
                List<ProjectProposal> proposals = await _proposalService.GetAllProposalByUser(user.Id);

                if (proposals.Count == 0)
                {
                    Console.WriteLine("No projects to show.");
                    return;
                }

                foreach (var p in proposals)
                {
                    Console.WriteLine($"PROJECT: {p.Title}  STATUS: {p.ApprovalStatusObject.Name}");
                    Console.WriteLine($"Description: {p.Description}");
                    Console.WriteLine($"Type: {p.ProjectTypeObject.Name}  Area: {p.AreaObject.Name}");
                    Console.WriteLine($"Estimated Amount: {p.EstimatedAmount}  Duration: {p.EstimatedDuration} days");
                    Console.WriteLine("--------------------------------------------------\n");
                }
            }

            private async Task ReviewPendingProjectsAsync(User user)
            {
                Console.Clear();
                var steps = await _projectApprovalService.GetListProjectsById(user.Role);

                if (steps.Count == 0)
                {
                    Console.WriteLine("No projects pending your approval...");
                    return;
                }
                    

                while (true)
                {
                    steps = await _projectApprovalService.GetListProjectsById(user.Role);
                    Console.Clear();
                    Console.WriteLine("Projects pending your approval:\n");

                    for (int i = 0; i < steps.Count; i++)
                    {
                        var p = steps[i].ProjectProposalObject;
                        Console.WriteLine($"{i}) PROJECT: {p.Title}");
                        Console.WriteLine($"   Description: {p.Description}");
                        Console.WriteLine($"   Applicant: {p.UserObject.Name}");
                        Console.WriteLine($"   Type: {p.ProjectTypeObject.Name}  Area: {p.AreaObject.Name}");
                        Console.WriteLine($"   Estimated: ${p.EstimatedAmount}  Duration: {p.EstimatedDuration} days");
                        Console.WriteLine("--------------------------------------------------");
                    }

                    Console.Write("Select project number to approve/reject (or 'exit'): ");
                    string input = Console.ReadLine();
                    if (input?.ToLower() == "exit") break;

                    if (!int.TryParse(input, out int index) || index < 0 || index >= steps.Count)
                    {
                        Console.WriteLine("Invalid selection. Press any key...");
                        Console.ReadKey();
                        continue;
                    }

                    var selected = steps[index];
                    Console.Write("Approve (A) or Reject (R)?: ");
                    string action = Console.ReadLine()?.Trim().ToUpper();
                    if (action != "A" && action != "R") continue;

                    Console.Write("Confirm (Y/N): ");
                    string confirm = Console.ReadLine()?.Trim().ToUpper();
                    if (confirm != "Y") continue;

                    selected.DecisionDate = DateTime.Now;
                    selected.ApproverUserId = user.Id;
                    selected.UserObject = user;

                    Console.Write("Observations: ");
                    selected.Observations = Console.ReadLine();

                    bool result = (action == "A")
                        ? await _projectApprovalService.ApproveProjectStepAsync(selected)
                        : await _projectApprovalService.RejectProjectStepAsync(selected);
                    Console.WriteLine("");
                    Console.WriteLine(result
                        ? $"Project {(action == "A" ? "approved" : "rejected")}!"
                        : "Something went wrong.");
                    Console.WriteLine("");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }

