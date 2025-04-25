using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class DbContextApprover : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<ApprovalStatus> ApprovalStatus { get; set; }
        public DbSet<ApproverRole> ApproverRole { get; set; }
        public DbSet<ProjectType> ProjectType { get; set; }
        public DbSet<ApprovalRule> ApprovalRule { get; set; }
        public DbSet<ProjectProposal> ProjectProposal { get; set; }
        public DbSet<ProjectApprovalStep> ProjectApprovalStep { get; set; }

        public DbContextApprover(DbContextOptions<DbContextApprover> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(25).IsRequired().IsUnicode(false);
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired().IsUnicode(false);

                entity.Property(e => e.Role).IsRequired();
                entity.HasOne(e => e.ApproverRoleObject)
                .WithMany()
                .HasForeignKey(e => e.Role)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("Area");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(25).IsRequired().IsUnicode(false);
            });

            modelBuilder.Entity<ProjectType>(entity =>
            {
                entity.ToTable("ProjectType");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(25).IsRequired().IsUnicode(false);
            });

            modelBuilder.Entity<ApprovalStatus>(entity =>
            {
                entity.ToTable("ApprovalStatus");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(25).IsRequired().IsUnicode(false);
            });

            modelBuilder.Entity<ApproverRole>(entity =>
            {
                entity.ToTable("ApproverRole");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(25).IsRequired().IsUnicode(false);
            });

            modelBuilder.Entity<ApprovalRule>(entity =>
            {
                entity.ToTable("ApprovalRule");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.MinAmount).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.MaxAmount).IsRequired().HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.AreaObject)
                .WithMany()
                .HasForeignKey(e => e.Area)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ProjectTypeObject)
                .WithMany()
                .HasForeignKey(e => e.Type)
                .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.StepOrder).IsRequired();

                entity.Property(e => e.ApproverRoleId).IsRequired();
                entity.HasOne(e => e.ApproverRoleObject)
                .WithMany()
                .HasForeignKey(e => e.ApproverRoleId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ProjectProposal>(entity =>
            {
                entity.ToTable("ProjectProposal");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.Title).HasMaxLength(255).IsRequired().IsUnicode(false);
                entity.Property(e => e.Description).IsRequired().IsUnicode(false);

                entity.Property(e => e.Area).IsRequired();
                entity.HasOne(e => e.AreaObject)
                .WithMany()
                .HasForeignKey(e => e.Area)
                .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Type).IsRequired();
                entity.HasOne(e => e.ProjectTypeObject)
                .WithMany()
                .HasForeignKey(e => e.Type)
                .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.EstimatedAmount).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.EstimatedDuration).IsRequired();

                entity.Property(e => e.Status).IsRequired();
                entity.HasOne(e => e.ApprovalStatusObject)
                .WithMany()
                .HasForeignKey(e => e.Status)
                .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.CreateAt).IsRequired();

                entity.Property(e => e.CreateBy).IsRequired();
                entity.HasOne(e => e.UserObject)
                .WithMany()
                .HasForeignKey(e => e.CreateBy)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ProjectApprovalStep>(entity =>
            {
                entity.ToTable("ProjectApprovalStep");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ProjectProposalId).IsRequired();
                entity.HasOne(e => e.ProjectProposalObject)
                .WithMany()
                .HasForeignKey(e => e.ProjectProposalId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.UserObject)
                .WithMany()
                .HasForeignKey(e => e.ApproverUserId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ApproverRoleId).IsRequired();
                entity.HasOne(e => e.ApproverRoleObject)
                .WithMany()
                .HasForeignKey(e => e.ApproverRoleId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Status).IsRequired();
                entity.HasOne(e => e.ApprovalStatusObject)
                .WithMany()
                .HasForeignKey(e => e.Status)
                .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.StepOrder).IsRequired();
                entity.Property(e => e.DecisionDate);
                entity.Property(e => e.Observations).IsUnicode(false);
            });

            modelBuilder.Entity<ApproverRole>().HasData
                (
                new ApproverRole
                {
                    Id=1,
                    Name="Líder de Área"
                },
                new ApproverRole
                {
                    Id =2,
                    Name ="Gerente"
                },
                new ApproverRole
                {
                    Id =3,
                    Name ="Director"
                },
                new ApproverRole
                {
                    Id =4,
                    Name ="Comité Técnico"
                }
                );

            modelBuilder.Entity<ApprovalStatus>().HasData
                (
                new ApprovalStatus
                {
                    Id =1,
                    Name ="Pending"
                },
                new ApprovalStatus
                {
                    Id =2,
                    Name ="Approved"
                },
                new ApprovalStatus
                {
                    Id =3,
                    Name ="Rejected"
                },
                new ApprovalStatus
                {
                    Id =4,
                    Name ="Observed"
                }
                );

            modelBuilder.Entity<ProjectType>().HasData
                (
                new ProjectType
                {
                    Id =1,
                    Name ="Mejora de Procesos"
                },
                new ProjectType
                {
                    Id =2,
                    Name ="Innovación y Desarrollo"
                },
                new ProjectType
                {
                    Id =3,
                    Name ="Infraestructura"
                },
                new ProjectType
                {
                    Id =4,
                    Name ="Capacitación Interna"
                }
                );
            modelBuilder.Entity<Area>().HasData
                (
                new Area
                {
                    Id =1,
                    Name ="Finanzas"
                },
                new Area
                {
                    Id =2,
                    Name ="Tecnología"
                },
                new Area
                {
                    Id =3,
                    Name ="Recursos Humanos"
                },
                new Area
                {
                    Id =4,
                    Name ="Operaciones"
                }
                );

            modelBuilder.Entity<User>().HasData
                (
                new User
                {
                    Id =1,
                    Name ="José Ferreyra",
                    Email ="jferreyra@unaj.com",
                    Role =2  
                },
                new User
                {
                    Id =2,
                    Name ="Ana Lucero",
                    Email ="alucero@unaj.com",
                    Role =1
                },
                new User
                {
                    Id =3,
                    Name ="Gonzalo Molinas",
                    Email ="gmolinas@unaj.com",
                    Role =2
                },
                new User
                {
                    Id =4,
                    Name ="Lucas Olivera",
                    Email ="lolivera@unaj.com",
                    Role =3
                },
                new User
                {
                    Id =5,
                    Name ="Danilo Fagundez",
                    Email ="dfagundez@unaj.com",
                    Role =4
                },
                new User
                {
                    Id =6,
                    Name ="Gabriel Galli",
                    Email ="ggalli@unaj.com",
                    Role =4
                }
                );
            modelBuilder.Entity<ApprovalRule>().HasData
                (
                new ApprovalRule
                {
                    Id =1,
                    MinAmount =0,
                    MaxAmount =100000,
                    Area=null,
                    Type=null,
                    StepOrder=1,
                    ApproverRoleId=1
                },
                new ApprovalRule
                {
                    Id =2,
                    MinAmount =5000,
                    MaxAmount =20000,
                    Area =null,
                    Type =null,
                    StepOrder =2,
                    ApproverRoleId =2
                },
                new ApprovalRule
                {
                    Id =3,
                    MinAmount =0,
                    MaxAmount =20000,
                    Area =2,
                    Type =2,
                    StepOrder =1,
                    ApproverRoleId =2
                },
                new ApprovalRule
                {
                    Id =4,
                    MinAmount =20000,
                    MaxAmount =0,
                    Area =null,
                    Type =null,
                    StepOrder =3,
                    ApproverRoleId =3
                },
                new ApprovalRule
                {
                    Id =5,
                    MinAmount =5000,
                    MaxAmount =0,
                    Area =1,
                    Type =1,
                    StepOrder =2,
                    ApproverRoleId =2
                },
                new ApprovalRule
                {
                    Id =6,
                    MinAmount =0,
                    MaxAmount =10000,
                    Area =null,
                    Type =2,
                    StepOrder =1,
                    ApproverRoleId =1
                },
                new ApprovalRule
                {
                    Id = 7,
                    MinAmount = 0,
                    MaxAmount = 10000,
                    Area = 2,
                    Type = 1,
                    StepOrder = 1,
                    ApproverRoleId = 4
                },
                new ApprovalRule
                {
                    Id = 8,
                    MinAmount = 10000,
                    MaxAmount = 30000,
                    Area = 2,
                    Type = null,
                    StepOrder = 2,
                    ApproverRoleId = 2
                },
                new ApprovalRule
                {
                    Id = 9,
                    MinAmount = 30000,
                    MaxAmount = 0,
                    Area = 3,
                    Type = null,
                    StepOrder = 2,
                    ApproverRoleId = 3
                },
                new ApprovalRule
                {
                    Id = 10,
                    MinAmount = 0,
                    MaxAmount = 50000,
                    Area = null,
                    Type = 4,
                    StepOrder = 1,
                    ApproverRoleId = 4
                }
                );
        }

    }

 }

