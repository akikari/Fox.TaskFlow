//==================================================================================================
// Entity Framework Core DbContext for Fox.TaskFlow application.
// Configures SQLite database with task entities and seeding.
//==================================================================================================
using Fox.TaskFlow.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fox.TaskFlow.Infrastructure.Data;

//==================================================================================================
/// <summary>
/// Database context for the TaskFlow application.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TaskFlowDbContext"/> class.
/// </remarks>
/// <param name="options">The DbContext options.</param>
//==================================================================================================
public sealed class TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options) : DbContext(options)
{
    //==============================================================================================
    /// <summary>
    /// Gets or sets the Tasks DbSet.
    /// </summary>
    //==============================================================================================
    public DbSet<TaskEntity> Tasks { get; set; }

    //==============================================================================================
    /// <summary>
    /// Configures the model and relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    //==============================================================================================
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskEntity>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);

            entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);

            entity.Property(e => e.DueDate).IsRequired(false);

            entity.Property(e => e.Status).IsRequired().HasConversion<string>();

            entity.Property(e => e.CreatedAt).IsRequired();

            entity.Property(e => e.UpdatedAt).IsRequired();

            entity.HasData(
                new TaskEntity
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Title = "Setup development environment",
                    Description = "Install required tools and configure the development environment for the new project. This includes IDE setup, database tools, and version control configuration.",
                    Status = TaskStatus.Completed,
                    CreatedAt = DateTime.Parse("2026-03-01T08:00:00"),
                    UpdatedAt = DateTime.Parse("2026-03-01T10:30:00"),
                    DueDate = DateTime.Parse("2026-03-01T12:00:00")
                },
                new TaskEntity
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Title = "Implement user authentication",
                    Description = "Create a secure authentication system with JWT tokens, password hashing, and role-based authorization. Include login, logout, and password reset functionality.",
                    Status = TaskStatus.InProgress,
                    CreatedAt = DateTime.Parse("2026-03-01T09:00:00"),
                    UpdatedAt = DateTime.Parse("2026-03-01T14:00:00"),
                    DueDate = DateTime.Parse("2026-03-05T17:00:00")
                },
                new TaskEntity
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Title = "Design database schema",
                    Description = "Create entity relationship diagrams and design the database schema for all application entities. Consider normalization, indexes, and performance optimization.",
                    Status = TaskStatus.Created,
                    CreatedAt = DateTime.Parse("2026-03-01T10:00:00"),
                    UpdatedAt = DateTime.Parse("2026-03-01T10:00:00"),
                    DueDate = DateTime.Parse("2026-03-03T17:00:00")
                },
                new TaskEntity
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Title = "Write API documentation",
                    Description = "Document all REST API endpoints with OpenAPI/Swagger specifications. Include request/response examples, authentication requirements, and error codes.",
                    Status = TaskStatus.Created,
                    CreatedAt = DateTime.Parse("2026-03-01T11:00:00"),
                    UpdatedAt = DateTime.Parse("2026-03-01T11:00:00"),
                    DueDate = null
                },
                new TaskEntity
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Title = "Implement legacy payment gateway",
                    Description = "Integration with the old payment provider that is being phased out. This task was cancelled due to business decision to skip the legacy system.",
                    Status = TaskStatus.Cancelled,
                    CreatedAt = DateTime.Parse("2026-02-28T14:00:00"),
                    UpdatedAt = DateTime.Parse("2026-03-01T09:00:00"),
                    DueDate = DateTime.Parse("2026-03-02T17:00:00")
                }
            );
        });
    }
}
