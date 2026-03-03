using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.TaskFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: ["Id", "CreatedAt", "Description", "DueDate", "Status", "Title", "UpdatedAt"],
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 3, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Install required tools and configure the development environment for the new project. This includes IDE setup, database tools, and version control configuration.", new DateTime(2026, 3, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "Completed", "Setup development environment", new DateTime(2026, 3, 1, 10, 30, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 3, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "Create a secure authentication system with JWT tokens, password hashing, and role-based authorization. Include login, logout, and password reset functionality.", new DateTime(2026, 3, 5, 17, 0, 0, 0, DateTimeKind.Unspecified), "InProgress", "Implement user authentication", new DateTime(2026, 3, 1, 14, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 3, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Create entity relationship diagrams and design the database schema for all application entities. Consider normalization, indexes, and performance optimization.", new DateTime(2026, 3, 3, 17, 0, 0, 0, DateTimeKind.Unspecified), "Created", "Design database schema", new DateTime(2026, 3, 1, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 3, 1, 11, 0, 0, 0, DateTimeKind.Unspecified), "Document all REST API endpoints with OpenAPI/Swagger specifications. Include request/response examples, authentication requirements, and error codes.", null, "Created", "Write API documentation", new DateTime(2026, 3, 1, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 2, 28, 14, 0, 0, 0, DateTimeKind.Unspecified), "Integration with the old payment provider that is being phased out. This task was cancelled due to business decision to skip the legacy system.", new DateTime(2026, 3, 2, 17, 0, 0, 0, DateTimeKind.Unspecified), "Cancelled", "Implement legacy payment gateway", new DateTime(2026, 3, 1, 9, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);

            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
