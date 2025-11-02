using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sqordia.Application.Common.Interfaces;
using Sqordia.Application.Common.Models;
using Sqordia.Contracts.Requests.BusinessPlan;
using Sqordia.Contracts.Responses.BusinessPlan;
using Sqordia.Domain.Enums;
using System.Security.Claims;

namespace Sqordia.Application.Services.Implementations;

public class BusinessPlanService : IBusinessPlanService
{
    private readonly IApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<BusinessPlanService> _logger;
    private readonly ILocalizationService _localizationService;

    public BusinessPlanService(
        IApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ILogger<BusinessPlanService> logger,
        ILocalizationService localizationService)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _localizationService = localizationService;
    }

    private Guid? GetCurrentUserId()
    {
        var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            return null;
        }
        return Guid.TryParse(userIdString, out var userId) ? userId : null;
    }

    public async Task<Result<BusinessPlanResponse>> CreateBusinessPlanAsync(CreateBusinessPlanRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                return Result.Failure<BusinessPlanResponse>(Error.Unauthorized("General.Unauthorized", _localizationService.GetString("General.Unauthorized")));
            }

            // Validate organization exists
            var organization = await _context.Organizations
                .FirstOrDefaultAsync(o => o.Id == request.OrganizationId, cancellationToken);

            if (organization == null)
            {
                return Result.Failure<BusinessPlanResponse>(Error.NotFound("Organization.Error.NotFound", _localizationService.GetString("Organization.Error.NotFound")));
            }

            // Verify user is a member of the organization
            var isMember = await _context.OrganizationMembers
                .AnyAsync(om => om.OrganizationId == request.OrganizationId && 
                               om.UserId == currentUserId.Value && 
                               om.IsActive, cancellationToken);

            if (!isMember)
            {
                return Result.Failure<BusinessPlanResponse>(Error.Forbidden("Organization.Error.Forbidden", _localizationService.GetString("Organization.Error.Forbidden")));
            }

            // Parse business plan type
            if (!Enum.TryParse<BusinessPlanType>(request.PlanType, out var planType))
            {
                return Result.Failure<BusinessPlanResponse>(Error.Validation("BusinessPlan.InvalidPlanType", $"Invalid plan type '{request.PlanType}'. Valid values are: BusinessPlan, StrategicPlan, LeanCanvas"));
            }

            // Create business plan
            var businessPlan = new Domain.Entities.BusinessPlan.BusinessPlan(
                request.Title,
                planType,
                request.OrganizationId,
                request.Description);

            businessPlan.CreatedBy = currentUserId.Value.ToString();

            _context.BusinessPlans.Add(businessPlan);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Business plan {PlanId} created by user {UserId} for organization {OrgId}",
                businessPlan.Id, currentUserId.Value, request.OrganizationId);

            // Map to response
            var response = new BusinessPlanResponse
            {
                Id = businessPlan.Id,
                Title = businessPlan.Title,
                Description = businessPlan.Description,
                PlanType = businessPlan.PlanType.ToString(),
                Status = businessPlan.Status.ToString(),
                OrganizationId = businessPlan.OrganizationId,
                OrganizationName = organization.Name,
                Version = businessPlan.Version,
                TotalQuestions = businessPlan.TotalQuestions,
                CompletedQuestions = businessPlan.CompletedQuestions,
                CompletionPercentage = businessPlan.CompletionPercentage,
                QuestionnaireCompletedAt = businessPlan.QuestionnaireCompletedAt,
                GenerationStartedAt = businessPlan.GenerationStartedAt,
                GenerationCompletedAt = businessPlan.GenerationCompletedAt,
                FinalizedAt = businessPlan.FinalizedAt,
                Created = businessPlan.Created,
                LastModified = businessPlan.LastModified,
                CreatedBy = businessPlan.CreatedBy
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating business plan");
            return Result.Failure<BusinessPlanResponse>(Error.InternalServerError("General.InternalServerError", _localizationService.GetString("General.InternalServerError")));
        }
    }

    public async Task<Result<BusinessPlanResponse>> GetBusinessPlanAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                return Result.Failure<BusinessPlanResponse>(Error.Unauthorized("General.Unauthorized", _localizationService.GetString("General.Unauthorized")));
            }

            var businessPlan = await _context.BusinessPlans
                .Include(bp => bp.Organization)
                .FirstOrDefaultAsync(bp => bp.Id == id, cancellationToken);

            if (businessPlan == null)
            {
                return Result.Failure<BusinessPlanResponse>(Error.NotFound("BusinessPlan.Error.NotFound", _localizationService.GetString("BusinessPlan.Error.NotFound")));
            }

            // Verify user has access to this business plan (member of organization)
            var isMember = await _context.OrganizationMembers
                .AnyAsync(om => om.OrganizationId == businessPlan.OrganizationId && 
                               om.UserId == currentUserId.Value && 
                               om.IsActive, cancellationToken);

            if (!isMember)
            {
                return Result.Failure<BusinessPlanResponse>(Error.Forbidden("BusinessPlan.Error.Forbidden", _localizationService.GetString("BusinessPlan.Error.Forbidden")));
            }

            var response = new BusinessPlanResponse
            {
                Id = businessPlan.Id,
                Title = businessPlan.Title,
                Description = businessPlan.Description,
                PlanType = businessPlan.PlanType.ToString(),
                Status = businessPlan.Status.ToString(),
                OrganizationId = businessPlan.OrganizationId,
                OrganizationName = businessPlan.Organization.Name,
                Version = businessPlan.Version,
                TotalQuestions = businessPlan.TotalQuestions,
                CompletedQuestions = businessPlan.CompletedQuestions,
                CompletionPercentage = businessPlan.CompletionPercentage,
                QuestionnaireCompletedAt = businessPlan.QuestionnaireCompletedAt,
                GenerationStartedAt = businessPlan.GenerationStartedAt,
                GenerationCompletedAt = businessPlan.GenerationCompletedAt,
                FinalizedAt = businessPlan.FinalizedAt,
                Created = businessPlan.Created,
                LastModified = businessPlan.LastModified,
                CreatedBy = businessPlan.CreatedBy
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving business plan {PlanId}", id);
            return Result.Failure<BusinessPlanResponse>(Error.InternalServerError("General.InternalServerError", _localizationService.GetString("General.InternalServerError")));
        }
    }

    public async Task<Result<IEnumerable<BusinessPlanResponse>>> GetOrganizationBusinessPlansAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                return Result.Failure<IEnumerable<BusinessPlanResponse>>(Error.Unauthorized("General.Unauthorized", _localizationService.GetString("General.Unauthorized")));
            }

            // Verify user is a member of the organization
            var isMember = await _context.OrganizationMembers
                .AnyAsync(om => om.OrganizationId == organizationId && 
                               om.UserId == currentUserId.Value && 
                               om.IsActive, cancellationToken);

            if (!isMember)
            {
                return Result.Failure<IEnumerable<BusinessPlanResponse>>(Error.Forbidden("Organization.Error.Forbidden", _localizationService.GetString("Organization.Error.Forbidden")));
            }

            var businessPlans = await _context.BusinessPlans
                .Include(bp => bp.Organization)
                .Where(bp => bp.OrganizationId == organizationId)
                .OrderByDescending(bp => bp.Created)
                .ToListAsync(cancellationToken);

            var responses = businessPlans.Select(bp => new BusinessPlanResponse
            {
                Id = bp.Id,
                Title = bp.Title,
                Description = bp.Description,
                PlanType = bp.PlanType.ToString(),
                Status = bp.Status.ToString(),
                OrganizationId = bp.OrganizationId,
                OrganizationName = bp.Organization.Name,
                Version = bp.Version,
                TotalQuestions = bp.TotalQuestions,
                CompletedQuestions = bp.CompletedQuestions,
                CompletionPercentage = bp.CompletionPercentage,
                QuestionnaireCompletedAt = bp.QuestionnaireCompletedAt,
                GenerationStartedAt = bp.GenerationStartedAt,
                GenerationCompletedAt = bp.GenerationCompletedAt,
                FinalizedAt = bp.FinalizedAt,
                Created = bp.Created,
                LastModified = bp.LastModified,
                CreatedBy = bp.CreatedBy
            });

            return Result.Success(responses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving business plans for organization {OrgId}", organizationId);
            return Result.Failure<IEnumerable<BusinessPlanResponse>>(Error.InternalServerError("General.InternalServerError", _localizationService.GetString("General.InternalServerError")));
        }
    }

    public async Task<Result<BusinessPlanResponse>> UpdateBusinessPlanAsync(Guid id, UpdateBusinessPlanRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                return Result.Failure<BusinessPlanResponse>(Error.Unauthorized("General.Unauthorized", _localizationService.GetString("General.Unauthorized")));
            }

            var businessPlan = await _context.BusinessPlans
                .Include(bp => bp.Organization)
                .FirstOrDefaultAsync(bp => bp.Id == id, cancellationToken);

            if (businessPlan == null)
            {
                return Result.Failure<BusinessPlanResponse>(Error.NotFound("BusinessPlan.Error.NotFound", _localizationService.GetString("BusinessPlan.Error.NotFound")));
            }

            // Verify user has access
            var member = await _context.OrganizationMembers
                .FirstOrDefaultAsync(om => om.OrganizationId == businessPlan.OrganizationId && 
                                          om.UserId == currentUserId.Value && 
                                          om.IsActive, cancellationToken);

            if (member == null)
            {
                return Result.Failure<BusinessPlanResponse>(Error.Forbidden("BusinessPlan.Error.Forbidden", _localizationService.GetString("BusinessPlan.Error.Forbidden")));
            }

            // Only Owner or Admin can update
            if (member.Role != OrganizationRole.Owner && member.Role != OrganizationRole.Admin)
            {
                return Result.Failure<BusinessPlanResponse>(Error.Forbidden("BusinessPlan.Error.Forbidden", _localizationService.GetString("BusinessPlan.Error.Forbidden")));
            }

            // Update fields
            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                businessPlan.UpdateTitle(request.Title);
            }

            if (request.Description != null)
            {
                businessPlan.UpdateDescription(request.Description);
            }

            businessPlan.LastModifiedBy = currentUserId.Value.ToString();
            businessPlan.IncrementVersion();

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Business plan {PlanId} updated by user {UserId}", id, currentUserId.Value);

            var response = new BusinessPlanResponse
            {
                Id = businessPlan.Id,
                Title = businessPlan.Title,
                Description = businessPlan.Description,
                PlanType = businessPlan.PlanType.ToString(),
                Status = businessPlan.Status.ToString(),
                OrganizationId = businessPlan.OrganizationId,
                OrganizationName = businessPlan.Organization.Name,
                Version = businessPlan.Version,
                TotalQuestions = businessPlan.TotalQuestions,
                CompletedQuestions = businessPlan.CompletedQuestions,
                CompletionPercentage = businessPlan.CompletionPercentage,
                QuestionnaireCompletedAt = businessPlan.QuestionnaireCompletedAt,
                GenerationStartedAt = businessPlan.GenerationStartedAt,
                GenerationCompletedAt = businessPlan.GenerationCompletedAt,
                FinalizedAt = businessPlan.FinalizedAt,
                Created = businessPlan.Created,
                LastModified = businessPlan.LastModified,
                CreatedBy = businessPlan.CreatedBy
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating business plan {PlanId}", id);
            return Result.Failure<BusinessPlanResponse>(Error.InternalServerError("General.InternalServerError", _localizationService.GetString("General.InternalServerError")));
        }
    }

    public async Task<Result> DeleteBusinessPlanAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                return Result.Failure(Error.Unauthorized("General.Unauthorized", _localizationService.GetString("General.Unauthorized")));
            }

            var businessPlan = await _context.BusinessPlans
                .FirstOrDefaultAsync(bp => bp.Id == id, cancellationToken);

            if (businessPlan == null)
            {
                return Result.Failure(Error.NotFound("BusinessPlan.Error.NotFound", _localizationService.GetString("BusinessPlan.Error.NotFound")));
            }

            // Verify user has access and is Owner or Admin
            var member = await _context.OrganizationMembers
                .FirstOrDefaultAsync(om => om.OrganizationId == businessPlan.OrganizationId && 
                                          om.UserId == currentUserId.Value && 
                                          om.IsActive, cancellationToken);

            if (member == null)
            {
                return Result.Failure(Error.Forbidden("BusinessPlan.Error.Forbidden", _localizationService.GetString("BusinessPlan.Error.Forbidden")));
            }

            if (member.Role != OrganizationRole.Owner && member.Role != OrganizationRole.Admin)
            {
                return Result.Failure(Error.Forbidden("BusinessPlan.Error.Forbidden", _localizationService.GetString("BusinessPlan.Error.Forbidden")));
            }

            _context.BusinessPlans.Remove(businessPlan);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Business plan {PlanId} deleted by user {UserId}", id, currentUserId.Value);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting business plan {PlanId}", id);
            return Result.Failure(Error.InternalServerError("General.InternalServerError", _localizationService.GetString("General.InternalServerError")));
        }
    }

    public async Task<Result<BusinessPlanResponse>> ArchiveBusinessPlanAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                return Result.Failure<BusinessPlanResponse>(Error.Unauthorized("General.Unauthorized", _localizationService.GetString("General.Unauthorized")));
            }

            var businessPlan = await _context.BusinessPlans
                .Include(bp => bp.Organization)
                .FirstOrDefaultAsync(bp => bp.Id == id, cancellationToken);

            if (businessPlan == null)
            {
                return Result.Failure<BusinessPlanResponse>(Error.NotFound("BusinessPlan.Error.NotFound", _localizationService.GetString("BusinessPlan.Error.NotFound")));
            }

            // Verify user has access
            var member = await _context.OrganizationMembers
                .FirstOrDefaultAsync(om => om.OrganizationId == businessPlan.OrganizationId && 
                                          om.UserId == currentUserId.Value && 
                                          om.IsActive, cancellationToken);

            if (member == null)
            {
                return Result.Failure<BusinessPlanResponse>(Error.Forbidden("BusinessPlan.Error.Forbidden", _localizationService.GetString("BusinessPlan.Error.Forbidden")));
            }

            if (member.Role != OrganizationRole.Owner && member.Role != OrganizationRole.Admin)
            {
                return Result.Failure<BusinessPlanResponse>(Error.Forbidden("BusinessPlan.Error.Forbidden", _localizationService.GetString("BusinessPlan.Error.Forbidden")));
            }

            businessPlan.Archive();
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Business plan {PlanId} archived by user {UserId}", id, currentUserId.Value);

            var response = new BusinessPlanResponse
            {
                Id = businessPlan.Id,
                Title = businessPlan.Title,
                Description = businessPlan.Description,
                PlanType = businessPlan.PlanType.ToString(),
                Status = businessPlan.Status.ToString(),
                OrganizationId = businessPlan.OrganizationId,
                OrganizationName = businessPlan.Organization.Name,
                Version = businessPlan.Version,
                TotalQuestions = businessPlan.TotalQuestions,
                CompletedQuestions = businessPlan.CompletedQuestions,
                CompletionPercentage = businessPlan.CompletionPercentage,
                QuestionnaireCompletedAt = businessPlan.QuestionnaireCompletedAt,
                GenerationStartedAt = businessPlan.GenerationStartedAt,
                GenerationCompletedAt = businessPlan.GenerationCompletedAt,
                FinalizedAt = businessPlan.FinalizedAt,
                Created = businessPlan.Created,
                LastModified = businessPlan.LastModified,
                CreatedBy = businessPlan.CreatedBy
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error archiving business plan {PlanId}", id);
            return Result.Failure<BusinessPlanResponse>(Error.InternalServerError("General.InternalServerError", _localizationService.GetString("General.InternalServerError")));
        }
    }

    public async Task<Result<BusinessPlanResponse>> UnarchiveBusinessPlanAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                return Result.Failure<BusinessPlanResponse>(Error.Unauthorized("General.Unauthorized", _localizationService.GetString("General.Unauthorized")));
            }

            var businessPlan = await _context.BusinessPlans
                .Include(bp => bp.Organization)
                .FirstOrDefaultAsync(bp => bp.Id == id, cancellationToken);

            if (businessPlan == null)
            {
                return Result.Failure<BusinessPlanResponse>(Error.NotFound("BusinessPlan.Error.NotFound", _localizationService.GetString("BusinessPlan.Error.NotFound")));
            }

            // Verify user has access
            var member = await _context.OrganizationMembers
                .FirstOrDefaultAsync(om => om.OrganizationId == businessPlan.OrganizationId && 
                                          om.UserId == currentUserId.Value && 
                                          om.IsActive, cancellationToken);

            if (member == null)
            {
                return Result.Failure<BusinessPlanResponse>(Error.Forbidden("BusinessPlan.Error.Forbidden", _localizationService.GetString("BusinessPlan.Error.Forbidden")));
            }

            if (member.Role != OrganizationRole.Owner && member.Role != OrganizationRole.Admin)
            {
                return Result.Failure<BusinessPlanResponse>(Error.Forbidden("BusinessPlan.Error.Forbidden", _localizationService.GetString("BusinessPlan.Error.Forbidden")));
            }

            businessPlan.Unarchive();
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Business plan {PlanId} unarchived by user {UserId}", id, currentUserId.Value);

            var response = new BusinessPlanResponse
            {
                Id = businessPlan.Id,
                Title = businessPlan.Title,
                Description = businessPlan.Description,
                PlanType = businessPlan.PlanType.ToString(),
                Status = businessPlan.Status.ToString(),
                OrganizationId = businessPlan.OrganizationId,
                OrganizationName = businessPlan.Organization.Name,
                Version = businessPlan.Version,
                TotalQuestions = businessPlan.TotalQuestions,
                CompletedQuestions = businessPlan.CompletedQuestions,
                CompletionPercentage = businessPlan.CompletionPercentage,
                QuestionnaireCompletedAt = businessPlan.QuestionnaireCompletedAt,
                GenerationStartedAt = businessPlan.GenerationStartedAt,
                GenerationCompletedAt = businessPlan.GenerationCompletedAt,
                FinalizedAt = businessPlan.FinalizedAt,
                Created = businessPlan.Created,
                LastModified = businessPlan.LastModified,
                CreatedBy = businessPlan.CreatedBy
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unarchiving business plan {PlanId}", id);
            return Result.Failure<BusinessPlanResponse>(Error.InternalServerError("General.InternalServerError", _localizationService.GetString("General.InternalServerError")));
        }
    }

    public async Task<Result<IEnumerable<BusinessPlanResponse>>> GetUserBusinessPlansAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                return Result.Failure<IEnumerable<BusinessPlanResponse>>(Error.Unauthorized("General.Unauthorized", _localizationService.GetString("General.Unauthorized")));
            }

            // Get all organizations where the user is a member
            var userOrganizations = await _context.OrganizationMembers
                .Where(om => om.UserId == currentUserId.Value && om.IsActive)
                .Select(om => om.OrganizationId)
                .ToListAsync(cancellationToken);

            if (!userOrganizations.Any())
            {
                return Result.Success<IEnumerable<BusinessPlanResponse>>(new List<BusinessPlanResponse>());
            }

            // Get all business plans for these organizations
            var businessPlans = await _context.BusinessPlans
                .Include(bp => bp.Organization)
                .Where(bp => userOrganizations.Contains(bp.OrganizationId))
                .OrderByDescending(bp => bp.Created)
                .ToListAsync(cancellationToken);

            var responses = businessPlans.Select(bp => new BusinessPlanResponse
            {
                Id = bp.Id,
                Title = bp.Title,
                Description = bp.Description,
                PlanType = bp.PlanType.ToString(),
                Status = bp.Status.ToString(),
                OrganizationId = bp.OrganizationId,
                OrganizationName = bp.Organization.Name,
                Version = bp.Version,
                TotalQuestions = bp.TotalQuestions,
                CompletedQuestions = bp.CompletedQuestions,
                CompletionPercentage = bp.CompletionPercentage,
                QuestionnaireCompletedAt = bp.QuestionnaireCompletedAt,
                GenerationStartedAt = bp.GenerationStartedAt,
                GenerationCompletedAt = bp.GenerationCompletedAt,
                FinalizedAt = bp.FinalizedAt,
                Created = bp.Created,
                LastModified = bp.LastModified,
                CreatedBy = bp.CreatedBy
            }).ToList();

            _logger.LogInformation("Retrieved {Count} business plans for user {UserId}", responses.Count, currentUserId.Value);
            return Result.Success<IEnumerable<BusinessPlanResponse>>(responses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving business plans for user");
            return Result.Failure<IEnumerable<BusinessPlanResponse>>(Error.InternalServerError("General.InternalServerError", _localizationService.GetString("General.InternalServerError")));
        }
    }
}

