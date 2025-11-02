using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sqordia.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOBNLTemplateAndFinancialFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DecimalPlaces = table.Column<int>(type: "int", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                    table.UniqueConstraint("AK_Currencies_Code", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "OBNLBusinessPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OBNLType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vision = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Values = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FundingRequirements = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FundingPurpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComplianceStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ComplianceLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ComplianceLastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ComplianceNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LegalStructure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GoverningBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BoardComposition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StakeholderEngagement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImpactMeasurement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SustainabilityStrategy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrantApplications = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportingRequirements = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskManagement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SuccessMetrics = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBNLBusinessPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OBNLBusinessPlans_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetAudience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    UsageCount = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RatingCount = table.Column<int>(type: "int", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreviewImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Changelog = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUsed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromCurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToCurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InverseRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Spread = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeRates_Currencies_FromCurrencyId",
                        column: x => x.FromCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExchangeRates_Currencies_ToCurrencyId",
                        column: x => x.ToCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FinancialKPIs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MetricType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    TargetValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PreviousValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChangePercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Trend = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Benchmark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialKPIs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialKPIs_BusinessPlans_BusinessPlanId",
                        column: x => x.BusinessPlanId,
                        principalTable: "BusinessPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialKPIs_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FinancialProjectionItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Scenario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BaseAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    Frequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrowthRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Assumptions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialProjectionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialProjectionItems_BusinessPlans_BusinessPlanId",
                        column: x => x.BusinessPlanId,
                        principalTable: "BusinessPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialProjectionItems_Currencies_CurrencyCode",
                        column: x => x.CurrencyCode,
                        principalTable: "Currencies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentAnalyses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InitialInvestment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpectedReturn = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetPresentValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InternalRateOfReturn = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaybackPeriod = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReturnOnInvestment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AnalysisPeriod = table.Column<int>(type: "int", nullable: false),
                    RiskLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvestmentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvestorType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Valuation = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EquityOffering = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FundingRequired = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FundingStage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Assumptions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentAnalyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentAnalyses_BusinessPlans_BusinessPlanId",
                        column: x => x.BusinessPlanId,
                        principalTable: "BusinessPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentAnalyses_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaxRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPercentage = table.Column<bool>(type: "bit", nullable: false),
                    CalculationMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicableTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrencyCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LegalReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxRules_Currencies_CurrencyCode",
                        column: x => x.CurrencyCode,
                        principalTable: "Currencies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GrantApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OBNLBusinessPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrantName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrantingOrganization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrantType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MatchingFunds = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Objectives = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpectedOutcomes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetPopulation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeographicScope = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timeline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BudgetBreakdown = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EvaluationPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SustainabilityPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationDeadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Decision = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrantApplications_OBNLBusinessPlans_OBNLBusinessPlanId",
                        column: x => x.OBNLBusinessPlanId,
                        principalTable: "OBNLBusinessPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImpactMeasurements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OBNLBusinessPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MetricName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeasurementType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitOfMeasurement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaselineValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TargetValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CollectionMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Frequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponsibleParty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastMeasurement = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextMeasurement = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImpactMeasurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImpactMeasurements_OBNLBusinessPlans_OBNLBusinessPlanId",
                        column: x => x.OBNLBusinessPlanId,
                        principalTable: "OBNLBusinessPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OBNLCompliances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OBNLBusinessPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequirementType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Jurisdiction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegulatoryBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComplianceLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Documentation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBNLCompliances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OBNLCompliances_OBNLBusinessPlans_OBNLBusinessPlanId",
                        column: x => x.OBNLBusinessPlanId,
                        principalTable: "OBNLBusinessPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateCustomizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customizations = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    UsageCount = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RatingCount = table.Column<int>(type: "int", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Changelog = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUsed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateCustomizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateCustomizations_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateCustomizations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateRatings_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateRatings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    SectionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Placeholder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValidationRules = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HelpText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateSections_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateUsages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsageType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Referrer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateUsages_BusinessPlans_BusinessPlanId",
                        column: x => x.BusinessPlanId,
                        principalTable: "BusinessPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TemplateUsages_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateUsages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxCalculations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FinancialProjectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxRuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxableAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CalculationMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxPeriod = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxCalculations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxCalculations_FinancialProjectionItems_FinancialProjectionId",
                        column: x => x.FinancialProjectionId,
                        principalTable: "FinancialProjectionItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaxCalculations_TaxRules_TaxRuleId",
                        column: x => x.TaxRuleId,
                        principalTable: "TaxRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateFields",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateSectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FieldType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Placeholder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsReadOnly = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ValidationRules = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Options = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HelpText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Format = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinLength = table.Column<int>(type: "int", nullable: false),
                    MaxLength = table.Column<int>(type: "int", nullable: false),
                    MinValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Pattern = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateFields_TemplateSections_TemplateSectionId",
                        column: x => x.TemplateSectionId,
                        principalTable: "TemplateSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_FromCurrencyId",
                table: "ExchangeRates",
                column: "FromCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_ToCurrencyId",
                table: "ExchangeRates",
                column: "ToCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialKPIs_BusinessPlanId",
                table: "FinancialKPIs",
                column: "BusinessPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialKPIs_CurrencyId",
                table: "FinancialKPIs",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialProjectionItems_BusinessPlanId",
                table: "FinancialProjectionItems",
                column: "BusinessPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialProjectionItems_CurrencyCode",
                table: "FinancialProjectionItems",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_GrantApplications_OBNLBusinessPlanId",
                table: "GrantApplications",
                column: "OBNLBusinessPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ImpactMeasurements_OBNLBusinessPlanId",
                table: "ImpactMeasurements",
                column: "OBNLBusinessPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentAnalyses_BusinessPlanId",
                table: "InvestmentAnalyses",
                column: "BusinessPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentAnalyses_CurrencyId",
                table: "InvestmentAnalyses",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_OBNLBusinessPlans_OrganizationId",
                table: "OBNLBusinessPlans",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OBNLCompliances_OBNLBusinessPlanId",
                table: "OBNLCompliances",
                column: "OBNLBusinessPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxCalculations_FinancialProjectionId",
                table: "TaxCalculations",
                column: "FinancialProjectionId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxCalculations_TaxRuleId",
                table: "TaxCalculations",
                column: "TaxRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxRules_CurrencyCode",
                table: "TaxRules",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateCustomizations_TemplateId",
                table: "TemplateCustomizations",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateCustomizations_UserId",
                table: "TemplateCustomizations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateFields_TemplateSectionId",
                table: "TemplateFields",
                column: "TemplateSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateRatings_TemplateId",
                table: "TemplateRatings",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateRatings_UserId",
                table: "TemplateRatings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateSections_TemplateId",
                table: "TemplateSections",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateUsages_BusinessPlanId",
                table: "TemplateUsages",
                column: "BusinessPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateUsages_TemplateId",
                table: "TemplateUsages",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateUsages_UserId",
                table: "TemplateUsages",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExchangeRates");

            migrationBuilder.DropTable(
                name: "FinancialKPIs");

            migrationBuilder.DropTable(
                name: "GrantApplications");

            migrationBuilder.DropTable(
                name: "ImpactMeasurements");

            migrationBuilder.DropTable(
                name: "InvestmentAnalyses");

            migrationBuilder.DropTable(
                name: "OBNLCompliances");

            migrationBuilder.DropTable(
                name: "TaxCalculations");

            migrationBuilder.DropTable(
                name: "TemplateCustomizations");

            migrationBuilder.DropTable(
                name: "TemplateFields");

            migrationBuilder.DropTable(
                name: "TemplateRatings");

            migrationBuilder.DropTable(
                name: "TemplateUsages");

            migrationBuilder.DropTable(
                name: "OBNLBusinessPlans");

            migrationBuilder.DropTable(
                name: "FinancialProjectionItems");

            migrationBuilder.DropTable(
                name: "TaxRules");

            migrationBuilder.DropTable(
                name: "TemplateSections");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Templates");
        }
    }
}
