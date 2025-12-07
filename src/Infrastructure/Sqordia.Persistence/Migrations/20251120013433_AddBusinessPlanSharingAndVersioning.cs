using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sqordia.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessPlanSharingAndVersioning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessPlanShares",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SharedWithUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SharedWithEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Permission = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    PublicToken = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastAccessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccessCount = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessPlanShares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessPlanShares_BusinessPlans_BusinessPlanId",
                        column: x => x.BusinessPlanId,
                        principalTable: "BusinessPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessPlanShares_Users_SharedWithUserId",
                        column: x => x.SharedWithUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "BusinessPlanVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ExecutiveSummary = table.Column<string>(type: "text", nullable: true),
                    ProblemStatement = table.Column<string>(type: "text", nullable: true),
                    Solution = table.Column<string>(type: "text", nullable: true),
                    MarketAnalysis = table.Column<string>(type: "text", nullable: true),
                    CompetitiveAnalysis = table.Column<string>(type: "text", nullable: true),
                    SwotAnalysis = table.Column<string>(type: "text", nullable: true),
                    BusinessModel = table.Column<string>(type: "text", nullable: true),
                    MarketingStrategy = table.Column<string>(type: "text", nullable: true),
                    BrandingStrategy = table.Column<string>(type: "text", nullable: true),
                    OperationsPlan = table.Column<string>(type: "text", nullable: true),
                    ManagementTeam = table.Column<string>(type: "text", nullable: true),
                    FinancialProjections = table.Column<string>(type: "text", nullable: true),
                    FundingRequirements = table.Column<string>(type: "text", nullable: true),
                    RiskAnalysis = table.Column<string>(type: "text", nullable: true),
                    ExitStrategy = table.Column<string>(type: "text", nullable: true),
                    AppendixData = table.Column<string>(type: "text", nullable: true),
                    MissionStatement = table.Column<string>(type: "text", nullable: true),
                    SocialImpact = table.Column<string>(type: "text", nullable: true),
                    BeneficiaryProfile = table.Column<string>(type: "text", nullable: true),
                    GrantStrategy = table.Column<string>(type: "text", nullable: true),
                    SustainabilityPlan = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PlanType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessPlanVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessPlanVersions_BusinessPlans_BusinessPlanId",
                        column: x => x.BusinessPlanId,
                        principalTable: "BusinessPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPlanShares_BusinessPlanId",
                table: "BusinessPlanShares",
                column: "BusinessPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPlanShares_BusinessPlanId_IsActive",
                table: "BusinessPlanShares",
                columns: new[] { "BusinessPlanId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPlanShares_PublicToken",
                table: "BusinessPlanShares",
                column: "PublicToken",
                unique: true,
                filter: "[PublicToken] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPlanShares_SharedWithUserId",
                table: "BusinessPlanShares",
                column: "SharedWithUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPlanVersions_BusinessPlanId",
                table: "BusinessPlanVersions",
                column: "BusinessPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPlanVersions_BusinessPlanId_VersionNumber",
                table: "BusinessPlanVersions",
                columns: new[] { "BusinessPlanId", "VersionNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPlanVersions_Created",
                table: "BusinessPlanVersions",
                column: "Created");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessPlanShares");

            migrationBuilder.DropTable(
                name: "BusinessPlanVersions");
        }
    }
}
