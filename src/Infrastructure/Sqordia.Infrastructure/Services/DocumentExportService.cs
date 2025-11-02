using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using PuppeteerSharp;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Sqordia.Application.Common.Interfaces;
using Sqordia.Application.Common.Models;
using Sqordia.Application.Services;
using Sqordia.Domain.Entities.BusinessPlan;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Sqordia.Infrastructure.Services;

/// <summary>
/// Service for exporting business plans to various document formats
/// </summary>
public class DocumentExportService : IDocumentExportService
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<DocumentExportService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DocumentExportService(
        IApplicationDbContext context,
        ILogger<DocumentExportService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;

        // Configure QuestPDF license
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<Result<ExportResult>> ExportToPdfAsync(Guid businessPlanId, string language = "fr", CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting PDF export for business plan {BusinessPlanId} in {Language}", businessPlanId, language);

            var businessPlan = await GetBusinessPlanWithValidationAsync(businessPlanId, cancellationToken);
            if (businessPlan == null)
            {
                return Result.Failure<ExportResult>("Business plan not found or access denied");
            }

            // Generate PDF using QuestPDF
            var pdfBytes = GeneratePdfWithQuestPDF(businessPlan, language);

            var result = new ExportResult
            {
                FileData = pdfBytes,
                FileName = $"{SanitizeFileName(businessPlan.Title)}_{language}_{DateTime.UtcNow:yyyyMMdd}.pdf",
                ContentType = "application/pdf",
                FileSizeBytes = pdfBytes.Length,
                Language = language,
                Template = "default"
            };

            _logger.LogInformation("PDF export completed successfully. File size: {FileSize} bytes", pdfBytes.Length);
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting business plan {BusinessPlanId} to PDF", businessPlanId);
            return Result.Failure<ExportResult>($"Export failed: {ex.Message}");
        }
    }

    public async Task<Result<ExportResult>> ExportToWordAsync(Guid businessPlanId, string language = "fr", CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting Word export for business plan {BusinessPlanId} in {Language}", businessPlanId, language);

            var businessPlan = await GetBusinessPlanWithValidationAsync(businessPlanId, cancellationToken);
            if (businessPlan == null)
            {
                return Result.Failure<ExportResult>("Business plan not found or access denied");
            }

            // Generate Word document
            var wordBytes = GenerateWordDocument(businessPlan, language);

            var result = new ExportResult
            {
                FileData = wordBytes,
                FileName = $"{SanitizeFileName(businessPlan.Title)}_{language}_{DateTime.UtcNow:yyyyMMdd}.docx",
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                FileSizeBytes = wordBytes.Length,
                Language = language,
                Template = "default"
            };

            _logger.LogInformation("Word export completed successfully. File size: {FileSize} bytes", wordBytes.Length);
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting business plan {BusinessPlanId} to Word", businessPlanId);
            return Result.Failure<ExportResult>($"Export failed: {ex.Message}");
        }
    }

    public async Task<Result<string>> ExportToHtmlAsync(Guid businessPlanId, string language = "fr", CancellationToken cancellationToken = default)
    {
        try
        {
            var businessPlan = await GetBusinessPlanWithValidationAsync(businessPlanId, cancellationToken);
            if (businessPlan == null)
            {
                return Result.Failure<string>("Business plan not found or access denied");
            }

            var html = GenerateHtmlContent(businessPlan, language);
            return Result.Success(html);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting business plan {BusinessPlanId} to HTML", businessPlanId);
            return Result.Failure<string>($"Export failed: {ex.Message}");
        }
    }

    public async Task<Result<List<ExportTemplate>>> GetAvailableTemplatesAsync()
    {
        await Task.CompletedTask; // For future template expansion

        var templates = new List<ExportTemplate>
        {
            new()
            {
                Id = "default",
                Name = "Default Template",
                Description = "Clean, professional business plan template with standard formatting",
                IsDefault = true,
                SupportedFormats = new List<string> { "pdf", "docx", "html" },
                SupportedLanguages = new List<string> { "fr", "en" }
            },
            new()
            {
                Id = "executive",
                Name = "Executive Summary",
                Description = "Condensed template focusing on key business highlights",
                IsDefault = false,
                SupportedFormats = new List<string> { "pdf", "docx" },
                SupportedLanguages = new List<string> { "fr", "en" }
            }
        };

        return Result.Success(templates);
    }

    private async Task<BusinessPlan?> GetBusinessPlanWithValidationAsync(Guid businessPlanId, CancellationToken cancellationToken)
    {
        var currentUserId = GetCurrentUserId();
        if (!currentUserId.HasValue)
        {
            return null;
        }

        var businessPlan = await _context.BusinessPlans
            .Include(bp => bp.Organization)
                .ThenInclude(o => o.Members)
            .Include(bp => bp.QuestionnaireResponses)
                .ThenInclude(qr => qr.QuestionTemplate)
            .FirstOrDefaultAsync(bp => bp.Id == businessPlanId && !bp.IsDeleted, cancellationToken);

        if (businessPlan == null)
        {
            return null;
        }

        // Verify user has access to this business plan
        var hasAccess = businessPlan.Organization.Members
            .Any(m => m.UserId == currentUserId.Value && m.IsActive);

        return hasAccess ? businessPlan : null;
    }

    private byte[] GeneratePdfWithQuestPDF(BusinessPlan businessPlan, string language)
    {
        return QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text($"{GetLocalizedText("Business Plan", language)}: {businessPlan.Title}")
                    .SemiBold().FontSize(16).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);

                        // Executive Summary
                        if (!string.IsNullOrEmpty(businessPlan.ExecutiveSummary))
                        {
                            x.Item().Text(GetLocalizedText("Executive Summary", language)).SemiBold().FontSize(14);
                            x.Item().Text(businessPlan.ExecutiveSummary);
                        }

                        // Problem Statement
                        if (!string.IsNullOrEmpty(businessPlan.ProblemStatement))
                        {
                            x.Item().Text(GetLocalizedText("Problem Statement", language)).SemiBold().FontSize(14);
                            x.Item().Text(businessPlan.ProblemStatement);
                        }

                        // Solution
                        if (!string.IsNullOrEmpty(businessPlan.Solution))
                        {
                            x.Item().Text(GetLocalizedText("Solution", language)).SemiBold().FontSize(14);
                            x.Item().Text(businessPlan.Solution);
                        }

                        // Market Analysis
                        if (!string.IsNullOrEmpty(businessPlan.MarketAnalysis))
                        {
                            x.Item().Text(GetLocalizedText("Market Analysis", language)).SemiBold().FontSize(14);
                            x.Item().Text(businessPlan.MarketAnalysis);
                        }

                        // Competitive Analysis
                        if (!string.IsNullOrEmpty(businessPlan.CompetitiveAnalysis))
                        {
                            x.Item().Text(GetLocalizedText("Competitive Analysis", language)).SemiBold().FontSize(14);
                            x.Item().Text(businessPlan.CompetitiveAnalysis);
                        }

                        // Financial Projections
                        if (!string.IsNullOrEmpty(businessPlan.FinancialProjections))
                        {
                            x.Item().Text(GetLocalizedText("Financial Projections", language)).SemiBold().FontSize(14);
                            x.Item().Text(businessPlan.FinancialProjections);
                        }

                        // Marketing Strategy
                        if (!string.IsNullOrEmpty(businessPlan.MarketingStrategy))
                        {
                            x.Item().Text(GetLocalizedText("Marketing Strategy", language)).SemiBold().FontSize(14);
                            x.Item().Text(businessPlan.MarketingStrategy);
                        }

                        // Management Team
                        if (!string.IsNullOrEmpty(businessPlan.ManagementTeam))
                        {
                            x.Item().Text(GetLocalizedText("Management Team", language)).SemiBold().FontSize(14);
                            x.Item().Text(businessPlan.ManagementTeam);
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span($"{GetLocalizedText("Generated on", language)}: ");
                        x.Span(DateTime.Now.ToString("MMMM dd, yyyy"));
                        x.Span(" | ");
                        x.Span($"{GetLocalizedText("Page", language)} ");
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
            });
        })
        .GeneratePdf();
    }

    private byte[] GenerateWordDocument(BusinessPlan businessPlan, string language)
    {
        using var memoryStream = new MemoryStream();
        using var wordDocument = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document);

        var mainPart = wordDocument.AddMainDocumentPart();
        mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
        var body = mainPart.Document.AppendChild(new Body());

        // Title
        var titleParagraph = new Paragraph();
        var titleRun = new Run();
        var titleText = new Text($"{GetLocalizedText("Business Plan", language)}: {businessPlan.Title}");
        titleRun.Append(titleText);
        titleParagraph.Append(titleRun);
        body.Append(titleParagraph);

        // Add sections
        AddWordSection(body, GetLocalizedText("Executive Summary", language), businessPlan.ExecutiveSummary);
        AddWordSection(body, GetLocalizedText("Problem Statement", language), businessPlan.ProblemStatement);
        AddWordSection(body, GetLocalizedText("Solution", language), businessPlan.Solution);
        AddWordSection(body, GetLocalizedText("Market Analysis", language), businessPlan.MarketAnalysis);
        AddWordSection(body, GetLocalizedText("Competitive Analysis", language), businessPlan.CompetitiveAnalysis);
        AddWordSection(body, GetLocalizedText("Financial Projections", language), businessPlan.FinancialProjections);
        AddWordSection(body, GetLocalizedText("Marketing Strategy", language), businessPlan.MarketingStrategy);
        AddWordSection(body, GetLocalizedText("Management Team", language), businessPlan.ManagementTeam);

        wordDocument.Save();
        return memoryStream.ToArray();
    }

    private static void AddWordSection(Body body, string heading, string? content)
    {
        if (string.IsNullOrEmpty(content)) return;

        // Add heading
        var headingParagraph = new Paragraph();
        var headingRun = new Run();
        var headingText = new Text(heading);
        headingRun.Append(headingText);
        headingParagraph.Append(headingRun);
        body.Append(headingParagraph);

        // Add content
        var contentParagraph = new Paragraph();
        var contentRun = new Run();
        var contentText = new Text(content);
        contentRun.Append(contentText);
        contentParagraph.Append(contentRun);
        body.Append(contentParagraph);

        // Add spacing
        body.Append(new Paragraph());
    }

    private string GenerateHtmlContent(BusinessPlan businessPlan, string language)
    {
        var html = new StringBuilder();
        html.AppendLine("<!DOCTYPE html>");
        html.AppendLine("<html>");
        html.AppendLine("<head>");
        html.AppendLine("<meta charset='UTF-8'>");
        html.AppendLine("<meta name='viewport' content='width=device-width, initial-scale=1.0'>");
        html.AppendLine($"<title>{GetLocalizedText("Business Plan", language)}: {businessPlan.Title}</title>");
        html.AppendLine("<style>");
        html.AppendLine(GetDefaultCss());
        html.AppendLine("</style>");
        html.AppendLine("</head>");
        html.AppendLine("<body>");

        html.AppendLine("<div class='container'>");
        html.AppendLine($"<h1>{GetLocalizedText("Business Plan", language)}: {businessPlan.Title}</h1>");

        AddHtmlSection(html, GetLocalizedText("Executive Summary", language), businessPlan.ExecutiveSummary);
        AddHtmlSection(html, GetLocalizedText("Problem Statement", language), businessPlan.ProblemStatement);
        AddHtmlSection(html, GetLocalizedText("Solution", language), businessPlan.Solution);
        AddHtmlSection(html, GetLocalizedText("Market Analysis", language), businessPlan.MarketAnalysis);
        AddHtmlSection(html, GetLocalizedText("Competitive Analysis", language), businessPlan.CompetitiveAnalysis);
        AddHtmlSection(html, GetLocalizedText("Financial Projections", language), businessPlan.FinancialProjections);
        AddHtmlSection(html, GetLocalizedText("Marketing Strategy", language), businessPlan.MarketingStrategy);
        AddHtmlSection(html, GetLocalizedText("Management Team", language), businessPlan.ManagementTeam);

        html.AppendLine("</div>");
        html.AppendLine("</body>");
        html.AppendLine("</html>");

        return html.ToString();
    }

    private static void AddHtmlSection(StringBuilder html, string heading, string? content)
    {
        if (string.IsNullOrEmpty(content)) return;

        html.AppendLine($"<section>");
        html.AppendLine($"<h2>{heading}</h2>");
        html.AppendLine($"<div class='content'>{content.Replace("\n", "<br>")}</div>");
        html.AppendLine("</section>");
    }

    private static string GetDefaultCss()
    {
        return @"
            body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; margin: 0; padding: 20px; background-color: #f5f5f5; }
            .container { max-width: 800px; margin: 0 auto; background: white; padding: 40px; box-shadow: 0 0 10px rgba(0,0,0,0.1); }
            h1 { color: #2c3e50; border-bottom: 3px solid #3498db; padding-bottom: 10px; }
            h2 { color: #34495e; margin-top: 30px; margin-bottom: 15px; }
            section { margin-bottom: 25px; }
            .content { text-align: justify; color: #2c3e50; }
            @media print { body { background: white; } .container { box-shadow: none; } }
        ";
    }

    private static string GetLocalizedText(string key, string language)
    {
        var translations = new Dictionary<string, Dictionary<string, string>>
        {
            ["Business Plan"] = new() { ["fr"] = "Plan d'Affaires", ["en"] = "Business Plan" },
            ["Executive Summary"] = new() { ["fr"] = "Résumé Exécutif", ["en"] = "Executive Summary" },
            ["Problem Statement"] = new() { ["fr"] = "Énoncé du Problème", ["en"] = "Problem Statement" },
            ["Solution"] = new() { ["fr"] = "Solution", ["en"] = "Solution" },
            ["Market Analysis"] = new() { ["fr"] = "Analyse de Marché", ["en"] = "Market Analysis" },
            ["Competitive Analysis"] = new() { ["fr"] = "Analyse Concurrentielle", ["en"] = "Competitive Analysis" },
            ["Financial Projections"] = new() { ["fr"] = "Projections Financières", ["en"] = "Financial Projections" },
            ["Marketing Strategy"] = new() { ["fr"] = "Stratégie Marketing", ["en"] = "Marketing Strategy" },
            ["Management Team"] = new() { ["fr"] = "Équipe de Direction", ["en"] = "Management Team" },
            ["Generated on"] = new() { ["fr"] = "Généré le", ["en"] = "Generated on" },
            ["Page"] = new() { ["fr"] = "Page", ["en"] = "Page" }
        };

        if (translations.TryGetValue(key, out var translation) &&
            translation.TryGetValue(language, out var text))
        {
            return text;
        }

        return key; // Fallback to key if translation not found
    }

    private static string SanitizeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        return string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
    }

    private Guid? GetCurrentUserId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.User?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}