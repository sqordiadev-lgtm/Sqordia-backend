using System.ComponentModel.DataAnnotations;

namespace Sqordia.Contracts.Requests.BusinessPlan;

public class UpdateSharePermissionRequest
{
    [Required]
    [Range(0, 2, ErrorMessage = "Permission must be 0 (ReadOnly), 1 (Edit), or 2 (FullAccess)")]
    public required int Permission { get; set; } // 0 = ReadOnly, 1 = Edit, 2 = FullAccess
}

